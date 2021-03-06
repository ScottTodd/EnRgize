﻿using UnityEngine;
using System.Collections;

public class TouchWallController : MonoBehaviour
{
    public GameObject wallObject; // Wall with physics properties
    public GameObject reticlePrefab;
    public GameObject persistentReticlePrefab;
    public float activateDelay = 1.0f;
    public float timeUntilDisable = 3.0f; // seconds
    public float minimumDeltaDistanceToUpdate = 1.0f;

    private GameController gameController;

    private Vector3 activeWorldPosition1;
    private Vector3 activeWorldPosition2;
    private Vector3 currentWorldPosition1;
    private Vector3 currentWorldPosition2;
    private float deltaDistance;

    private LightningLine lightningLineScript;
    private bool inputDetected;
    private float latestInputTime;
    private bool wallActive = false;
    private bool spawning = false;
    private float latestActivateTime;
    
    // Platform dependent variables
    #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
    private int currentInputPosition = 1;
    private GameObject firstInputReticle;
    #else
    
    #endif

    void Start() {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();

        lightningLineScript = (LightningLine) transform.GetComponent<LightningLine>();
        DisableWall();
    }
    
    void EnableWall() {
        wallActive = true;
        wallObject.SetActive(true);
        lightningLineScript.insideParentObject.SetActive(true);
    }
    
    void DisableWall() {
        wallActive = false;
        wallObject.SetActive(false);
        lightningLineScript.insideParentObject.SetActive(false);
    }

    void QueryCurrentPositions() {
        inputDetected = false;

        // Attempt to get two world positions, input source dependent on platform
        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePosition = Input.mousePosition;

            if (gameController.ScreenPointWithinGUI(mousePosition)) {
                return;
            }

            if (currentInputPosition == 1) {
                DisableWall();

                currentWorldPosition1 = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
                currentWorldPosition1.z = 0;
                currentInputPosition = 2;

                GameObject tempObjectParent = GameObject.FindGameObjectWithTag("TempObjectParent");
                firstInputReticle = (GameObject) Instantiate(persistentReticlePrefab);
                firstInputReticle.transform.parent = tempObjectParent.transform;
                firstInputReticle.transform.position = currentWorldPosition1;
            } else {
                currentWorldPosition2 = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
                currentWorldPosition2.z = 0;
                currentInputPosition = 1;

                inputDetected = true;
                latestInputTime = Time.time;
                Destroy(firstInputReticle);
            }
        }
        #else
        if (Input.touchCount >= 2) {
            inputDetected = true;
            latestInputTime = Time.time;

            if (gameController.ScreenPointWithinGUI(Input.GetTouch(0).position) ||
                gameController.ScreenPointWithinGUI(Input.GetTouch(1).position)) {
                return;
            }

            Vector2 touchPosition1 = Input.GetTouch(0).position;
            currentWorldPosition1 = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition1.x, touchPosition1.y, 0));
            currentWorldPosition1.z = 0;
            
            Vector2 touchPosition2 = Input.GetTouch(1).position;
            currentWorldPosition2 = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition2.x, touchPosition2.y, 0));
            currentWorldPosition2.z = 0;
        }
        #endif
    }

    void StartActivateDelay() {
        spawning = true;
        DisableWall();

        GameObject tempObjectParent = GameObject.FindGameObjectWithTag("TempObjectParent");

        GameObject reticle1 = (GameObject) Instantiate(reticlePrefab);
        reticle1.transform.parent = tempObjectParent.transform;
        reticle1.transform.position = activeWorldPosition1;

        GameObject reticle2 = (GameObject) Instantiate(reticlePrefab);
        reticle2.transform.parent = tempObjectParent.transform;
        reticle2.transform.position = activeWorldPosition2;
    }

    void ApplyActivePositions() {
        latestActivateTime = Time.time;

        // Set position as the center of the two world positions
        Vector3 centerPosition = (activeWorldPosition1 + activeWorldPosition2) / 2;
        transform.localPosition = centerPosition;
        
        // Fix position of wallObject, despite physics interactions
        wallObject.transform.localPosition = new Vector3(0,0,0);
        
        // Set angle as the angle from the first world position to the second
        Vector3 differencePosition = activeWorldPosition2 - activeWorldPosition1;
        float angle = Vector3.Angle(Vector3.right, differencePosition);
        float sign = (Vector3.Dot(Vector3.up, differencePosition) > 0.0f) ? 1.0f: -1.0f;
        transform.localEulerAngles = new Vector3(0, 0, angle * sign);
        
        // Scale wallObject by the distance between the two world positions
        float distance = Vector3.Distance(activeWorldPosition1, activeWorldPosition2);
        Vector3 localScaleCopy = wallObject.transform.localScale;
        localScaleCopy.x = distance;
        wallObject.transform.localScale = localScaleCopy;
        
        // Scale lightningLine by the distance as well
        lightningLineScript.startPosition.x = -distance / 2.0f;
        lightningLineScript.endPosition.x   =  distance / 2.0f;
        lightningLineScript.UpdateLightningBolts();
    }

    void Update() {
        QueryCurrentPositions();

        if (inputDetected) {
            float distance1 = Vector3.Distance(currentWorldPosition1, activeWorldPosition1);
            float distance2 = Vector3.Distance(currentWorldPosition2, activeWorldPosition2);
            deltaDistance = distance1 + distance2;

            #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (!wallActive || deltaDistance > minimumDeltaDistanceToUpdate) {
                activeWorldPosition1 = currentWorldPosition1;
                activeWorldPosition2 = currentWorldPosition2;
                StartActivateDelay();
            }
            #else
            if (!wallActive || deltaDistance > minimumDeltaDistanceToUpdate) {
                activeWorldPosition1 = currentWorldPosition1;
                activeWorldPosition2 = currentWorldPosition2;
                StartActivateDelay();
            }
            #endif
        } else {
            #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (wallActive && Time.time - latestActivateTime > timeUntilDisable) {
                DisableWall();
            }
            #else
            if (wallActive && Time.time - latestActivateTime > timeUntilDisable) {
                DisableWall();
            }
            #endif
        }

        if (spawning) {
            // Confirm that the correct amount of time has passed (no new input)
            if (Time.time - latestInputTime > activateDelay) {
                EnableWall();
                ApplyActivePositions();
                spawning = false;
            }
        }
    }
}

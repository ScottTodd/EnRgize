using UnityEngine;
using System.Collections;

public class TouchWallController : MonoBehaviour
{
    public GameObject wallObject; // Wall with physics properties
    public float timeUntilDisable = 3.0f; // seconds
    public float minimumDeltaDistanceToUpdate = 1.0f;

    // public so these can be observed in the inspector, not for designer editting
    public Vector3 activeWorldPosition1;
    public Vector3 activeWorldPosition2;
    public Vector3 currentWorldPosition1;
    public Vector3 currentWorldPosition2;
    public float deltaDistance;

    private LightningLine lightningLineScript;
    private bool inputDetected;
    private float latestInputTime;

    void Start() {
        lightningLineScript = (LightningLine) transform.GetComponent<LightningLine>();
    }
    
    void EnableWall() {
        wallObject.SetActive(true);
        lightningLineScript.insideParentObject.SetActive(true);
    }
    
    void DisableWall() {
        wallObject.SetActive(false);
        lightningLineScript.insideParentObject.SetActive(false);
    }

    void QueryCurrentPositions() {
        inputDetected = false;

        // Attempt to get two world positions, input source dependent on platform
        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        if (Input.GetMouseButtonDown(0)) {
            inputDetected = true;
            latestInputTime = Time.time;

            Vector2 mousePosition = Input.mousePosition;
            currentWorldPosition1 = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
            currentWorldPosition1.z = 0;
            
            currentWorldPosition2 = new Vector3(-4, 2, 0);
        }
        #else
        if (Input.touchCount >= 2) {
            inputDetected = true;
            latestInputTime = Time.time;

            Vector2 touchPosition1 = Input.GetTouch(0).position;
            currentWorldPosition1 = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition1.x, touchPosition1.y, 0));
            currentWorldPosition1.z = 0;
            
            Vector2 touchPosition2 = Input.GetTouch(1).position;
            currentWorldPosition2 = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition2.x, touchPosition2.y, 0));
            currentWorldPosition2.z = 0;
        }
        #endif
    }

    void ApplyActivePositions() {
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
    }

    void Update() {
        QueryCurrentPositions();

        if (inputDetected) {
            float distance1 = Vector3.Distance(currentWorldPosition1, activeWorldPosition1);
            float distance2 = Vector3.Distance(currentWorldPosition2, activeWorldPosition2);
            deltaDistance = distance1 + distance2;

            #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (deltaDistance > minimumDeltaDistanceToUpdate) {
                EnableWall();
                activeWorldPosition1 = currentWorldPosition1;
                activeWorldPosition2 = currentWorldPosition2;
                ApplyActivePositions();
            }
            #else
            if (deltaDistance > minimumDeltaDistanceToUpdate) {
                EnableWall();
                activeWorldPosition1 = currentWorldPosition1;
                activeWorldPosition2 = currentWorldPosition2;
                ApplyActivePositions();
            }
            #endif
        } else {
            #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Time.time - latestInputTime > timeUntilDisable) {
                DisableWall();
            }
            #else
            if (Time.time - latestInputTime > timeUntilDisable) {
                DisableWall();
            }
            #endif
        }
    }
}

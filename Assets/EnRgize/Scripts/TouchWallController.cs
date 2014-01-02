using UnityEngine;
using System.Collections;

public class TouchWallController : MonoBehaviour
{
    public GameObject wallObject;

    private LightningLine lightningLineScript;
    private Vector3 worldPosition1;
    private Vector3 worldPosition2;
    private Vector3 centerPosition;
    private Vector3 differencePosition;
    private float angle;
    private float sign;
    private float distance;

    void Start() {
        lightningLineScript = (LightningLine) transform.GetComponent<LightningLine>();
    }

    void Update() {
        // Get two world positions, input source dependent on platform
        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Input.GetMouseButtonDown(0)) {
                Vector2 mousePosition = Input.mousePosition;
                worldPosition1 = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
                worldPosition1.z = 0;
                
                worldPosition2 = new Vector3(-4, 2, 0);
            }
        #else
            if (Input.touchCount >= 2) {
                Vector2 touchPosition1 = Input.GetTouch(0).position;
                worldPosition1 = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition1.x, touchPosition1.y, 0));
                worldPosition1.z = 0;
                
                Vector2 touchPosition2 = Input.GetTouch(1).position;
                worldPosition2 = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition2.x, touchPosition2.y, 0));
                worldPosition2.z = 0;
            }
        #endif

        // Set position as the center of the two world positions
        centerPosition = (worldPosition1 + worldPosition2) / 2;
        transform.localPosition = centerPosition;

        // Fix position of wallObject, despite physics interactions
        wallObject.transform.localPosition = new Vector3(0,0,0);

        // Set angle as the angle from the first world position to the second
        differencePosition = worldPosition2 - worldPosition1;
        angle = Vector3.Angle(Vector3.right, differencePosition);
        sign = (Vector3.Dot(Vector3.up, differencePosition) > 0.0f) ? 1.0f: -1.0f;
        transform.localEulerAngles = new Vector3(0, 0, angle * sign);

        // Scale wallObject by the distance between the two world positions
        distance = Vector3.Distance(worldPosition1, worldPosition2);
        Vector3 localScaleCopy = wallObject.transform.localScale;
        localScaleCopy.x = distance;
        wallObject.transform.localScale = localScaleCopy;

        // Scale lightningLine by the distance as well
        lightningLineScript.startPosition.x = -distance / 2.0f;
        lightningLineScript.endPosition.x   =  distance / 2.0f;
    }
}

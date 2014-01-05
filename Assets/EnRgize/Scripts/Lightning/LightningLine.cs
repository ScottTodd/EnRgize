using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightningLine : MonoBehaviour
{
    public GameObject lightningBoltPrefab;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public bool inferPositions = false;
    public GameObject inferFromObject;
    public float thickness = 2.0f;
    public int numBolts = 10;
    public Color tintColor;

    // Parent object to each LightningBolt, exposed to other scripts
    [HideInInspector] public GameObject insideParentObject;
    
    public float updateRate = 1.0f / 60.0f; // seconds between updates
    private float lastUpdateTime;
    
    private List<GameObject> lightningBolts;
    
    void Awake() {
        lightningBolts = new List<GameObject>();
    }
    
    void Start() {
        insideParentObject = new GameObject("Bolts Inside");
        insideParentObject.transform.parent = transform;
        insideParentObject.transform.localPosition = new Vector3(0,0,0);
        insideParentObject.transform.localEulerAngles = new Vector3(0,0,0);

        if (inferPositions) {
            float distance = inferFromObject.transform.localScale.x;
            startPosition = new Vector2(-distance/2.0f, 0);
            endPosition   = new Vector2( distance/2.0f, 0);
        }

        CreateNewLightningBolts();
    }
    
    void CreateNewLightningBolts() {
        // Delete old LightningBolts
        for (int i = 0; i < lightningBolts.Count; i++) {
            Destroy(lightningBolts[i]);
        }
        lightningBolts.Clear();
        
        // Create new LightningBolts inside
        for (int i = 0; i < numBolts + 2; i++) {
            GameObject lightningBolt = (GameObject) Instantiate(lightningBoltPrefab);
            lightningBolt.transform.parent = insideParentObject.transform;
            lightningBolt.transform.localPosition = new Vector3(0,0,0);
            lightningBolt.transform.localScale = new Vector3(1,1,1);
            lightningBolt.transform.localEulerAngles = new Vector3(0,0,0);

            LightningBolt boltScript = (LightningBolt) lightningBolt.GetComponent<LightningBolt>();
            boltScript.tintColor = tintColor;
            
            lightningBolts.Add(lightningBolt);
        }
        
        UpdateLightningBolts();
    }
    
    void UpdateLightningBolts() {
        // The first two bolts are from start to end and end to start
        GameObject lightningBolt;
        LightningBolt boltScript;

        lightningBolt = lightningBolts[0];
        boltScript = (LightningBolt) lightningBolt.GetComponent<LightningBolt>();
        boltScript.startPosition = startPosition;
        boltScript.endPosition = endPosition;

        lightningBolt = lightningBolts[1];
        boltScript = (LightningBolt) lightningBolt.GetComponent<LightningBolt>();
        boltScript.startPosition = endPosition;
        boltScript.endPosition = startPosition;

        // The next numBolts lightningBolts are from random start and end positions
        //          X
        //          | <- randomOffset
        // A --------------------- B
        // |--------| <-- randomStep
        
        // Compute information about this line
        Vector2 difference = endPosition - startPosition;
        Vector2 normal = new Vector2(difference.y, -difference.x).normalized;
        
        // Declare variables used within the loops
        float randomStepPercent1, randomStepPercent2;
        float randomOffset1, randomOffset2;
        Vector2 startOnLine, endOnLine;
        Vector2 startOffset, endOffset;
        
        // Create lightning bolts to and from random points around this line
        for (int i = 0; i < numBolts; i++) {
            // calculate start position (random spot around line)
            randomStepPercent1  = Random.value;
            randomOffset1 = Random.Range(-1.0f, 1.0f) * thickness;
            startOnLine = startPosition + difference * randomStepPercent1;
            startOffset = startOnLine + normal * randomOffset1;
            
            // calculate end position (random spot around line)
            randomStepPercent2  = Random.value;
            randomOffset2 = Random.Range(-1.0f, 1.0f) * thickness;
            endOnLine = startPosition + difference * randomStepPercent2;
            endOffset = endOnLine + normal * randomOffset2;
                        
            // Set positions in LightningBolt script
            lightningBolt = lightningBolts[i+2];
            boltScript = (LightningBolt) lightningBolt.GetComponent<LightningBolt>();
            boltScript.startPosition = startOffset;
            boltScript.endPosition = endOffset;
        }
    }
    
    void Update() {
        if (Time.time - lastUpdateTime > updateRate) {
            UpdateLightningBolts();
            lastUpdateTime = Time.time;
        }
    }
}

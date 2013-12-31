using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightningBall : MonoBehaviour
{
    public GameObject lightningBoltPrefab;
    public float radius = 2.0f;
    public int numBoltsInside = 10;
    public int numBoltsOutside = 10;

    public float updateRate = 1.0f / 60.0f; // seconds between updates
    private float lastUpdateTime;

    private List<GameObject> lightningBoltsInside;
    private List<GameObject> lightningBoltsOutside;

    void Awake() {
        lightningBoltsInside = new List<GameObject>();
        lightningBoltsOutside = new List<GameObject>();
    }

    void Start() {
        CreateNewLightningBolts();
    }

    void CreateNewLightningBolts() {
        // Delete old LightningBolts
        for (int i = 0; i < lightningBoltsInside.Count; i++) {
            Destroy(lightningBoltsInside[i]);
        }
        lightningBoltsInside.Clear();
        for (int i = 0; i < lightningBoltsOutside.Count; i++) {
            Destroy(lightningBoltsOutside[i]);
        }
        lightningBoltsOutside.Clear();

        // Create new LightningBolts inside
        GameObject inside = new GameObject("Bolts Inside");
        inside.transform.parent = transform;
        inside.transform.localPosition = new Vector3(0,0,0);
        for (int i = 0; i < numBoltsInside; i++) {
            GameObject lightningBolt = (GameObject) Instantiate(lightningBoltPrefab);
            lightningBolt.transform.parent = inside.transform;
            lightningBolt.transform.localPosition = new Vector3(0,0,0);

            lightningBoltsInside.Add(lightningBolt);
        }

        // Create new LightningBolts outside
        GameObject outside = new GameObject("Bolts Outside");
        outside.transform.parent = transform;
        outside.transform.localPosition = new Vector3(0,0,0);
        for (int i = 0; i < numBoltsOutside; i++) {
            GameObject lightningBolt = (GameObject) Instantiate(lightningBoltPrefab);
            lightningBolt.transform.parent = outside.transform;
            lightningBolt.transform.localPosition = new Vector3(0,0,0);
            
            lightningBoltsOutside.Add(lightningBolt);
        }

        UpdateLightningBolts();
    }

    void UpdateLightningBolts() {
        float randomRadius1, randomRadius2;
        float randomTheta1, randomTheta2, theta1, theta2;
        float x1, y1, x2, y2;
        Vector3 startPosition, endPosition;

        // Inside: Create lightning bolts to and from random points within the circle
        for (int i = 0; i < lightningBoltsInside.Count; i++) {
            // Calculate start position (random spot within circle)
            randomTheta1 = Random.value * 2.0f * Mathf.PI;
            randomRadius1 = Random.value * radius;
            x1 = randomRadius1 * Mathf.Cos(randomTheta1);
            y1 = randomRadius1 * Mathf.Sin(randomTheta1);
            startPosition = new Vector3(x1, y1, 0);
            
            // Calculate end position (random spot within circle)
            randomTheta2 = Random.value * 2.0f * Mathf.PI;
            randomRadius2 = radius;
            x2 = randomRadius2 * Mathf.Cos(randomTheta2);
            y2 = randomRadius2 * Mathf.Sin(randomTheta2);
            endPosition = new Vector3(x2, y2, 0);

            // Set positions in LightningBolt script
            GameObject lightningBolt = lightningBoltsInside[i];
            LightningBolt boltScript = (LightningBolt) lightningBolt.GetComponent<LightningBolt>();
            boltScript.startPosition = startPosition;
            boltScript.endPosition = endPosition;
        }

        // Outside: create lightning bolts around the edge of the circle
        float randomThetaOffset = Random.value * 2 * Mathf.PI;
        for (int i = 0; i < lightningBoltsOutside.Count; i++) {
            // Calculate start position (random spot on edge of circle)
            theta1 = 2.0f * Mathf.PI / numBoltsOutside * i + randomThetaOffset;
            x1 = radius * Mathf.Cos(theta1);
            y1 = radius * Mathf.Sin(theta1);
            startPosition = new Vector3(x1, y1, 0);

            // Calculate end position (random spot on edge of circle)
            theta2 = 2.0f * Mathf.PI / numBoltsOutside * (i + 1) + randomThetaOffset;
            x2 = radius * Mathf.Cos(theta2);
            y2 = radius * Mathf.Sin(theta2);
            endPosition = new Vector3(x2, y2, 0);

            // Set positions in LightningBolt script
            GameObject lightningBolt = lightningBoltsOutside[i];
            LightningBolt boltScript = (LightningBolt) lightningBolt.GetComponent<LightningBolt>();
            boltScript.startPosition = startPosition;
            boltScript.endPosition = endPosition;
        }
    }

    void Update() {
        if (Time.time - lastUpdateTime > updateRate) {
            UpdateLightningBolts();
            lastUpdateTime = Time.time;
        }
    }
}

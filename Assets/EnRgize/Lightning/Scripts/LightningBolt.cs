using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightningBolt : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Vector3 startPosition;
    public Vector3 endPosition;

    public int numSegments = 10;
    public float maxStepPercent = 0.05f;
    public float maxOffsetPercent = 0.10f;
    public Color tintColor;
    public Color branchTintColor;

    public int maxBranches = 1;
    public GameObject branchPrefab;
    public float minBranchStepPercent = 0.4f;
    public float maxBranchStepPercent = 0.9f;
    public float maxBranchOffsetPercent = 0.25f;
    private int numBranchesActive = 0;
    private float branchProbability;
    private List<GameObject> branches;

    public float updateRate = 1.0f / 60.0f; // seconds between updates
    private float lastUpdateTime;
    
    void Awake() {
        branches = new List<GameObject>();
    }

    void Start() {
        lastUpdateTime = Time.time;
        branchProbability = 1.0f * maxBranches / numSegments;

        for (int i = 0; i < maxBranches; i++) {
            GameObject branchBolt = (GameObject) Instantiate(branchPrefab);
            branchBolt.transform.parent = transform;
            branchBolt.transform.localPosition = new Vector3(0,0,0);
            branchBolt.transform.localScale = new Vector3(1,1,1);
            branchBolt.transform.localEulerAngles = new Vector3(0,0,0);
            branches.Add(branchBolt);
        }

        lineRenderer.SetColors(tintColor, tintColor);

        CreateBolt();
    }

    public void CreateBolt() {
        // Deactivate old branches
        for (int i = 0; i < branches.Count; i++) {
            branches[i].SetActive(false);
        }
        numBranchesActive = 0;

        // Compute information about this bolt
        Vector3 difference = endPosition - startPosition;
        Vector3 normalToGround = new Vector3(0, 0, 1);
        Vector3 normal = Vector3.Cross(difference, normalToGround).normalized;
        float totalDistance = Vector3.Distance(startPosition, endPosition);

        lineRenderer.SetVertexCount(numSegments);

        // Declare variables used within the loop
        float previousOffset = 0;
        float stepPercent, randomStepPercent;
        Vector3 stepPosition;
        float randomOffsetScale, randomOffsetDistance, randomOffset;
        Vector3 nextPosition;

        lineRenderer.SetPosition(0, startPosition);

        for (int i = 1; i < numSegments - 1; i++) {
            // Step along the bolt
            stepPercent = 1.0f / (numSegments + 1) * (i + 1);
            randomStepPercent = Random.Range(stepPercent * (1 - maxStepPercent),
                                             stepPercent * (1 + maxStepPercent));
            randomStepPercent = Mathf.Clamp(randomStepPercent, 0.0f, 1.0f);
            stepPosition = startPosition + difference * randomStepPercent;

            // Offset by a factor of the normal
            randomOffsetScale = Random.Range(-maxOffsetPercent, maxOffsetPercent);
            randomOffsetDistance = randomOffsetScale * totalDistance;
            randomOffset = previousOffset + randomOffsetDistance;
            nextPosition = stepPosition + normal * randomOffset;

            // Add this vertex and continue
            lineRenderer.SetPosition(i, nextPosition);
            previousOffset = randomOffset;

            // Branch using some probability
            if ((numBranchesActive < maxBranches) && (Random.value < branchProbability)) {
                CreateBranch(nextPosition, randomStepPercent, difference, normal);
            }
        }

        lineRenderer.SetPosition(numSegments - 1, endPosition);

        // Update the particle system
        ParticleSystem particles = gameObject.GetComponentInChildren<ParticleSystem>();
        if (particles) {
            particles.gameObject.transform.localScale = new Vector3(totalDistance, 1, 1);
        }
    }

    public void CreateBranch(Vector3 branchStart, float branchStepPercent, Vector3 difference, Vector3 normal) {
        GameObject branchBolt = branches[numBranchesActive];
        branchBolt.SetActive(true);
        numBranchesActive++;

        LightningBolt branchBoltScript = (LightningBolt) branchBolt.GetComponent<LightningBolt>();
        branchBoltScript.tintColor = branchTintColor;
        branchBoltScript.startPosition = branchStart;

        // Step along the remainder of the bolt
        float branchEndStepPercent = Random.Range(minBranchStepPercent, maxBranchStepPercent);
        Vector3 branchEndStepPosition = branchStart + difference * (1 - branchStepPercent) * branchEndStepPercent;

        // Offset the end by a factor of the normal
        float totalDistance = Vector3.Distance(startPosition, endPosition);
        float randomOffsetScale = Random.Range(-maxBranchOffsetPercent, maxBranchOffsetPercent);
        float randomOffsetDistance = randomOffsetScale * totalDistance;
        branchBoltScript.endPosition = branchEndStepPosition + normal * randomOffsetDistance;

        branchBoltScript.CreateBolt();
    }

    void Update() {
        if (Time.time - lastUpdateTime > updateRate) {
            CreateBolt();
            lastUpdateTime = Time.time;
        }
    }
}

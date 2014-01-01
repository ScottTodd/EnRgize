using UnityEngine;
using System.Collections;

public class LightningBolt : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public int numSegments = 10;
    public float maxStepPercent = 0.05f;
    public float maxOffsetPercent = 0.10f;
    public Color tintColor;

    public float updateRate = 1.0f / 60.0f; // seconds between updates
    private float lastUpdateTime;

    void Start() {
        lastUpdateTime = Time.time;

        lineRenderer.material.SetColor("_Color", tintColor);

        CreateBolt();
    }

    public void CreateBolt() {
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
        }

        lineRenderer.SetPosition(numSegments - 1, endPosition);
    }

    void Update() {
        if (Time.time - lastUpdateTime > updateRate) {
            CreateBolt();
            lastUpdateTime = Time.time;
        }
    }
}

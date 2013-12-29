using UnityEngine;
using System.Collections;

public class LightningBolt : MonoBehaviour
{
    public Vector3 startVertex;
    public Vector3 endVertex;
    public int numSegments = 10;
    public float maxStepPercent = 0.05f;
    public float maxOffsetPercent = 0.10f;

    public LineRenderer lineRenderer;

    void Start() {
        CreateBolt();
    }

    void CreateBolt() {
        // Compute information about this bolt
        Vector3 difference = endVertex - startVertex;
        Vector3 normalToGround = new Vector3(0, 0, 1);
        Vector3 normal = Vector3.Cross(difference, normalToGround).normalized;
        float totalDistance = Vector3.Distance(startVertex, endVertex);

        lineRenderer.SetVertexCount(numSegments);

        // Declare variables used within the loop
        float previousOffset = 0;
        float stepPercent, randomStepPercent;
        Vector3 stepVertex;
        float randomOffsetScale, randomOffsetDistance, randomOffset;
        Vector3 nextVertex;

        lineRenderer.SetPosition(0, startVertex);

        for (int i = 1; i < numSegments - 1; i++) {
            // Step along the bolt
            stepPercent = 1.0f / (numSegments + 1) * (i + 1);
            randomStepPercent = Random.Range(stepPercent * (1 - maxStepPercent),
                                             stepPercent * (1 + maxStepPercent));
            randomStepPercent = Mathf.Clamp(randomStepPercent, 0.0f, 1.0f);
            stepVertex = startVertex + difference * randomStepPercent;

            // Offset by a factor of the normal
            randomOffsetScale = Random.Range(-maxOffsetPercent, maxOffsetPercent);
            randomOffsetDistance = randomOffsetScale * totalDistance;
            randomOffset = previousOffset + randomOffsetDistance;
            nextVertex = stepVertex + normal * randomOffset;

            // Add this segment and continue
            lineRenderer.SetPosition(i, nextVertex);
            previousOffset = randomOffset;
        }

        lineRenderer.SetPosition(numSegments - 1, endVertex);
    }

    void Update() {
        CreateBolt();
    }
}

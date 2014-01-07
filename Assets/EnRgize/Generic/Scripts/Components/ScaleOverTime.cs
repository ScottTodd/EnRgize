using UnityEngine;
using System.Collections;

public class ScaleOverTime : MonoBehaviour
{
    public float scaleTime = 1.0f;
    public Vector3 targetScale;
    
    private float startTime;
    private Vector3 originalScale;
    
    void Start() {
        startTime = Time.time;
        originalScale = transform.localScale;
    }
    
    void Update() {
        float timeElapsed = Mathf.Clamp(Time.time - startTime, 0, 1);
        transform.localScale = Vector3.Lerp(originalScale, targetScale, timeElapsed);
    }
}

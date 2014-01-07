using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifeTime = 1.0f;
    private float startTime;
    
    void Start() {
        startTime = Time.time;
    }

    void Update() {
        float timePassed = Time.time - startTime;
        
        if (timePassed > lifeTime) {
            Destroy(gameObject);
        }
    }
}

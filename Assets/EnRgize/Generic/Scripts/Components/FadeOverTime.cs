using UnityEngine;
using System.Collections;

public class FadeOverTime : MonoBehaviour
{
    public float timeToFade = 1.0f; // seconds
    private float startTime;

    void Start() {
        startTime = Time.time;
    }

    void Update() {
        float timePassed = Time.time - startTime;

        if (timePassed > timeToFade) {
            Destroy(gameObject);
        }

        float alpha = 1 - timePassed / timeToFade;

        Color colorCopy = renderer.material.color;
        colorCopy.a = alpha;
        renderer.material.SetColor("_Color", colorCopy);
    }
}

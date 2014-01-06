using UnityEngine;
using System.Collections;

public class SetRandomScale : MonoBehaviour
{
    public Vector3 minScale;
    public Vector3 maxScale;

    void Start() {
        float percent = Random.value;
        Vector3 scale = Vector3.Lerp(minScale, maxScale, percent);
        transform.localScale = scale;
    }

    void Update() {

    }
}

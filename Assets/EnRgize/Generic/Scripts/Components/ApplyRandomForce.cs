using UnityEngine;
using System.Collections;

public class ApplyRandomForce : MonoBehaviour
{
    public float minHorizontalForce = 10.0f;
    public float maxHorizontalForce = 100.0f;
    public float minVerticalForce   = 10.0f;
    public float maxVerticalForce   = 100.0f;

    void Start() {
        Rigidbody2D rigidBody = (Rigidbody2D) transform.GetComponent<Rigidbody2D>();

        float horizontalForce = Random.Range(minHorizontalForce, maxHorizontalForce);
        float horizontalSign = (Random.value > 0.5) ? -1 : 1;
        horizontalForce *= horizontalSign;

        float verticalForce = Random.Range(minVerticalForce,   maxVerticalForce);
        float verticalSign = (Random.value > 0.5) ? -1 : 1;
        verticalForce *= verticalSign;

        rigidBody.AddForce(new Vector2(horizontalForce, verticalForce));
    }

    void Update() {

    }
}

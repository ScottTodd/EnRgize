using UnityEngine;
using System.Collections;

public class RotateOverTime : MonoBehaviour
{
    public float rotateSpeed = 90; // degrees per second
    public bool randomDirection = true;
    public bool startAtRandomAngle = true;
    public Vector3 rotationAxis = Vector3.forward;

    private int sign;

    void Start() {
        if (startAtRandomAngle) {
            transform.Rotate(rotationAxis * Random.Range(0, 360));
        }

        if (randomDirection) {
            sign = (Random.value > 0.5) ? -1 : 1;
        } else {
            sign = 1;
        }
    }

    void Update() {
        transform.Rotate(rotationAxis * Time.deltaTime * rotateSpeed * sign);
    }
}

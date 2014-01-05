using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    private Rigidbody2D ballRigidBody;

    void Start() {
        ballRigidBody = (Rigidbody2D) transform.GetComponent<Rigidbody2D>();
        ballRigidBody.AddForce(new Vector2(200, 200));
    }

    void Update() {

    }
}

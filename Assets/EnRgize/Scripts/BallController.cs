using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    private Rigidbody2D ballRigidBody;

    void Start() {
        //ballRigidBody = (Rigidbody2D) transform.GetComponent<Rigidbody2D>();
        //ballRigidBody.AddForce(new Vector2(200, 200));
    }

    void Update() {
        if (Input.touchCount > 0) {
            Vector2 touchPosition = Input.GetTouch(0).position;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 0));
            transform.localPosition = new Vector3(worldPosition.x, worldPosition.y, 0);
        }

        /*
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
            transform.localPosition = new Vector3(worldPosition.x, worldPosition.y, 0);
        }
        */
    }
}

using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    public bool charged = false;

    private LightningBall lightningBallScript;
    private GameObject lightningBallObject;

    void Start() {
        lightningBallScript = transform.GetComponentInChildren<LightningBall>();
        lightningBallObject = lightningBallScript.gameObject;

        if (!charged) {
            lightningBallObject.SetActive(false);
        }
    }

    void Update() {
        if (charged) {
            lightningBallObject.SetActive(true);
        } else {
            lightningBallObject.SetActive(false);
        }

    }
    
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "GroundedWall") {
            charged = false;
            lightningBallObject.SetActive(false);
        }
        if (collision.gameObject.tag == "ChargedWall") {
            charged = true;
            lightningBallObject.SetActive(true);
        }
    }
}

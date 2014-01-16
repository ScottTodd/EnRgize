using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    public bool charged = false;
    public AudioClip chargeSound;
    public AudioClip dischargeSound;

    private LightningBall lightningBallScript;
    private GameObject lightningBallObject;
    private AudioSource audioSource;

    void Start() {
        lightningBallScript = transform.GetComponentInChildren<LightningBall>();
        lightningBallObject = lightningBallScript.gameObject;

        if (!charged) {
            lightningBallObject.SetActive(false);
        }

        audioSource = transform.GetComponent<AudioSource>();
    }

    void Update() {
        if (charged) {
            lightningBallObject.SetActive(true);
        } else {
            lightningBallObject.SetActive(false);
        }

    }
    
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "ChargedWall") {
            charged = true;
            lightningBallObject.SetActive(true);

            audioSource.clip = chargeSound;
            audioSource.Play();
        }

        if (collision.gameObject.tag == "GroundedWall") {
            charged = false;
            lightningBallObject.SetActive(false);
            
            audioSource.clip = dischargeSound;
            audioSource.Play();
        }
    }
}

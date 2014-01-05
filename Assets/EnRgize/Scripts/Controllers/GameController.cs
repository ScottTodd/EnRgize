using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    void Start() {
    
    }

    void Update() {
        // On Android, the back button is KeyCode.Escape
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}

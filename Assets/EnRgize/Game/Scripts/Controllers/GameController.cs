using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public int buttonWidth = 48;
    public int buttonHeight = 48;
    public Texture audioEnabledButton;
    public Texture audioDisabledButton;

    private bool audioEnabled = true;
    private Rect buttonPosition;

    void Start() {
        buttonPosition = new Rect(0, Screen.height - buttonHeight, buttonWidth, buttonHeight);
    }

    void Update() {
        // On Android, the back button is KeyCode.Escape
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    public bool ScreenPointWithinGUI(Vector2 point) {
        Vector2 guiPoint = new Vector2(point.x, Screen.height - point.y);
        return buttonPosition.Contains(guiPoint);
    }

    void OnGUI() {
        if (audioEnabled) {
            if (GUI.Button(buttonPosition, audioEnabledButton)) {
                audioEnabled = false;
                AudioListener.volume = 0;
            }
        } else {
            if (GUI.Button(buttonPosition, audioDisabledButton)) {
                audioEnabled = true;
                AudioListener.volume = 1;
            }
        }
    }
}

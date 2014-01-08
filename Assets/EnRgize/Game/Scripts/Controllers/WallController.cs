using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour
{
    void Start() {
        // Keep MeshRenderer enabled in editor view but disable it in game
        MeshRenderer meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    void Update() {

    }
}

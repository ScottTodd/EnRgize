using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour
{
    public GameObject backgroundCirclePrefab;
    public int numBackgroundCircles = 20;
    public Vector3 spawnArea;

    void Start() {
        for (int i = 0; i < numBackgroundCircles; i++) {
            GameObject backgroundCircle = (GameObject) Instantiate(backgroundCirclePrefab);
            backgroundCircle.transform.parent = transform;

            float spawnX = Random.Range(-spawnArea.x / 2.0f, spawnArea.x / 2.0f);
            float spawnY = Random.Range(-spawnArea.y / 2.0f, spawnArea.y / 2.0f);
            float spawnZ = Random.Range(-spawnArea.z / 2.0f, spawnArea.z / 2.0f);
            backgroundCircle.transform.localPosition = new Vector3(spawnX, spawnY, spawnZ);
        }
    }

    void Update() {

    }
}

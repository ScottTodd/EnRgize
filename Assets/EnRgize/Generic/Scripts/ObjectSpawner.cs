using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public int numberToSpawn = 5;
    public Vector3 spawnArea = new Vector3(1,1,1);
    
    void Start() {
        for (int i = 0; i < numberToSpawn; i++) {
            GameObject spawnedObject = (GameObject) Instantiate(objectPrefab);
            spawnedObject.transform.parent = transform;
            
            float spawnX = Random.Range(-spawnArea.x / 2.0f, spawnArea.x / 2.0f);
            float spawnY = Random.Range(-spawnArea.y / 2.0f, spawnArea.y / 2.0f);
            float spawnZ = Random.Range(-spawnArea.z / 2.0f, spawnArea.z / 2.0f);
            spawnedObject.transform.localPosition = new Vector3(spawnX, spawnY, spawnZ);
        }
    }
    
    void Update() {
        
    }
}

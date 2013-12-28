using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

	void Start () {
	}

	void Update () {
		// rotate at 90 degrees per second
		transform.Rotate(Vector3.up * Time.deltaTime*90);
	}
}
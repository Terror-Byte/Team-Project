using UnityEngine;
using System.Collections;


public class BulletScript : MonoBehaviour {
	int countdownToDestruction = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		countdownToDestruction++;
		if (countdownToDestruction == 50) {
			GameObject bullet = GameObject.Find("BulletPrefab");
			Destroy (gameObject);

		}
	}
}

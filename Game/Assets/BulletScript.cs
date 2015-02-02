using UnityEngine;
using System.Collections;


public class BulletScript : MonoBehaviour {
	// int countdownToDestruction = 0;

	Vector2 startPos;
	Vector2 currentPos;
	public float range = 10.0f;

	// Use this for initialization
	void Start () {
		startPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		/* Old time-based destruction
		countdownToDestruction++;
		if (countdownToDestruction == 50) {
			Destroy (gameObject);
		}
		*/

		/* New distance-based destruction */
		currentPos = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);

		float currentDist = Vector2.Distance (currentPos, startPos);
		Debug.Log ("Start Pos: " + startPos.ToString () + " Current Pos: " + currentPos.ToString ());

		if (currentDist > range) {
			Debug.Log (currentDist);
			Destroy(gameObject);
		}
		/*
		if (Norm () >= range) {
			Destroy(gameObject);
		}
		*/
	}

	float Norm() {
		// float xComponent = Mathf.Pow (startPos.x - currentPos.x, 2);
		// float yComponent = Mathf.Pow (startPos.y - currentPos.y, 2);

		float xComponent = Mathf.Pow (currentPos.x - startPos.x, 2);
		float yComponent = Mathf.Pow (currentPos.y - startPos.y, 2);
		return Mathf.Sqrt (xComponent + yComponent);
	}

}

using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public GameObject bulletPrefab;
	public float speed = 1;
	Vector2 dir = new Vector2(0,0);

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey (KeyCode.W)) 
		{
			dir.y = speed;
		} 
		else if (Input.GetKey (KeyCode.S)) 
		{
			dir.y = -speed;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			dir.x = -speed;
		}
		else if (Input.GetKey (KeyCode.D))
		{
			dir.x = speed;
		}
		else
		{
			dir = Vector2.zero;
		}

		transform.Translate (dir); 

		// If left button pressed, generate a new bullet and fire.
		if (Input.GetMouseButtonDown(0))
		{
			//GameObject worldScript = GameObject.Find ("WorldControllerObj");
			//WorldController worldController = worldScript.GetComponent<WorldController>();
			//GameObject bulletObj = GameObject.Find ("BulletPrefab");
			//BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();	

			GameObject player = GameObject.Find ("PlayerPrefab");
			float xPos = player.transform.position.x;
			float yPos = player.transform.position.y;

			GameObject newBullet = (GameObject)Instantiate(bulletPrefab, new Vector3(xPos, yPos, 1), Quaternion.identity);
			newBullet.AddComponent("Rigidbody2D");
			newBullet.rigidbody2D.gravityScale = 0f;

			// http://answers.unity3d.com/questions/736511/shoot-towards-mouse-in-unity2d.html

			//Vector2 mousePos = Camera.main.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
			Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			Vector2 forceDirection = mousePos - new Vector2(player.transform.position.x, player.transform.position.y);
			Debug.Log (forceDirection.ToString ());
			forceDirection.Normalize();
			Debug.Log (forceDirection.ToString ());
			newBullet.rigidbody2D.velocity = forceDirection * 5;
		}
	}
}

using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public GameObject bulletPrefab;
	public float speed = 1;
	Vector2 dir = new Vector2(0,0);

	public float weaponSpd = 2.5f;

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
		if (Input.GetMouseButton(0))
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

			//Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Vector3 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
			//Vector2 forceDirection = new Vector2(mousePos.x, mousePos.y) - new Vector2(playerPos.x, playerPos.y);
			//forceDirection.Normalize();

			Vector3 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
			Vector3 mousePos = Input.mousePosition;
			Vector3 forceDirection = mousePos - playerPos;
			forceDirection.Normalize ();

			newBullet.rigidbody2D.velocity = forceDirection * weaponSpd;
		}
	}
}

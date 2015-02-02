using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public GameObject bulletPrefab;
	public float speed = 1;
	Vector2 dir = new Vector2(0,0);

	public float weaponSpd = 2.5f;
	public int weaponRefresh = 25;
	int refreshCounter;

	// Use this for initialization
	void Start () 
	{
		refreshCounter = 0;
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
			if (refreshCounter == 0)
			{
				GameObject player = GameObject.Find ("PlayerPrefab");
				float xPos = player.transform.position.x;
				float yPos = player.transform.position.y;
				
				GameObject newBullet = (GameObject)Instantiate(bulletPrefab, new Vector3(xPos, yPos, 1), Quaternion.identity);
				newBullet.AddComponent("Rigidbody2D");
				newBullet.rigidbody2D.gravityScale = 0f;
				
				Vector3 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
				Vector3 mousePos = Input.mousePosition;
				Vector3 forceDirection = mousePos - playerPos;
				forceDirection.Normalize ();
				
				newBullet.rigidbody2D.velocity = forceDirection * weaponSpd;
				refreshCounter++;
			}
			else if (refreshCounter == weaponRefresh)
			{
				refreshCounter = 0;
			}
			else
			{
				refreshCounter++;
			}
		}
	}
}

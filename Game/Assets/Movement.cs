using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

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


	}
}

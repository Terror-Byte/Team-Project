﻿using UnityEngine;
using System.Collections;


public class BulletScript : MonoBehaviour {
	// int countdownToDestruction = 0;

    public string source = "";
    int wepDmg = 5;
	Vector2 startPos;
	Vector2 currentPos;
	public float range = 10.0f;
	Vector2 moveVector = new Vector2();
    GameController game;
	// Use this for initialization
	void Start () {
        game = GameObject.Find("GameController").GetComponent<GameController>();
		startPos = gameObject.transform.position;
        Physics2D.IgnoreCollision(collider2D, collider2D);

	}
	
	// Update is called once per frame
	void Update () {
		/* Old time-based destruction
		countdownToDestruction++;
		if (countdownToDestruction == 50) {
			Destroy (gameObject);
		}
		if (Norm () >= range) {
			Destroy(gameObject);
		}
		*/


		/* New distance-based destruction */
		currentPos = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);

		float currentDist = Vector2.Distance (currentPos, startPos);

		if (currentDist > range) {
			Destroy(gameObject);
		}

		// Movement code
		transform.Translate (moveVector);

	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            //Debug.Log("hit!");
            if (source != "Enemy")
                coll.gameObject.SendMessage("ApplyDamage", wepDmg);

            Destroy(this.gameObject);
        }
		else if (coll.gameObject.tag == "Player")
		{
			//Debug.Log("Player Hit!");
			Destroy(this.gameObject);
			coll.gameObject.SendMessage("ApplyDamage", wepDmg);
		}
	}

	void setDamage(int dmg)
    {
        wepDmg = dmg;
    }
}

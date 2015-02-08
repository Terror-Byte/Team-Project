using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int health = 500;
	public float speed = 1;
	
	enum state { Roam, Attack };
	state aiState = state.Roam;
	public bool hasTarget = false;
	public Vector2 target = new Vector2();

	public Vector2 playerPosVec2 = new Vector2();
	Vector2 mousePos = new Vector2(); // For testing only

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
		mousePos = Input.mousePosition; // For testing only

		playerPosVec2.x = gameObject.transform.position.x;
		playerPosVec2.y = gameObject.transform.position.y;

        if (health <= 0)
            Destroy(this.gameObject);

		if (aiState == state.Roam)
		{
			if (!hasTarget)
			{
				target = new Vector2(Random.Range(-5, 5), Random.Range (-5, 5));
				hasTarget = true;
			}
			else if (hasTarget)
			{
				Debug.DrawLine(new Vector3(playerPosVec2.x, playerPosVec2.y, 1), new Vector3(target.x, target.y, 1), Color.red);
				if (playerPosVec2.x <= (target.x + 0.1) && playerPosVec2.x >= (target.x - 0.1) && playerPosVec2.y <= (target.y + 0.1) && playerPosVec2.y >= (target.y - 0.1))
				{
					hasTarget = false;
				}

				Vector2 vec2Target = target - playerPosVec2;
				vec2Target.Normalize();
				vec2Target.x = vec2Target.x * speed * Time.deltaTime;
				vec2Target.y = vec2Target.y * speed * Time.deltaTime;
				transform.Translate(vec2Target);
			}
		}
		else if (aiState == state.Attack)
		{

		}
	}
    
    void ApplyDamage(int x)
    {
        health -= x;
    }
}

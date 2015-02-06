using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int health = 500;
	public float speed = 1;
	
	enum state { Roam, Attack };
	state aiState = state.Roam;
	bool hasTarget = false;
	// bool atTarget = false;
	Vector2 target = new Vector2();

	Vector2 playerPosVec2 = new Vector2();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
		playerPosVec2.x = gameObject.transform.position.x;
		playerPosVec2.y = gameObject.transform.position.y;

        if (health <= 0)
            Destroy(this.gameObject);

		if (aiState == state.Roam)
		{
			if (!hasTarget)
			{
				target = new Vector2(Random.Range(5, 10), Random.Range (5, 10));
				hasTarget = true;
			}
			else if (hasTarget)
			{
				if (playerPosVec2.x <= target.x + 2 || playerPosVec2.x >= target.x - 2 || playerPosVec2.y <= target.y + 2 || playerPosVec2.y >= target.y - 2)
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

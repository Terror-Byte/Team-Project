using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {

    public Vector2 levelVector = new Vector2(0,0);
    //public int maxDifficultyCompleted = 0;
    //public int difficultyLevel = 0;
    public float flashTime = 0.5f;
    float t = 1.0f;

	GameController game;

	// Use this for initialization
	void Start () 
    {
		game = GameObject.Find ("GameController").GetComponent<GameController>();

	}
	
	// Update is called once per frame
	void Update () 
    {
        t -= Time.deltaTime;

        if (t <= 0.0f)
        {
            this.gameObject.renderer.enabled = !this.gameObject.renderer.enabled;
           
            t = flashTime;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (game.maxDifficultyCompleted >= DifficultyLevel(levelVector + new Vector2(0, 1)))
            {
                transform.Translate(new Vector2(0,2));
                ++levelVector.y;
                this.gameObject.renderer.enabled = true; //Makes the sprite visible when moving
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (game.maxDifficultyCompleted >= DifficultyLevel(levelVector + new Vector2(0, -1)))
            {
                transform.Translate(new Vector2(0, -2));
                --levelVector.y;
                this.gameObject.renderer.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (game.maxDifficultyCompleted >= DifficultyLevel(levelVector + new Vector2(-1, 0)))
            {
                transform.Translate(new Vector2(-2, 0));
                --levelVector.x;
                this.gameObject.renderer.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (game.maxDifficultyCompleted >= DifficultyLevel(levelVector + new Vector2(1, 0)))
            {
                transform.Translate(new Vector2(2, 0));
                ++levelVector.x;
                this.gameObject.renderer.enabled = true;
            }
        }

        game.difficultyLevel = DifficultyLevel(levelVector);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            game.SendMessage("Load", 2);
        }
	}

    int DifficultyLevel(Vector2 level)
    {
        return (int)Mathf.Max(Mathf.Abs(level.x), Mathf.Abs(level.y));
    }
}

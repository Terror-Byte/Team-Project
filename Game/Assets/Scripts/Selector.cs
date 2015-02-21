using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {

    public Vector2 levelVector = new Vector2(0,0);
    public int maxDifficultyCompleted = 0;
    public int difficultyLevel = 0;
    public float flashTime = 0.5f;
    float t = 1.0f;

	GameObject gameController;

	// Use this for initialization
	void Start () 
    {
		gameController = GameObject.Find ("GameController");
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
            if (maxDifficultyCompleted >= DifficultyLevel(levelVector + new Vector2(0,1)))
            {
                transform.Translate(new Vector2(0,2));
                ++levelVector.y;
                this.gameObject.renderer.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (maxDifficultyCompleted >= DifficultyLevel(levelVector + new Vector2(0, -1)))
            {
                transform.Translate(new Vector2(0, -2));
                --levelVector.y;
                this.gameObject.renderer.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (maxDifficultyCompleted >= DifficultyLevel(levelVector + new Vector2(-1, 0)))
            {
                transform.Translate(new Vector2(-2, 0));
                --levelVector.x;
                this.gameObject.renderer.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (maxDifficultyCompleted >= DifficultyLevel(levelVector + new Vector2(1, 0)))
            {
                transform.Translate(new Vector2(2, 0));
                ++levelVector.x;
                this.gameObject.renderer.enabled = true;
            }
        }

        difficultyLevel = DifficultyLevel(levelVector);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.LoadLevel("ActionScene");
        }
	}

    int DifficultyLevel(Vector2 level)
    {
        return (int)Mathf.Max(Mathf.Abs(level.x), Mathf.Abs(level.y));
    }
}

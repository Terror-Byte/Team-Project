using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {

    public Vector2 levelVector = new Vector2(0,0);
    //public int maxDifficultyCompleted = 0;
    //public int difficultyLevel = 0;
    public float flashTime = 0.5f;
    float t = 1.0f;

	GameController game;
	LevelGenerator levelGen;

	string[,] levels = new string[7,7];

	// Use this for initialization
	void Start () 
    {
		game = GameObject.Find ("GameController").GetComponent<GameController>();

		//maxDifficultyCompleted = game.maxLevelCompleted;
		levelGen = game.GetComponent<LevelGenerator>();
		
		if (!levelGen.levelsInitialised)
		{
			levels = initialiseLevels();
			levelGen.levels = levels;
			levelGen.levelsInitialised = true;
		}
		else
		{
			levels = levelGen.levels;
		}
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
			if (levelVector.x == 0 && levelVector.y == 0)
			{
				//levelGen.LoadLevel(levelVector.x, levelVector.y);
				//Debug.Log ("Random level loaded!");
				game.SendMessage("Load", 2);
			}
			else
			{
				//game.SendMessage("Load", 2);
				Application.LoadLevel("ActionScene");
				//Debug.Log("Loading random level");
				levelGen.LoadLevel(levelVector.x, levelVector.y);
			}
			game.health = 100;
        }
	}

    int DifficultyLevel(Vector2 level)
    {
        return (int)Mathf.Max(Mathf.Abs(level.x), Mathf.Abs(level.y));
    }

	string[,] initialiseLevels()
	{
		string[,] levelsTemp = new string[7,7];
		
		for (int x = 0; x < 7; x++)
		{
			for (int y = 0; y < 7; y++)
			{
				Random.seed = System.DateTime.Now.Millisecond;
				int rand = Random.Range(1, 4);
				
				if (x != 0 && y != 0 || x != 0 && y != 6 || x != 6 && y != 0 || x != 6 && y != 6)
				{
					switch (rand)
					{
					case 1: levelsTemp[x, y] = "level1.txt";
						break;
					case 2: levelsTemp[x, y] = "level2.txt";
						break;
					case 3: levelsTemp[x, y] = "level3.txt";
						break;
					}
					//Debug.Log(levelsTemp[x, y]);
					//Debug.Log("X: " + x + " Y: " + y);
				}
			}
		}
		return levelsTemp;
	}
}

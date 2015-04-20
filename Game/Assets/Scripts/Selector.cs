using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {

    public GameObject islandPrefab;
    public GameObject home;
    //public int maxDifficultyCompleted = 0;
    //public int difficultyLevel = 0;
    public float flashTime = 0.5f;
    float t = 1.0f;
    public bool onIsland = false;
	GameController game;
	LevelGenerator levelGen;

	string[,] levels = new string[7,7];

	// Use this for initialization
	void Start () 
    {
		game = GameObject.Find ("GameController").GetComponent<GameController>();

        if (game.currentIsland != null)
            this.transform.position = game.currentIsland.transform.position;

        this.gameObject.renderer.enabled = true;

        if (game.islands.Length == 0)
            SpawnIslands();
        else
            RespawnIslands();
		//maxDifficultyCompleted = game.maxLevelCompleted;
        /*
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
         */
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

        if (Input.GetKey(KeyCode.W))
        {
            if (game.maxDifficultyCompleted >= DifficultyLevel(game.levelVector + new Vector2(0, 1)))
            {
                GameObject.Find("Boat").GetComponent<SpriteRenderer>().transform.rotation = Quaternion.AngleAxis(270, Vector3.forward);
                transform.Translate(new Vector2(0, 0.4f));
                ++game.levelVector.y;
                this.gameObject.renderer.enabled = true; //Makes the sprite visible when moving
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (game.maxDifficultyCompleted >= DifficultyLevel(game.levelVector + new Vector2(0, -1)))
            {
                GameObject.Find("Boat").GetComponent<SpriteRenderer>().transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                transform.Translate(new Vector2(0, -0.4f));
                --game.levelVector.y;
                this.gameObject.renderer.enabled = true;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (game.maxDifficultyCompleted >= DifficultyLevel(game.levelVector + new Vector2(-1, 0)))
            {
                GameObject.Find("Boat").GetComponent<SpriteRenderer>().transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                transform.Translate(new Vector2(-0.4f, 0));
                --game.levelVector.x;
                this.gameObject.renderer.enabled = true;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (game.maxDifficultyCompleted >= DifficultyLevel(game.levelVector + new Vector2(1, 0)))
            {
                GameObject.Find("Boat").GetComponent<SpriteRenderer>().transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                transform.Translate(new Vector2(0.4f, 0));
                ++game.levelVector.x;
                this.gameObject.renderer.enabled = true;
            }
        }

        game.difficultyLevel = DifficultyLevel(game.levelVector);

	}

    int DifficultyLevel(Vector2 level)
    {
        int tmp = (int)Mathf.Max(Mathf.Abs(level.x), Mathf.Abs(level.y));

        tmp = Mathf.RoundToInt(tmp / 50);

        if (tmp == 0)
            tmp = 1;

        return tmp;
    }

    //string[,] initialiseLevels()
    //{
    //    string[,] levelsTemp = new string[7,7];
		
    //    for (int x = 0; x < 7; x++)
    //    {
    //        for (int y = 0; y < 7; y++)
    //        {
    //            Random.seed = System.DateTime.Now.Millisecond;
    //            int rand = Random.Range(1, 4);
				
    //            if (x != 0 && y != 0 || x != 0 && y != 6 || x != 6 && y != 0 || x != 6 && y != 6)
    //            {
    //                switch (rand)
    //                {
    //                case 1: levelsTemp[x, y] = "level1.txt";
    //                    break;
    //                case 2: levelsTemp[x, y] = "level2.txt";
    //                    break;
    //                case 3: levelsTemp[x, y] = "level3.txt";
    //                    break;
    //                }
    //                //Debug.Log(levelsTemp[x, y]);
    //                //Debug.Log("X: " + x + " Y: " + y);
    //            }
    //        }
    //    }
    //    return levelsTemp;
    //}

    void SpawnIslands()
    {
        //creates the home island and other islands around
        GameObject homeTmp = (GameObject)Instantiate(home, new Vector2(0,0), Quaternion.identity);
        homeTmp.transform.SetParent(game.transform);
        for (float x = -670.5f; x < 670.5f; x += 30)
        {
            for (float y = -420.5f; y < 420.5f; y += 27)
            {
                float randX = Random.Range(3.0f, 23.0f);
                float randY = Random.Range(3.0f, 23.0f);

                if (new Vector2(x + randX, y + randY).magnitude > 8)
                {
                    GameObject tmp = (GameObject)Instantiate(islandPrefab, new Vector2(x + randX, y + randY), Quaternion.identity);
                    tmp.transform.SetParent(game.transform);
                }
            }
        }
    }

    void RespawnIslands()
    {
        foreach (GameObject i in game.islands)
        {
            //i.transform.position = i.GetComponent<Island>().place;
            i.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        onIsland = true;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        onIsland = false;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        game.currentIsland = coll.gameObject;
        onIsland = true;
        if (coll.gameObject.tag == "Island" && coll.gameObject.GetComponent<Island>().isPlayable || coll.name == "Home" && coll.gameObject.GetComponent<Island>().isPlayable)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject.Find("Sounds").audio.Play();

                if (game.levelVector.x < 3 && game.levelVector.y < 3 && game.levelVector.x > -3 && game.levelVector.y > -3)
                {
                    //levelGen.LoadLevel(levelVector.x, levelVector.y);
                    //Debug.Log ("Random level loaded!");
                    game.SendMessage("Load", 2);
                }
                else
                {
                    //game.SendMessage("Load", 2);
                    game.SendMessage("Load", 3);
                    //Debug.Log("Loading random level");
                    //levelGen.LoadLevel(levelVector.x, levelVector.y);
                }
                //game.health = 100;
                // levelGen.CreateMap();
                // levelGen.CreateGraph();
            }
        }
    }
}

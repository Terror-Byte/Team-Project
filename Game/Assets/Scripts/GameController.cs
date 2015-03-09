using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    //Player Stats
    //Will be contained over level changes
    public int health;
    public float strength;
    public float speed;
    public int xp;
    public int level;
    
    //Lvl stats
    public int maxDifficultyCompleted = 0;
    public int difficultyLevel = 0;

    Vector3 origin;
    public bool mouseEdge = false;
    int screenX;
    int screenY;
    float t = 0.0f;
    public int dist = 5;
    public int edge = 50;

	//public GameObject playerClass;
	public int playerXp = 0;

    public GameObject[] gos;
    public int totalEnemies;

	LevelGenerator levelGen;

	// Use this for initialization
	void Start () 
    {
        health = 100;
        strength = 5;
        speed = 5;
        xp = 0;
        level = 0;

        origin = Camera.main.transform.position;

        screenX = Screen.width;
        screenY = Screen.height;

        DontDestroyOnLoad(this.gameObject);

		levelGen = gameObject.GetComponent<LevelGenerator>();
	}
	
	// Update is called once per frame
	void Update() 
    {
        if (Application.loadedLevelName == "Scene1")
        {
			//if (player == null)
				//player = GameObject.Find ("Player");
            gos = GameObject.FindGameObjectsWithTag("Enemy");
            totalEnemies = gos.Length;
        }

		if (Application.loadedLevelName == "ActionScene")
		{
			//if (player == null)
			//player = GameObject.Find ("Player");
			gos = GameObject.FindGameObjectsWithTag("Enemy");
			totalEnemies = gos.Length;
			// Debug.Log ("Enemies: " + totalEnemies);
		}

        if (Application.loadedLevelName == "Select")
            origin = new Vector3(0, 0, -10);

        if (GameObject.Find("Player") != null)
        {
            origin = GameObject.Find("Player").transform.position;
            origin.z = -10.0f;
        }

        t = 2f;

        //move camera right
        if (Input.mousePosition.x > screenX - edge && Input.mousePosition.x < screenX + edge)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + Vector3.right * dist, t * Time.deltaTime);
        }
        else
        {
            Camera.main.transform.position = (Vector3.Lerp(Camera.main.transform.position, origin, t * Time.deltaTime));
        }

        //move camera left
        if (Input.mousePosition.x < 0 + edge && Input.mousePosition.x > 0 - edge)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + Vector3.left * dist, t * Time.deltaTime);
        }
        else
        {
            Camera.main.transform.position = (Vector3.Lerp(Camera.main.transform.position, origin, t * Time.deltaTime));
        }

        //move camera up
        if (Input.mousePosition.y > screenY - edge && Input.mousePosition.y < screenY + edge)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + Vector3.up * dist, t * Time.deltaTime);
        }
        else
        {
            Camera.main.transform.position = (Vector3.Lerp(Camera.main.transform.position, origin, t * Time.deltaTime));
        }

        //move camera down
        if (Input.mousePosition.y < 0 + edge && Input.mousePosition.y > 0 - edge)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + Vector3.down * dist, t * Time.deltaTime);
        }
        else
        {
            Camera.main.transform.position = (Vector3.Lerp(Camera.main.transform.position, origin, t * Time.deltaTime));
        }

        if (Application.loadedLevelName == ("Scene1") && totalEnemies == 0)
        {
            maxDifficultyCompleted = (int)Mathf.Max(difficultyLevel + 1, maxDifficultyCompleted);
            Application.LoadLevel("Select");
            //Selector maxDifficultyCompleted = leveldifficulty completed
        }

		if (Application.loadedLevelName == ("ActionScene") && totalEnemies == 0 && levelGen.mapCreated)
		{
			maxDifficultyCompleted = (int)Mathf.Max(difficultyLevel + 1, maxDifficultyCompleted);
			Application.LoadLevel("Select");
			levelGen.mapCreated = false;
		}
	}

    void EnemyDied()
    {
        totalEnemies--;
    }

	void Load(int level)
    {
        Application.LoadLevel(level);
    }
}
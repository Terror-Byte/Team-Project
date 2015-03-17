using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    //Player Stats
    //Will be contained over level changes
    public int health;
    public float strength;
    public float speed;
    public int xp;
    public int level;
    public int gold;

    //Lvl stats
    public int maxDifficultyCompleted = 0;
    public int difficultyLevel = 0;

    //public Vector3 origin;

    float t = 0.0f;
    public int dist = 5;
    public int edge = 100;

    //public GameObject playerClass;
    public int playerXp = 0;

    public GameObject[] gos;
    public int totalEnemies;

    LevelGenerator levelGen;

    // Use this for initialization
    void Start()
    {
        health = 100;
        strength = 5;
        speed = 5;
        xp = 0;
        level = 1;
        gold = 0;

        //origin = Camera.main.transform.position;

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

        //if (GameObject.FindGameObjectWithTag("Player") != null)
        //{
        //    origin = GameObject.FindGameObjectWithTag("Player").transform.position;
        //    origin.z = -15.0f;
       // }



        //if (Application.loadedLevelName == ("Scene1") && totalEnemies == 0)
        //{
        //    maxDifficultyCompleted = (int)Mathf.Max(difficultyLevel + 1, maxDifficultyCompleted);
        //    Application.LoadLevel("Select");
        //    //Selector maxDifficultyCompleted = leveldifficulty completed
        //}

        //if (Application.loadedLevelName == ("ActionScene") && totalEnemies == 0 /*&& levelGen.mapCreated*/)
        //{
        //    maxDifficultyCompleted = (int)Mathf.Max(difficultyLevel + 1, maxDifficultyCompleted);
        //    Application.LoadLevel("Select");
        //    //levelGen.mapCreated = false;
        //}
    }

    void EnemyDied()
    {
        totalEnemies--;
    }

    void Load(int level)
    {
        Application.LoadLevel(level);
    }

    void Finish()
    {
        maxDifficultyCompleted = (int)Mathf.Max(difficultyLevel + 1, maxDifficultyCompleted);
        Application.LoadLevel("Select");
    }
}
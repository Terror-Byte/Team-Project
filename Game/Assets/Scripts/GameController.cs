using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    Vector3 origin;
    public bool mouseEdge = false;
    int screenX;
    int screenY;
    float t = 0.0f;
    public int dist = 5;
    public int edge = 50;

	public GameObject player;
	public int playerXp = 0;

    GameObject[] gos;
    public int totalEnemies;

    // TileEngine objects
    int mapSizeX = 27;
    int mapSizeY = 20;
    TileType[] tileTypes;
    int[,] tiles;
    bool mapCreated = false;
    public GameObject grassPrefab;
    public GameObject sandPrefab;
    public GameObject waterPrefab;

	// Use this for initialization
	void Start () 
    {
        origin = Camera.main.transform.position;

        screenX = Screen.width;
        screenY = Screen.height;

        DontDestroyOnLoad(this.gameObject);

        initialiseMap();
	}
	
	// Update is called once per frame
	void Update () 
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
            if (!mapCreated)
            {
                for (int x = 0; x < mapSizeX; x++)
                {
                    for (int y = 0; y < mapSizeY; y++)
                    {
                        Instantiate(tileTypes[tiles[x, y]].tileVisual, new Vector3(x * 2, y * 2, 0), Quaternion.identity);
                    }
                }
                mapCreated = true;
            }
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
            Application.LoadLevel("Select");
            // mapCreated = false;
            //Selector maxDifficultyCompleted = leveldifficulty completed
        }

	}

    void EnemyDied()
    {
        totalEnemies--;
    }

	void SendXPToPlayer()
	{
		player.SendMessage ("LoadXPFromController", playerXp);
	}

    void initialiseMap()
    {
        tileTypes = new TileType[3];
        tileTypes[0] = new TileType("Grass", grassPrefab, true);
        tileTypes[1] = new TileType("Sand", sandPrefab, true);
        tileTypes[2] = new TileType("Water", waterPrefab, false);
        tiles = new int[mapSizeX, mapSizeY];
        tiles = TestMaps.LoadLevel("level1.txt", mapSizeX, mapSizeY);
    }
}
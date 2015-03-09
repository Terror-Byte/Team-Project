using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

    int mapSizeX = 31;
    int mapSizeY = 22;
    TileType[] tileTypes;
    int[,] tiles;
    public Node[,] navGraph;
    public bool mapCreated = false;
    public GameObject grassPrefab;
    public GameObject sandPrefab;
    public GameObject waterPrefab;
    public GameObject enemyPrefab;

    public string[,] levels;
    public bool levelsInitialised = false;

    GameController controller;

	// Use this for initialization
	void Start () {
        initialiseMap();
        controller = gameObject.GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		// Create seperate function of this.
        if (Application.loadedLevelName == "ActionScene")
        {
            if (!mapCreated)
            {
                /*
                for (int x = 0; x < mapSizeX; x++)
                {
                    for (int y = 0; y < mapSizeY; y++)
                    {
                        Instantiate(tileTypes[tiles[x, y]].tileVisual, new Vector3(x * 2, y * 2, 0), Quaternion.identity);
                    }
                }

                int enemyNo = Random.Range(3, 6);
                controller.totalEnemies = enemyNo;

                for (int i = 0; i < enemyNo; i++)
                {
                    Vector3 position = new Vector3(Random.Range((mapSizeX / 2), ((3 * mapSizeX) / 2)), Random.Range((mapSizeY / 2), ((3 * mapSizeY) / 2)), 10);
                    Instantiate(enemyPrefab, position, Quaternion.identity);
                }
                */
                
                CreateMap();
                CreateGraph();

                //for (int x = 0; x < (mapSizeX * 2) + 1; x++)
                //{
                //    for (int y = 0; y < (mapSizeY * 2) + 1; y++)
                //    {
                //        Debug.Log(navGraph[x, y].worldPos.ToString());
                //    }
                //}

                mapCreated = true;
            }
        }
        if (navGraph != null)
        {
            for (int x = 0; x < (mapSizeX * 2); x++)
            {
                for (int y = 0; y < (mapSizeY * 2); y++)
                {
                    Debug.DrawLine(navGraph[x, y].worldPos, navGraph[x, y + 1].worldPos, Color.red);
					Debug.DrawLine(navGraph[x, y].worldPos, navGraph[x + 1, y].worldPos, Color.red);
                    //Debug.Log(navGraph[x, y].worldPos);
                }
            }
        }
	}

    void initialiseMap()
    {
        tileTypes = new TileType[3];
        tileTypes[0] = new TileType("Grass", grassPrefab, true);
        tileTypes[1] = new TileType("Sand", sandPrefab, true);
        tileTypes[2] = new TileType("Water", waterPrefab, false);
        tiles = new int[mapSizeX, mapSizeY];        
    }

    public void LoadLevel(float x, float y)
    {
        int xPos = /*((int)x / 2) + 3*/ (int)x + 3;
        int yPos = /*((int)y / 2) + 3*/(int)y + 3;
        string fileName = levels[xPos, yPos];
        tiles = TestMaps.LoadLevel(fileName, mapSizeX, mapSizeY);

    }

    public void CreateMap()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                Instantiate(tileTypes[tiles[x, y]].tileVisual, new Vector3(x * 2, y * 2, 0), Quaternion.identity);
            }
        }
        int enemyNo = Random.Range(3, 6);
        controller.totalEnemies = enemyNo;

        for (int i = 0; i < enemyNo; i++)
        {
            Vector3 position = new Vector3(Random.Range((mapSizeX / 2), ((3 * mapSizeX) / 2)), Random.Range((mapSizeY / 2), ((3 * mapSizeY) / 2)), 10);
            Instantiate(enemyPrefab, position, Quaternion.identity);
        }
    }

    public void CreateGraph()
    {
        int graphX = (mapSizeX * 2) + 1;
        int graphY = (mapSizeY * 2) + 1;
        navGraph = new Node[graphX, graphY];

        // Initialise the navigation graph;
        for (int x = 0; x < graphX; x++)
        {
            for (int y = 0; y < graphY; y++)
            {
                navGraph[x, y] = new Node();

                float worldX = x; //+ 32;
                float worldY = y; //+ 32;
                navGraph[x, y].worldPos = new Vector3(worldX, worldY, 1);
                //Debug.Log("X: " + x + " Y: " + y + " World X: " + navGraph[x, y].worldPos.x + " World Y: " + navGraph[x, y].worldPos.y);
            }
        }

        // Calculate the Nodes' neighbours
        for (int x = 0; x < graphX; x++)
        {
            for (int y = 0; y < graphY; y++)
            {

                // Up
                if (y > 0)
                    navGraph[x, y].neighbours.Add(navGraph[x, y - 1]);

                // Down
                if (y < mapSizeY - 1)
                    navGraph[x, y].neighbours.Add(navGraph[x, y + 1]);

                // Left
                if (x > 0)
                {
                    navGraph[x, y].neighbours.Add(navGraph[x - 1, y]);
                    if (y > 0)
                        navGraph[x, y].neighbours.Add(navGraph[x - 1, y - 1]);
                    if (y < mapSizeY - 1)
                        navGraph[x, y].neighbours.Add(navGraph[x - 1, y + 1]);
                }

                // Right
                if (x < mapSizeX - 1)
                {
                    navGraph[x, y].neighbours.Add(navGraph[x + 1, y]);
                    if (y > 0)
                        navGraph[x, y].neighbours.Add(navGraph[x + 1, y - 1]);
                    if (y < mapSizeY - 1)
                        navGraph[x, y].neighbours.Add(navGraph[x + 1, y + 1]);
                }
            }
        }
    }
}

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
        /*if (Application.loadedLevelName == "ActionScene")
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
                int enemyNo = Random.Range(3, 6);
				controller.totalEnemies = enemyNo;

                for (int i = 0; i < enemyNo; i++)
                {
                    Vector3 position = new Vector3(Random.Range((mapSizeX / 2), ((3 * mapSizeX) / 2)), Random.Range((mapSizeY / 2), ((3 * mapSizeY) / 2)), 10);
                    Instantiate(enemyPrefab, position, Quaternion.identity);
                }

                mapCreated = true;
            }
        }*/
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

    void CreateMap()
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

    void CreateGraph()
    {
        int graphX = mapSizeX * 4;
        int graphY = mapSizeY * 4;
        navGraph = new Node[graphX, graphY];

        // Initialise the navigation graph;
        for (int x = 0; x < graphX; x++)
        {
            for (int y = 0; y < graphY; y++)
            {
                navGraph[x, y] = new Node();

                float graphXf = graphX;
                float graphYf = graphY;
                float worldX = (graphXf / 2) + (graphXf / 3);
                float worldY = 0;
                navGraph[x, y].worldPos = new Vector2(worldX, worldY);
            }
        }

        // Calculate the Nodes' neighbours
        for (int x = 0; x < graphX; x++)
        {
            for (int y = 0; y < graphY; y++)
            {
                navGraph[x, y] = new Node();

                // Up
                if (y > 0)
                    navGraph[x, y].neighbours.Add(navGraph[x, y - 1]);

                // Down
                if (y < mapSizeY - 1)
                    navGraph[x, y].neighbours.Add(navGraph[x, y + 1]);

                // Left
                if (x > 0)
                    navGraph[x, y].neighbours.Add(navGraph[x - 1, y]);

                // Right
                if (x < mapSizeX - 1)
                    navGraph[x, y].neighbours.Add(navGraph[x - 1, y]);              
            }
        }
    }
}

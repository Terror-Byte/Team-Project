using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

    int mapSizeX = 31;
    int mapSizeY = 22;
    TileType[] tileTypes;
    int[,] tiles;
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
                int enemyNo = Random.Range(3, 6);

                for (int i = 0; i < enemyNo; i++)
                {
                    Vector3 position = new Vector3(Random.Range((mapSizeX / 2), ((3 * mapSizeX) / 2)), Random.Range((mapSizeY / 2), ((3 * mapSizeY) / 2)), 10);
                    Instantiate(enemyPrefab, position, Quaternion.identity);
                }

                mapCreated = true;
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
}

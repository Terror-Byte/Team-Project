using UnityEngine;
using System.Collections.Generic;

public class LevelGen : MonoBehaviour {

    //int mapSizeX = 31;
    //int mapSizeY = 22;

    int mapSizeX = 50;
    int mapSizeY = 50;

    int tileAmount = 0;
    TileType[] tileTypes;
    int[,] tiles;

    public GameObject grassPrefab;
    public GameObject sandPrefab;
    public GameObject waterPrefab;

    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public GameObject treePrefab;
    //public GameObject enemyPrefab;

    GameController game;
	// Use this for initialization
	void Start () {
        game = GameObject.Find("GameController").GetComponent<GameController>();
        CreateLevel();
        StartLevel();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void StartLevel()
    {



        // Spawns enemies
        int enemyNo = Random.Range(5, 10);
        game.totalEnemies = enemyNo;

        for (int i = 0; i < enemyNo; i++)
        {
            int randEnemy = Random.Range(0, enemyPrefabs.Count);
            Vector3 position = new Vector3(Random.Range((mapSizeX / 2), ((3 * mapSizeX) / 2)), Random.Range((mapSizeY / 2), ((3 * mapSizeY) / 2)), 10);
            GameObject tmp = (GameObject)Instantiate(enemyPrefabs[randEnemy], position, Quaternion.identity);
            //tmp.SendMessage("ScaleStats", game.difficultyLevel);
        }

        //Spawns trees
        int treeNo = Random.Range(3, 6);
        
        for (int i = 0; i < treeNo; i++)
        {
            Vector3 position = new Vector3(Random.Range((mapSizeX / 2), ((3 * mapSizeX) / 2)), Random.Range((mapSizeY / 2), ((3 * mapSizeY) / 2)), 10);
            Instantiate(treePrefab, position, Quaternion.identity);
        }
    }

    void CreateLevel()
    {
        // Initialises tile types
        tileTypes = new TileType[3];
        tileTypes[0] = new TileType("Grass", grassPrefab, true);
        tileTypes[1] = new TileType("Sand", sandPrefab, true);
        tileTypes[2] = new TileType("Water", waterPrefab, false);
        tiles = new int[mapSizeX, mapSizeY];

        //// Selects random level to load
        //int levelNo = Random.Range(1, 4);

        //switch (levelNo)
        //{
        //    case 1:
        //        tiles = TestMaps.LoadLevel("level1.txt", mapSizeX, mapSizeY);
        //        break;

        //    case 2:
        //        tiles = TestMaps.LoadLevel("level2.txt", mapSizeX, mapSizeY);
        //        break;

        //    case 3:
        //        tiles = TestMaps.LoadLevel("level3.txt", mapSizeX, mapSizeY);
        //        break;
        //}

        
        Vector2 point = new Vector2(24, 24);

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 2;
            }
        }

        tileAmount = Random.Range(800, 1000);

        tiles[24, 24] = 1;
        tiles[24, 25] = 1;
        tiles[25, 24] = 1;
        tiles[23, 24] = 1;
        tiles[24, 23] = 1;

        //Debug.Log(dir);
        AttemptToPlace(point, 2);

        // Spawns the level
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (IsGrass(new Vector2(x, y)))
                {
                    tiles[x, y] = 0;
                }
                Instantiate(tileTypes[tiles[x, y]].tileVisual, new Vector3(x * 2, y * 2, 0), Quaternion.identity);
            }
        }
    }

    void AttemptToPlace(Vector2 p, int i)
    {

        int dir = Random.Range(1, 5);

        switch (dir)
        {
            case 1: //right
                p.x++;
                break;
            case 2: //down
                p.y++;
                break;
            case 3:
                p.x--; //left
                break;
            case 4:
                p.y--; //up
                break;
        }

        if (p.y + 1 < mapSizeY)
            tiles[(int)p.x, (int)p.y + 1] = 1;

        if (p.y - 1 >= 0)
            tiles[(int)p.x, (int)p.y - 1] = 1;

        if (p.x + 1 < mapSizeX)
            tiles[(int)p.x + 1, (int)p.y] = 1;

        if (p.x - 1 >= 0)
            tiles[(int)p.x - 1, (int)p.y] = 1;


        int r = Random.Range(0, 100);
        if (r == 0)
            p = new Vector2(24, 24);

        
        tileAmount--;

        if (tileAmount > 0)
            AttemptToPlace(p, 2);

    }

    bool IsGrass(Vector2 p)
    {
        int sandAmount = Random.Range(1, 3);
        try
        {
            if (tiles[(int)p.x + sandAmount, (int)p.y + sandAmount] != 2 && tiles[(int)p.x - sandAmount, (int)p.y - sandAmount] != 2 && tiles[(int)p.x + sandAmount, (int)p.y - sandAmount] != 2 && tiles[(int)p.x - sandAmount, (int)p.y + sandAmount] != 2)
                return true;
            else
                return false;
        }
        catch
        {
            return false;
        }
    }
}

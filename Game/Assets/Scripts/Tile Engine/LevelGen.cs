﻿using UnityEngine;
using System.Collections;

public class LevelGen : MonoBehaviour {

    int mapSizeX = 31;
    int mapSizeY = 22;
    TileType[] tileTypes;
    int[,] tiles;

    public GameObject grassPrefab;
    public GameObject sandPrefab;
    public GameObject waterPrefab;
    public GameObject enemyPrefab;

    GameObject gameControllerObj;
	// Use this for initialization
	void Start () {
        gameControllerObj = GameObject.Find("GameController");
        CreateLevel();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void CreateLevel()
    {
        // Initialises tile types
        tileTypes = new TileType[3];
        tileTypes[0] = new TileType("Grass", grassPrefab, true);
        tileTypes[1] = new TileType("Sand", sandPrefab, true);
        tileTypes[2] = new TileType("Water", waterPrefab, false);
        tiles = new int[mapSizeX, mapSizeY];    

        // Selects random level to load
        int levelNo = Random.Range(1, 4);

        switch(levelNo)
        {
            case 1:
                tiles = TestMaps.LoadLevel("level1.txt", mapSizeX, mapSizeY);
                break;

            case 2:
                tiles = TestMaps.LoadLevel("level2.txt", mapSizeX, mapSizeY);
                break;

            case 3:
                tiles = TestMaps.LoadLevel("level3.txt", mapSizeX, mapSizeY);
                break;
        }

        // Spawns the level
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                Instantiate(tileTypes[tiles[x, y]].tileVisual, new Vector3(x * 2, y * 2, 0), Quaternion.identity);
            }
        }

        // Spawns enemies
        int enemyNo = Random.Range(3, 6);
        gameControllerObj.GetComponent<GameController>().totalEnemies = enemyNo;

        for (int i = 0; i < enemyNo; i++)
        {
            Vector3 position = new Vector3(Random.Range((mapSizeX / 2), ((3 * mapSizeX) / 2)), Random.Range((mapSizeY / 2), ((3 * mapSizeY) / 2)), 10);
            Instantiate(enemyPrefab, position, Quaternion.identity);
        }
    }
}

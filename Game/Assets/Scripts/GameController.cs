using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    //Player Stats
    //Will be contained over level changes
    [Header("Player Stats")]
    public float currentHp;
    public float baseHp = 100;
    public int hpLvl = 1;
    public int strength = 5;
    public int speed = 1;
    public int dex = 1;
    public int xp = 0;
    //public int level = 0;
    public int gold = 0;

    //Hidden stat lvl up requirements
    public int hpGold;
    public int strGold;
    public int spdGold;
    public int dexGold;

    [Header("Weapon Stats")]
    public float wepDmg = 3;
    public float weaponSpd = 10.0f;
    public float weaponRefresh = 1.0f;

    // Working under the assumption that these variables will augment the above weapon stats
    [Header("Weapon mod Stats")]
    public string weaponName = "Default";
    //public enum weaponTypes { sword, spear, axe, bow };
    //public weaponTypes weaponType = weaponTypes.bow;
    public float weaponDmgMod = 3.0f;
    public float weaponRangeMod = 3.0f;
    public float weaponRefreshMod = 3.0f;
    public GameObject weapon;

    //Lvl stats
    [Header("Level Stats")]
    public int maxDifficultyCompleted = 1;
    public int difficultyLevel = 0;

    [Header("Enemies")]
    public GameObject[] gos;
    public int totalEnemies;

    // Use this for initialization
    void Start()
    {
        //origin = Camera.main.transform.position;
        DontDestroyOnLoad(this.gameObject);
        LvlUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.loadedLevelName == "Scene1" || Application.loadedLevelName == "ActionScene")
        {
            //if (player == null)
            //player = GameObject.Find ("Player");
            gos = GameObject.FindGameObjectsWithTag("Enemy");
            totalEnemies = gos.Length;
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

    void Finish()
    {
        maxDifficultyCompleted = (int)Mathf.Max(difficultyLevel + 1, maxDifficultyCompleted);
        Application.LoadLevel("Select");
    }

    void LvlUpdate()
    {
        hpGold = LvlUpCost(hpLvl);
        strGold = LvlUpCost(strength);
        spdGold = LvlUpCost(speed);
        dexGold = LvlUpCost(dex);
    }

    int LvlUpCost(int lvl)
    {
        return Mathf.RoundToInt((Mathf.Pow(lvl, 2) + lvl)/2);
    }
}
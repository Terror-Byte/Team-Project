using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {

    public string name = "weapon";

    public float range = 1;
    public float damage = 1;
    public float dex = 1;

    public string[] prefixRange = { "Blinded", "Basic", "Long", "Sighted", "Reaching" };
    public string[] prefixDamage = { "Pitiful", "Basic", "Sharp", "Dreaded", "Godly" };
    public string[] prefixDex = { "Sluggish", "Basic", "Speedy", "Agile", "Hasty" };
    public float[] modifiers = { -1.0f, 0.0f, 2.0f, 1.5f, 2.0f };

    public Sprite[] weaponsSprites;

    public GameController game;
	// Use this for initialization
	void Start () 
    {
        game = GameObject.Find("GameController").GetComponent<GameController>();

        //Code to change the weapon sprite, will use this to show the level of the weapon
        //int weapon = Random.Range(0, weaponsSprites.Length);
        //this.GetComponent<SpriteRenderer>().sprite = weaponsSprites[weapon];

        //amount of stats to be changed, 0, 1 or 2
        int i = Random.Range(0, 3);

        SelectPrefabs(i);

    }
	
	// Update is called once per frame
	void Update () 
    {
	    //no need to update anything
	}

    void SelectPrefabs(int i)
    {
        float rangeTmp = range;
        float damageTmp = damage;
        float dexTmp = dex;

        //each case will check for modifications to the stat already and will go to the next
        //stat if it has already been modified.
        for (int p = 0; p < i; p++)
        {
            int k = Random.Range(0, 3);
            switch (k)
            {
                case 0:
                    if (range == rangeTmp)
                    {
                        range += SelectRandom(0);
                        break;
                    }
                    else
                        goto case 1;

                case 1:
                    if (damage == damageTmp)
                    {
                        damage += SelectRandom(1);
                        break;
                    }
                    else
                        goto case 2;

                case 2:
                    if (dex == dexTmp)
                    {
                        dex += SelectRandom(2);
                        break;
                    }
                    else
                        goto case 0;
            }
        }

    }

    //returns a weighted random number
    float SelectRandom(int n)
    {

        //The first element is randomly generated and is the actual mod on the weapon stat 
        //and the second value is the probability of it occuring. All probabilities must add up to 1.0f
        List<KeyValuePair<float, float>> elements = new List<KeyValuePair<float, float>>
        {
            new KeyValuePair<float, float>(Random.Range(-1.5f, -0.5f), 0.1f), 
            new KeyValuePair<float, float>(Random.Range(-0.5f, 0.0f), 0.25f),
            new KeyValuePair<float, float>(Random.Range(0.0f, 1.5f), 0.5f),
            new KeyValuePair<float, float>(Random.Range(1.5f, 2.5f), 0.1f), 
            new KeyValuePair<float, float>(Random.Range(2.5f, 3.0f), 0.05f), 
        };

        float roll = Random.Range(0.0f, 1.0f);

        float cumulative = 0.0f;

        for (int i = 0; i < elements.Count; i++)
        {
            cumulative += elements[i].Value;
            if (roll < cumulative)
            {
                ModName(n, i);
                return (float)System.Math.Round(elements[i].Key, 2);
            }
        }

        //Used for testing weapon drops
        //Should never reach this 
        return 1000.0f;
    }

    void ModName(int n, int i)
    {
        switch (n)
        {
            case 0:
                name = prefixRange[i] + " " + name;
                break;
            case 1:
                name = prefixDamage[i] + " " + name;
                break;
            case 2:
                name = prefixDex[i] + " " + name;
                break;
        }

    }
}

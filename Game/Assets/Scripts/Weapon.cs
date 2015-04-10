using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public int range = 1;
    public int damage = 1;
    public int dex = 1;

    public string[] prefixRange = { "Blinded", "Basic", "Long", "Sighted", "Reaching" };
    public string[] prefixDamage = { "Pitiful", "Basic", "Sharp", "Dreaded", "Godly" };
    public string[] prefixDex = { "Sluggish", "Basic", "Speedy", "Agile", "Hasty" };
    public float[] modifiers = { 0.8f, 1.0f, 1.2f, 1.5f, 2.0f };

    public Sprite[] weaponsSprites;

    public GameController game;
	// Use this for initialization
	void Start () 
    {
        game = GameObject.Find("GameController").GetComponent<GameController>();

        int weapon = Random.Range(0, weaponsSprites.Length);
        this.GetComponent<SpriteRenderer>().sprite = weaponsSprites[weapon];
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}

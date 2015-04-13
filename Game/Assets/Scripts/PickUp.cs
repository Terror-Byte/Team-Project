using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

    public GameObject weapon;
    public bool isTrue = false;
    GameController game;
	// Use this for initialization
	void Start () 
    {
        game = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (Input.GetKeyUp(KeyCode.E) && weapon != null)
        {
            if (game.weapon != null)
            {
                game.weapon.transform.position = GameObject.Find("Player").transform.position;
                game.weapon.transform.SetParent(null);
            }

            game.weapon = weapon;
            game.weapon.transform.SetParent(game.transform);
            weapon.transform.position = (new Vector3(100, 100, -100));
            game.weaponName = weapon.GetComponent<Weapon>().name;
            game.weaponDmgMod = weapon.GetComponent<Weapon>().damage;
            game.weaponRangeMod = weapon.GetComponent<Weapon>().range;
            game.weaponRefreshMod = weapon.GetComponent<Weapon>().dex;
            //Destroy(weapon);
        }
	}

    void OnTriggerStay2D(Collider2D wep)
    {
        if (wep.tag == "Weapon")
        {
            isTrue = true;
            weapon = wep.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D wep)
    {
        isTrue = false;
        weapon = null;
    }
}

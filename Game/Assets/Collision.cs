using UnityEngine;
using System.Collections;

public class Collision : MonoBehaviour {

    public bool inExit;
    GameController game;

	// Use this for initialization
	void Start () 
    {
        inExit = false;
        game = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Finish")
        {
            inExit = true;
            if (Input.GetKeyDown(KeyCode.Space) && game.totalEnemies == 0)
            {
                game.SendMessage("Finish");
                //Debug.Log("Finish");
            }
        }
    }
}

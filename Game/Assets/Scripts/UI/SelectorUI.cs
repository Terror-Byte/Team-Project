using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectorUI : MonoBehaviour {

    Text healthText;
    Text strengthText;
    Text speedText;

    GameController game;

	// Use this for initialization
	void Start () {
        healthText = GameObject.Find("Health Up").GetComponent<Text>();
        strengthText = GameObject.Find("Strength Up").GetComponent<Text>();
        speedText = GameObject.Find("Speed Up").GetComponent<Text>();

        game = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

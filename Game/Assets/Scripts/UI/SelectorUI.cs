using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectorUI : MonoBehaviour {

    public Button healthButton;
    public Button strengthButton;
    public Button speedButton;

    public Text healthText;
    public Text strengthText;
    public Text speedText;

    GameController game;

	void Start () {

        //healthButton = GameObject.Find("Health Up").GetComponent<Button>();
        //strengthButton = GameObject.Find("Strength Up").GetComponent<Button>();
        //speedButton = GameObject.Find("Speed Up").GetComponent<Button>();

        healthButton.onClick.AddListener(() => HealthUp());
        strengthButton.onClick.AddListener(() => StrengthUp());
        speedButton.onClick.AddListener(() => SpeedUp());

        //healthText = healthButton.GetComponent<Text>();
        //strengthText = strengthButton.GetComponent<Text>();
        //speedText = speedButton.GetComponent<Text>();

        //healthText.text = "Health Up: 1 Gold";
        //strengthText.text = "Strength Up: 1 Gold";
        //speedText.text = "Speed Up: 1 Gold";

        game = GameObject.Find("GameController").GetComponent<GameController>();
	}

    void HealthUp()
    {
        if (game.gold >= game.hpGold)
        {
            game.hpLvl += 1;
            game.gold -= game.hpGold;
            game.SendMessage("LvlUpdate");
        }
    }

    void StrengthUp()
    {
        if (game.gold >= game.strGold)
        {
            game.strength += 1;
            game.gold -= game.strGold;
            game.SendMessage("LvlUpdate");
        }
    }

    void SpeedUp()
    {
        if (game.gold >= game.spdGold)
        {
            game.speed += 1;
            game.gold -= game.spdGold;
            game.SendMessage("LvlUpdate");
        }
    }
}

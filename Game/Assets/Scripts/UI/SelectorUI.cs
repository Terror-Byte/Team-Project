using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectorUI : MonoBehaviour {

    public Button healthButton;
    public Button strengthButton;
    public Button speedButton;
    public Button dexButton;

    public GameObject healthText, healthLevelText;
    public GameObject strengthText, strengthLevelText;
    public GameObject speedText, speedLevelText;
    public GameObject dexText, dexLevelText;
    public GameObject goldText;

    GameController game;

	void Start () {

        game = GameObject.Find("GameController").GetComponent<GameController>();

        healthButton.onClick.AddListener(() => HealthUp());
        strengthButton.onClick.AddListener(() => StrengthUp());
        speedButton.onClick.AddListener(() => SpeedUp());
        dexButton.onClick.AddListener(() => DexUp());

        //healthText.GetComponent<Text>().text = "          Health Up: 1 Gold";
        //strengthText.GetComponent<Text>().text = "          Strength Up: 1 Gold";
        //speedText.GetComponent<Text>().text = "          Speed Up: 1 Gold";
        //dexText.GetComponent<Text>().text = "          Dex Up: 1 Gold";
        healthText.GetComponent<Text>().text = game.hpGold.ToString();
        strengthText.GetComponent<Text>().text = game.strGold.ToString();
        speedText.GetComponent<Text>().text = game.spdGold.ToString();
        dexText.GetComponent<Text>().text = game.dexGold.ToString();

        //healthLevelText.GetComponent<Text>().text = "Health Level: " + game.hpLvl;
        //strengthLevelText.GetComponent<Text>().text = "Strength Level: " + game.strength;
        //speedLevelText.GetComponent<Text>().text = "Speed Level: " + game.speed;
        //dexLevelText.GetComponent<Text>().text = "Dex Level: " + game.dex;
        //goldText.GetComponent<Text>().text = "Gold: " + game.gold;
        healthLevelText.GetComponent<Text>().text = game.hpLvl.ToString();
        strengthLevelText.GetComponent<Text>().text = game.strength.ToString();
        speedLevelText.GetComponent<Text>().text = game.speed.ToString();
        dexLevelText.GetComponent<Text>().text = game.dex.ToString();
        goldText.GetComponent<Text>().text = "- " + game.gold;
	}

    void HealthUp()
    {
        if (game.gold >= game.hpGold)
        {
            game.hpLvl += 1;
            game.gold -= game.hpGold;
            game.SendMessage("LvlUpdate");
            healthLevelText.GetComponent<Text>().text = game.hpLvl.ToString();
            healthText.GetComponent<Text>().text = game.hpGold.ToString();
            goldText.GetComponent<Text>().text = "- " + game.gold;
        }
    }

    void StrengthUp()
    {
        if (game.gold >= game.strGold)
        {
            game.strength += 1;
            game.gold -= game.strGold;
            game.SendMessage("LvlUpdate");
            strengthLevelText.GetComponent<Text>().text = game.strength.ToString();
            strengthText.GetComponent<Text>().text = game.strGold.ToString();
            goldText.GetComponent<Text>().text = "- " + game.gold;
        }
    }

    void SpeedUp()
    {
        if (game.gold >= game.spdGold)
        {
            game.speed += 1;
            game.gold -= game.spdGold;
            game.SendMessage("LvlUpdate");
            speedLevelText.GetComponent<Text>().text = game.speed.ToString();
            speedText.GetComponent<Text>().text = game.spdGold.ToString();
            goldText.GetComponent<Text>().text = "- " + game.gold;
        }
    }

    void DexUp()
    {
        if (game.gold >= game.dexGold)
        {
            game.dex += 1;
            game.gold -= game.dexGold;
            game.SendMessage("LvlUpdate");
            dexLevelText.GetComponent<Text>().text = game.dex.ToString();
            dexText.GetComponent<Text>().text = game.dexGold.ToString();
            goldText.GetComponent<Text>().text = "- " + game.gold;
        }
    }
}

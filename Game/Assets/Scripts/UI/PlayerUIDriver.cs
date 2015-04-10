using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIDriver : MonoBehaviour {

    // ==========OLD UI AND HEALTH STUFF==========
    //public Text levelText;
    /*public Text goldText;
    private float minXValue;
    private float maxXValue;
    public RectTransform healthTransform;
    private float healthY;
    public RectTransform xpTransform;
    private float xpY;*/

    public Text goldText;
    public Text strText;
    public Text dexText;
    public Text hpText;
    public RectTransform healthTransform;
    public float minYValue; // PUBLIC FOR TESTING
    public float maxYValue; // PUBLIC FOR TESTING
    public float healthX;   // PUBLIC FOR TESTING

    // Game over variables
    public GameObject gameOver;
    Text gameOverText;
    public GameObject mainMenu;
    Text mainMenuButton;
    Button menuButton;

    GameController game;
    Movement player;

	// Use this for initialization
	void Start () {
        game = GameObject.Find("GameController").GetComponent<GameController>();
        player = gameObject.GetComponent<Movement>();

        /*healthY = healthTransform.position.y; // Y value of the health bar's position.
        xpY = xpTransform.position.y; // Y value of the XP bar's position.
        maxXValue = healthTransform.position.x; // Maximum position of bars.
        minXValue = healthTransform.position.x - healthTransform.rect.width; // Minimum position of bars.*/

        healthX = healthTransform.position.x; // X value of health bar's position.
        maxYValue = healthTransform.position.y; // Maximum position of bar.
        minYValue = healthTransform.position.y - healthTransform.rect.height;

        gameOver = GameObject.Find("Game Over Text");
        mainMenu = GameObject.Find("Main Menu Button");
        menuButton = mainMenu.GetComponent<Button>();
        menuButton.onClick.AddListener(() => LoadMenu());

        goldText.text = game.gold.ToString();
        strText.text = "Str - " + game.strength.ToString();
        dexText.text = "Dex - " + game.dex.ToString();
        hpText.text = "Hp - " + game.hpLvl.ToString();

        gameOver.SetActive(false);
        mainMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        // Calculations may be a bit off for health and xp bars, fix soon.
        /*float currentHealthX = currentPosition(game.currentHp, 100, maxXValue);
        if (game.currentHp != 0)
        {
            healthTransform.position = new Vector3(currentHealthX, healthY);
        }
        else
        {
            healthTransform.position = new Vector3(healthTransform.position.x - xpTransform.rect.width, healthY);
        }

        if (game.xp != 0)
        {
            float currentXPX = currentPosition(game.xp, player.nextLevelXP, maxXValue);
            xpTransform.position = new Vector3(currentXPX, xpY);
        }
        else
        {
            float currentXPX = xpTransform.position.x - xpTransform.rect.width;
            xpTransform.position = new Vector3(currentXPX, xpY);
        }*/

        float currentHealthY = currentPosition(game.currentHp, player.HP(), maxYValue);
        healthTransform.position = new Vector3(healthX, currentHealthY);
	}

    void LoadMenu()
    {
        Application.LoadLevel("Start");
    }

    float currentPosition(float currentVal, float maxVal, float length)
    {
        // healthPercentage = current health / max health (100 for now)
        // current x = maxXvalue - (maxXvalue * healthPercentage)

        // Percentage = current value / max value
        // current x pos = maxXvalue - (maxVcalue * percentage)

        //float percentage = currentVal / maxVal;
        //float result = maxXvalue - (maxXvalue * percentage);
        //return result;

        //float step = healthTransform.rect.height / maxVal;
        //float percentage = (currentVal / maxVal);// / step;
        //return (minYValue + (percentage * healthTransform.rect.height));

        float percent = currentVal / maxVal;
        float amount = healthTransform.rect.height * percent;
        return (minYValue + amount);
    }
}

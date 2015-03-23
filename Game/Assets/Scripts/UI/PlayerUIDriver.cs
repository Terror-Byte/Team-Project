using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIDriver : MonoBehaviour {

    // Health and XP bar shizzle
    //public Text levelText;
    public Text goldText;
    private float minXValue;
    private float maxXValue;
    public RectTransform healthTransform;
    private float healthY;
    public RectTransform xpTransform;
    private float xpY;

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

        healthY = healthTransform.position.y; // Y value of the health bar's position.
        xpY = xpTransform.position.y; // Y value of the XP bar's position.
        maxXValue = healthTransform.position.x; // Maximum position of bars.
        minXValue = healthTransform.position.x - healthTransform.rect.width; // Minimum position of bars.

        gameOver = GameObject.Find("Game Over Text");
        mainMenu = GameObject.Find("Main Menu Button");
        menuButton = mainMenu.GetComponent<Button>();
        menuButton.onClick.AddListener(() => LoadMenu());

        //levelText.text = "Level: " + game.level;
        goldText.text = "Gold: " + game.gold;

        gameOver.SetActive(false);
        mainMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        // Calculations may be a bit off for health and xp bars, fix soon.
        float currentHealthX = currentPosition(game.currentHp, 100, maxXValue);
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
        }
	}

    void LoadMenu()
    {
        Application.LoadLevel("Start");
    }

    float currentPosition(float currentVal, int maxVal, float maxXvalue)
    {
        // healthPercentage = current health / max health (100 for now)
        // current x = maxXvalue - (maxXvalue * healthPercentage)

        // Percentage = current value / max value
        // current x pos = maxXvalue - (maxVcalue * percentage)
        float currentValue = currentVal;
        float maxValue = (float)maxVal;

        float percentage = currentValue / maxValue;
        float result = maxXvalue * percentage;
        return result;
    }
}

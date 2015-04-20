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
    public Text spdText;
    public Text dexText;
    public Text hpText;
    public RectTransform healthTransform;
    private float minYValue; // PUBLIC FOR TESTING
    private float maxYValue; // PUBLIC FOR TESTING
    private float healthX;   // PUBLIC FOR TESTING
    public Text enemiesRemaining;

    public Text weaponName;
    public Image weaponSprite;
    public Text weaponDmg;
    public Text weaponRange;
    public Text weaponRefresh;

    Sprite sword;
    Sprite spear;
    Sprite bow;
    Sprite axe;

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

        healthX = healthTransform.localPosition.x; // X value of health bar's position.
        maxYValue = healthTransform.localPosition.y; // Maximum position of bar.
        minYValue = healthTransform.localPosition.y - healthTransform.rect.height;

        gameOver = GameObject.Find("Game Over Text");
        mainMenu = GameObject.Find("Main Menu Button");
        menuButton = mainMenu.GetComponent<Button>();
        menuButton.onClick.AddListener(() => LoadMenu());

        goldText.text = game.gold.ToString();
        strText.text = "Str - " + game.strength.ToString();
        spdText.text = "Spd - " + game.speed.ToString();
        dexText.text = "Dex - " + game.dex.ToString();
        hpText.text = "Hp - " + game.hpLvl.ToString();

        enemiesRemaining.text = "There are still enemies near...";

        gameOver.SetActive(false);
        mainMenu.SetActive(false);

        sword = (Sprite)Resources.Load("/Sprites/Weapons/Sword");
        spear = (Sprite)Resources.Load("/Sprites/Weapons/Spear");
        bow = (Sprite)Resources.Load("/Sprites/Weapons/Bow");
        axe = (Sprite)Resources.Load("/Sprites/Weapons/Axe");
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

        if (game.weapon != null)
            weaponSprite.sprite = game.weapon.GetComponent<SpriteRenderer>().sprite;

        weaponName.text = "Name: " + game.weaponName;
        weaponDmg.text = "Dmg: " + game.weaponDmgMod.ToString();
        weaponRange.text = "Range: " + game.weaponRangeMod.ToString();
        weaponRefresh.text = "Fire Rate: " + game.weaponRefreshMod.ToString();

        /*switch (game.weaponType)
        {
            case GameController.weaponTypes.sword:
                weaponSprite.GetComponent<SpriteRenderer>().sprite = sword;
                break;
            case GameController.weaponTypes.spear:
                weaponSprite.GetComponent<SpriteRenderer>().sprite = spear;
                break;
            case GameController.weaponTypes.axe:
                weaponSprite.GetComponent<SpriteRenderer>().sprite = axe;
                break;
            case GameController.weaponTypes.bow:
                weaponSprite.GetComponent<SpriteRenderer>().sprite = bow;
                break;
        }*/

        float currentHealthY = currentPosition(game.currentHp, player.HP(), maxYValue);
        healthTransform.localPosition = new Vector3(healthX, currentHealthY);

        if (game.totalEnemies <= 0)
        {
            enemiesRemaining.text = "All enemies are vanquished!";
        }
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

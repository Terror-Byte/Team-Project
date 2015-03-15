using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Movement : MonoBehaviour {

	public GameObject bulletPrefab;
	//public float speed = 2;
    public float wepDmg = 10;
	public float weaponSpd = 2.5f;
	public float weaponRefresh = 1.0f;
	float refreshCounter;

    Vector3 currVel;

    //private int maxHealth = 100;
    public bool playerLiving;
	//public int health = 100;
    //public int experience = 0;
    //public int level = 1;
    public int nextLevelXP = 100;

    // Health and XP bar shizzle
    public Text levelText;
    public Text goldText;
    private float minXValue;
    private float maxXValue;
    public RectTransform healthTransform;
    private float healthY;
    public RectTransform xpTransform;
    private float xpY;

    // Game over variables
    GameObject gameOver;
    Text gameOverText;
    GameObject mainMenu;
    Text mainMenuButton;
	Button menuButton;

	// Game Controller
	//GameObject gameController;
    GameController game;
    
	// Use this for initialization
	void Start () 
	{
		refreshCounter = 0;
        StartCoroutine(CalcVelocity());

        healthY = healthTransform.position.y; // Y value of the health bar's position.
        xpY = xpTransform.position.y; // Y value of the XP bar's position.
        maxXValue = healthTransform.position.x; // Maximum position of bars.
        minXValue = healthTransform.position.x - healthTransform.rect.width; // Minimum position of bars.
        playerLiving = true;

        gameOver = GameObject.Find("Game Over Text");
        mainMenu = GameObject.Find("Main Menu Button");
		menuButton = mainMenu.GetComponent<Button>();
		menuButton.onClick.AddListener(() => LoadMenu());

		game = GameObject.Find ("GameController").GetComponent<GameController>();
		//gameController.SendMessage ("SendXPToPlayer");

        levelText.text = "Level: " + game.level;
        // goldText.text = "Gold: " + game.gold;
        
        gameOver.SetActive(false);
        mainMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (game.health <= 0)
            PlayerDeath();

        // Calculations may be a bit off for health and xp bars, fix soon.
        float currentHealthX = currentPosition(game.health, 100, maxXValue);
        if (game.health != 0)
        {
            healthTransform.position = new Vector3(currentHealthX, healthY);
        }
        else
        {
            healthTransform.position = new Vector3(healthTransform.position.x - xpTransform.rect.width, healthY);
        }
       
        if (game.xp != 0)
        {
            float currentXPX = currentPosition(game.xp, nextLevelXP, maxXValue);
            xpTransform.position = new Vector3(currentXPX, xpY);
        }
        else
        {
            float currentXPX = xpTransform.position.x - xpTransform.rect.width;
            xpTransform.position = new Vector3(currentXPX, xpY);
        }

        // If the player is alive.
        if (playerLiving)
        {
            //GetAxisRaw does not smooth the input allowing for tighter controls
            Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            float movX = move.x * game.speed * Time.deltaTime;
            float movY = move.y * game.speed * Time.deltaTime;

            transform.Translate(new Vector2(movX, movY));

            // If left button pressed, generate a new bullet and fire.
            if (Input.GetMouseButton(0))
            {
                if (refreshCounter == 0.0f)
                {
                    GameObject player = GameObject.Find("Player");
                    float xPos = player.transform.position.x;
                    float yPos = player.transform.position.y;

                    GameObject newBullet = (GameObject)Instantiate(bulletPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    newBullet.SendMessage("setDamage", wepDmg);
                    newBullet.AddComponent("Rigidbody2D");
                    newBullet.rigidbody2D.gravityScale = 0.0f;
                    Physics2D.IgnoreCollision(collider2D, newBullet.collider2D);

                    Vector3 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
                    Vector3 mousePos = Input.mousePosition;
                    Vector3 forceDirection = mousePos - playerPos;
                    float angle = Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg;

                    Vector3 a = forceDirection.normalized * weaponSpd;

                    //Takes into account the players current direction so that the player can not overtake his own shots
                    newBullet.rigidbody2D.velocity = a - (currVel / 3);
                    newBullet.transform.rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
                    //a.Normalize ();
                    //newBullet.SendMessage ("SetMovX", a.x);
                    //newBullet.SendMessage ("SetMovY", a.y);

                    refreshCounter += Time.deltaTime;
                }
                else if (refreshCounter >= weaponRefresh)
                {
                    refreshCounter = 0;
                }
                else
                {
                    refreshCounter += Time.deltaTime;
                }
            }
            else //Continues adding to refresh counter to so that tapping mouse click works
            {
                refreshCounter += Time.deltaTime;
            }
        }
	}


    //coroutine to calculate current velocity
    IEnumerator CalcVelocity()
    {
        while (Application.isPlaying)
        {
            // Position at frame start
            Vector3 prevPos = transform.position;
            // Wait till it the end of the frame
            yield return new WaitForEndOfFrame();
            // Calculate velocity: Velocity = DeltaPosition / DeltaTime
            currVel = (prevPos - transform.position) / Time.deltaTime;
            //Debug.Log(currVel.normalized);
        }
    }

	void ApplyDamage(int x)
	{
		game.health -= x;
	}


    //may need to rewrite this. Is kind of confusing, instead of having a next level xp, why not just have a formula which
    //calculates the total required xp based on the current level of the player. (I think this is how runescape does it)
    //At the moment, the player can only level up once at a time if the xp gain is more than needed for 2 or more levels.
    void AddExperience(int x)
    {
        if (game.xp + x <= nextLevelXP)
        {
            game.xp += x;
            if (game.xp == nextLevelXP)
                LevelUp(0);
        }
        else
        {
            game.xp += x;
            int xpOverflow = game.xp - nextLevelXP;
            LevelUp(xpOverflow);
        }
    }

    void LevelUp(int overflow)
    {
        game.level++;
        game.xp = overflow;
        nextLevelXP = 100 + ((int)Mathf.Pow(game.level, 2) * 5);
        levelText.text = "Level: " + game.level;
    }

    float currentPosition(int currentVal, int maxVal, float maxXvalue)
    {
        // healthPercentage = current health / max health (100 for now)
        // current x = maxXvalue - (maxXvalue * healthPercentage)

        // Percentage = current value / max value
        // current x pos = maxXvalue - (maxVcalue * percentage)
        float currentValue = (float)currentVal;
        float maxValue = (float)maxVal;

        float percentage = currentValue / maxValue;
        float result = maxXvalue * percentage;
        return result;
    }

    void PlayerDeath()
    {
        playerLiving = false;
        gameObject.renderer.enabled = false;
        gameOver.SetActive(true);
        mainMenu.SetActive(true);
		Destroy (GameObject.FindGameObjectWithTag("GameController"));   
    }

    void LoadMenu()
    {
        Application.LoadLevel("Start");
    }

	void LoadXPFromController(int xp)
	{
		game.xp = xp;
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "WaterTile")
        {
            Debug.Log("Water Tile Collision");
            Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (move.x != 0)
            {
                move.x = -move.x;
            }

            if (move.y != 0)
            {
                move.y = -move.y;
            }

            float movX = move.x * game.speed * Time.deltaTime;
            float movY = move.y * game.speed * Time.deltaTime;

            transform.Translate(new Vector2(movX, movY));
        }
        Debug.Log("Collision!" + coll.gameObject.tag.ToString());
    }
}

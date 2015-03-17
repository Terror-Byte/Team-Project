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

    PlayerUIDriver uiDriver;
	// Game Controller
	//GameObject gameController;
    GameController game;

    public bool inExit = false;

	// Use this for initialization
	void Start () 
	{
		refreshCounter = 0;
        StartCoroutine(CalcVelocity());

        playerLiving = true;

		game = GameObject.Find ("GameController").GetComponent<GameController>();
		//gameController.SendMessage ("SendXPToPlayer");

        uiDriver = gameObject.GetComponent<PlayerUIDriver>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (game.health <= 0)
            PlayerDeath();

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
        uiDriver.levelText.text = "Level: " + game.level;
    }

    void PlayerDeath()
    {
        playerLiving = false;
        gameObject.renderer.enabled = false;      
        uiDriver.gameOver.SetActive(true);
        uiDriver.mainMenu.SetActive(true);
		Destroy (GameObject.FindGameObjectWithTag("GameController"));   
    }

	void LoadXPFromController(int xp)
	{
		game.xp = xp;
	}

    void OnTriggerEnter2D(Collider2D coll)
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
        //Debug.Log("Collision!" + coll.gameObject.tag.ToString());
        if (coll.gameObject.tag == "Gold")
        {
            Destroy(coll.gameObject);
            int amount = Random.Range(0, 4);
            switch (amount)
            {
                case 0:
                    game.gold += 25;
                    break;
                case 1:
                    game.gold += 50;
                    break;
                case 2:
                    game.gold += 75;
                    break;
                case 3:
                    game.gold += 100;
                    break;
            }
            uiDriver.goldText.text = "Gold: " + game.gold;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Finish")
        {
            inExit = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                game.SendMessage("Finish");
                Debug.Log("Finish");
            }
        }
    }
}

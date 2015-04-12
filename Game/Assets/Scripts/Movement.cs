using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Movement : MonoBehaviour {

	public GameObject bulletPrefab;

	float refreshCounter;

    Vector3 currVel;

    public bool playerLiving;

    public int nextLevelXP = 100;

    PlayerUIDriver uiDriver;

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

        game.currentHp = HP();
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (game.currentHp <= 0)
            PlayerDeath();

        // If the player is alive.
        if (playerLiving)
        {
            //GetAxisRaw does not smooth the input allowing for tighter controls
            Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            float movX = move.x * Speed() * Time.deltaTime;
            float movY = move.y * Speed() * Time.deltaTime;

            this.rigidbody2D.velocity = new Vector2(movX, movY);
            //transform.Translate(new Vector2(movX, movY));
            
            // If left button pressed, generate a new bullet and fire.
            if (Input.GetMouseButton(0))
            {
                if (refreshCounter == 0.0f)
                {
                    GameObject player = GameObject.Find("Player");
                    float xPos = player.transform.position.x;
                    float yPos = player.transform.position.y;

                    GameObject newBullet = (GameObject)Instantiate(bulletPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    newBullet.SendMessage("setDamage", GetDmg());
                    newBullet.AddComponent("Rigidbody2D");
                    newBullet.rigidbody2D.gravityScale = 0.0f;
                    Physics2D.IgnoreCollision(collider2D, newBullet.collider2D);

                    Vector3 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
                    Vector3 mousePos = Input.mousePosition;
                    Vector3 forceDirection = mousePos - playerPos;
                    float angle = Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg;

                    forceDirection.z = 0; //Set the Z value of the calculated firection vector to 0 to normalize in 2d space correctly.
                    Vector3 a = (forceDirection).normalized;

                    //Takes into account the players current direction so that the player can not overtake his own shots
                    newBullet.rigidbody2D.velocity = (a * game.weaponSpd);
                    newBullet.transform.rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);

                    refreshCounter += Time.deltaTime;
                }
                else if (refreshCounter >= Dex())
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
		game.currentHp -= x;
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
        //uiDriver.levelText.text = "Level: " + game.level;
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

    void OnCollisionEnter2D(Collision2D coll)
    {
        //if (coll.gameObject.tag == "WaterTile")
        //{
        //    Debug.Log("Water Tile Collision");
        //    Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //    if (move.x != 0)
        //    {
        //        move.x = -move.x;
        //    }

        //    if (move.y != 0)
        //    {
        //        move.y = -move.y;
        //    }

        //    float movX = move.x * game.speed * Time.deltaTime;
        //    float movY = move.y * game.speed * Time.deltaTime;

        //    transform.Translate(new Vector2(movX, movY));
        //}
        //Debug.Log("Collision!" + coll.gameObject.tag.ToString());
        if (coll.gameObject.tag == "Gold")
        {
            Destroy(coll.gameObject);
            game.gold++;
            //int amount = Random.Range(0, 4);
            //switch (amount)
            //{
            //    case 0:
            //        game.gold += 25;
            //        break;
            //    case 1:
            //        game.gold += 50;
            //        break;
            //    case 2:
            //        game.gold += 75;
            //        break;
            //    case 3:
            //        game.gold += 100;
            //        break;
            //}
            uiDriver.goldText.text = game.gold.ToString();
        }

        if (coll.gameObject.tag == "WaterTile")
        {
            //Debug.Log("Player collides with water");
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
                //Debug.Log("Finish");
            }
        }
    }

    float GetDmg()
    {
        float str = game.strength;
        float dmg = game.wepDmg;

        float tmp = (dmg + str) * (str / 5 + dmg / 10);

        //Debug.Log(Random.Range((tmp * 0.9f), (tmp * 1.1f)));

        return Random.Range((tmp * 0.9f), (tmp * 1.1f));
    }

    float Speed()
    {
        float spd = game.speed;

        return (spd * 13) + 235;
    }

    public float HP()
    {
        int hp = game.hpLvl;
        float baseHp = game.baseHp;

        float tmp = baseHp * (0.01f * hp * hp) + baseHp;
        //Debug.Log(tmp);
        return tmp;
    }

    float Dex()
    {
        int lvl = game.dex;
        float weaponMod = game.weaponRefresh / 10 + (0.5f / game.weaponRefresh);

        double tmp = Mathf.Pow(lvl, -weaponMod);
        //Debug.Log(tmp);
        return (float)tmp;
    }
}

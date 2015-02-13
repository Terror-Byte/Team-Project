using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Movement : MonoBehaviour {

	public GameObject bulletPrefab;
	public float speed = 2;
    public float wepDmg = 10;
	public float weaponSpd = 2.5f;
	public float weaponRefresh = 1.0f;
	float refreshCounter;

    Vector3 currVel;

    private int maxHealth = 100;
	public int health = 100;
    public int experience = 0;
    public int level = 1;
    public int nextLevelXP = 100;

    /*
    public Image healthBar;
    public Image expBar;
    Sprite healthSprite;
    float maxWidthHP;
    float maxWidthXP;
    */

    // Health and XP bar shizzle
    public Text levelText;
    private float minXValue;
    private float maxXValue;
    public RectTransform healthTransform;
    private float healthY;
    public RectTransform xpTransform;
    private float xpY;

	// Use this for initialization
	void Start () 
	{
		refreshCounter = 0;
        StartCoroutine(CalcVelocity());

        healthY = healthTransform.position.y; // Y value of the health bar's position.
        xpY = xpTransform.position.y; // Y value of the XP bar's position.
        maxXValue = healthTransform.position.x; // Maximum position of bars.
        minXValue = healthTransform.position.x - healthTransform.rect.width; // Minimum position of bars.
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (health <= 0)
			Destroy (this.gameObject);

        float currentHealthX = currentPosition(health, 100, maxXValue);       
        healthTransform.position = new Vector3(currentHealthX, healthY);
        if (experience != 0)
        {
            float currentXPX = currentPosition(experience, nextLevelXP, maxXValue);
            xpTransform.position = new Vector3(currentXPX, xpY);
        }
        else
        {
            float currentXPX = xpTransform.position.x - xpTransform.rect.width;
            xpTransform.position = new Vector3(currentXPX, xpY);
        }
        

        
        //GetAxisRaw does not smooth the input allowing for tighter controls
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float movX = move.x * speed * Time.deltaTime;
        float movY = move.y * speed * Time.deltaTime;

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
                newBullet.rigidbody2D.velocity = a - (currVel/3);
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
		health -= x;
	}

    void AddExperience(int x)
    {
        if (experience + x <= nextLevelXP)
        {
            experience += x;
            if (experience == nextLevelXP)
                LevelUp(0);
        }
        else
        {
            experience += x;
            int xpOverflow = experience - nextLevelXP;
            LevelUp(xpOverflow);
        }
    }

    void LevelUp(int overflow)
    {
        level++;
        experience = overflow;
        nextLevelXP = 100 + ((int)Mathf.Pow(level, 2) * 5);
        levelText.text = "Level: " + level;
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
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Enemy : MonoBehaviour {

    public double health = 50;
	public float speed = 1;
    public float dmg = 3;
    public float str = 5;
    public float range = 3;
    public Sprite projectile;
    float shotsfired = 0;
	public enum state { Roam, Attack };
	public state aiState = state.Roam;
	bool hasTarget = false;
    GameController gameController;
	GameObject player;
    Movement moveScript;

    // Pathfinding Stuff
    //LevelGenerator levelGen;
    //Node[,] navGraph;
    //Node currentPosNode;
    //List<Node> currentPath = null;
	//Node target = new Node();

    Vector2 target = new Vector2();
	Vector2 enemyPos = new Vector2();
	Vector2 playerPos = new Vector2();
    //Vector2 initialPos = new Vector2();

	// Weapon related variables
	Vector3 currVel;
	public GameObject bulletPrefab;
	public float weaponSpd = 15f; // Speed of projectile
	public float weaponRefresh = 0.1f; // Shooting speed
	float refreshCounter;
    public bool canShoot360 = false;

    //drops
    public List<GameObject> weapons = new List<GameObject>();
    public GameObject sword;
    public GameObject coin;

    public int experience = 25;

    // Shooting Enum
    public enum ShootingType { Single, Spread, Rotary};
    public ShootingType shootType;

    List<GameObject> tileList = new List<GameObject>();

    // Boss stuff
    [Header("Boss Stuff")]
    public bool isBoss = false;
    public float batNo = 0.0f;
    public GameObject bat;

	// Use this for initialization
	void Start () 
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>(); 
        //levelGen = gameController.GetComponent<LevelGenerator>();
        //navGraph = levelGen.navGraph;

        ScaleStats();

        int rand = Random.Range(0, 8);

        if (rand == 0)
        {
            if (canShoot360)
            {
                shootType = ShootingType.Rotary;
            }
            else
            {
                shootType = ShootingType.Spread;
            }
        }
        else if (rand >= 1 && rand <= 3)
        {
            shootType = ShootingType.Single;
        }
        else if (rand <= 4)
        {
            shootType = ShootingType.Spread;
        }

        if (isBoss)
        {
            shootType = ShootingType.Rotary;
        }
        /*
        switch (rand)
        {
            case 0: 
                shootType = ShootingType.Single;
                break;

            case 1:
                shootType = ShootingType.Spread;
                break;

            case 2:
                shootType = ShootingType.Rotary;
                break;
        }
        */

        //tileList.AddRange(GameObject.FindGameObjectsWithTag("WaterTile").ToList<GameObject>());
        tileList.AddRange(GameObject.FindGameObjectsWithTag("SandTile").ToList<GameObject>());
        tileList.AddRange(GameObject.FindGameObjectsWithTag("GrassTile").ToList<GameObject>());
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
            //player.SendMessage("AddExperience", experience);
            gameController.SendMessage("EnemyDied");
            if (isBoss)
                BossDrop();
            else
                ItemDrop();
        }

		player = GameObject.Find ("Player");
        moveScript = player.GetComponent<Movement>();
        // mousePos = Input.mousePosition; // For testing only

		enemyPos.x = gameObject.transform.position.x;
		enemyPos.y = gameObject.transform.position.y;
		playerPos.x = player.transform.position.x;
		playerPos.y = player.transform.position.y;

        //currentPosNode = ConvertToNode(enemyPos.x, enemyPos.y);

        int distMin = 6;
        if (isBoss)
            distMin = 10;

        Vector2 distToPlayer = playerPos - enemyPos;
        if (distToPlayer.magnitude < distMin)
        {
            aiState = state.Attack;
        }
        else
        {
            aiState = state.Roam;
        }

        if (!moveScript.playerLiving && aiState == state.Attack)
        {
            aiState = state.Roam;
            if (target == playerPos)
            {
                //if (gameObject.layer == 14)
                    //initialPos = gameObject.transform.position; // Only relevant for snake
                hasTarget = false;
            }
        }

		if (aiState == state.Roam)
		{
			if (!hasTarget)
			{
                Vector2 currentPosition = gameObject.transform.position;
                /*if (gameObject.layer == 14)
                {
                    bool hasPos = false;                  
                    while (!hasPos)
                    {                       
                        target = new Vector2(Random.Range(currentPosition.x - 5, currentPosition.x + 5), Random.Range(currentPosition.y - 5, currentPosition.y + 5));
                        if ((target - initialPos).magnitude < 15)
                            hasPos = true;
                    }
                }
                else
                {
                    target = new Vector2(Random.Range(currentPosition.x - 5, currentPosition.x + 5), Random.Range(currentPosition.y - 5, currentPosition.y + 5));
                }*/
                if (gameObject.layer == 14)
                {
                    try
                    {
                        int rand = Random.Range(0, tileList.Count);
                        target = tileList[rand].gameObject.transform.position;
                    }
                    catch
                    {
                        //put this try here for the boss in the first level
                    }
                }
                else
                {
                    target = new Vector2(Random.Range(currentPosition.x - 5, currentPosition.x + 5), Random.Range(currentPosition.y - 5, currentPosition.y + 5));
                }
                
				hasTarget = true;
			}
			else if (hasTarget)
			{
				//Debug.DrawLine(new Vector3(enemyPos.x, enemyPos.y, 1), new Vector3(target.x, target.y, 1), Color.red);
				if (enemyPos.x <= (target.x + 0.1) && enemyPos.x >= (target.x - 0.1) && enemyPos.y <= (target.y + 0.1) && enemyPos.y >= (target.y - 0.1))
				{
					hasTarget = false;
				}

				// Causes enemy to move towards player
				Vector2 vec2Target = target - enemyPos;
				vec2Target.Normalize();
				vec2Target.x = vec2Target.x * speed * Time.deltaTime;
				vec2Target.y = vec2Target.y * speed * Time.deltaTime;

                rigidbody2D.velocity = vec2Target;
                //transform.Translate(vec2Target);
			}
		}
		else if (aiState == state.Attack)
		{
            
			target = playerPos;
			//Debug.DrawLine(new Vector3(enemyPos.x, enemyPos.y, 1), new Vector3(target.x, target.y, 1), Color.red);

			// Causes enemy to move towards player
			Vector2 vec2Target = target - enemyPos;
			if (vec2Target.magnitude > 2)
			{
				vec2Target.Normalize();
				vec2Target.x = vec2Target.x * speed * Time.deltaTime;
				vec2Target.y = vec2Target.y * speed * Time.deltaTime;

                rigidbody2D.velocity = vec2Target;
                //transform.Translate(vec2Target);
			}
                        
			if (refreshCounter == 0.0f)
			{

                switch (shootType)
                {
                    case ShootingType.Single:
                        ShootSingle();
                        break;

                    case ShootingType.Spread:
                        ShootSpread();
                        break;

                    case ShootingType.Rotary:
                        ShootRotary(shotsfired);
                        shotsfired += 7;
                        if (!isBoss)
                        {
                            
                            weaponRefresh = 0.7f;
                            weaponSpd = 6;
                        }
                        else if (isBoss)
                        {
                            weaponRefresh = 0.7f;
                            weaponSpd = 10;

                            batNo++;
                            if (batNo >= 10)
                            {
                                batNo = 0;
                                Vector2 batPos = new Vector2(Random.Range(enemyPos.x, enemyPos.x + 3), Random.Range(enemyPos.y, enemyPos.y + 3));
                                Instantiate(bat, batPos, Quaternion.identity);
                            }
                        }
                        
                        break;
                }
				
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
	}
    
    void ApplyDamage(int x)
    {
        GameObject.Find("EnemyHurt").audio.Play();
        health -= x;
    }

    //Enemy drops - Just basic sword and coin dropping at the moment.
    void ItemDrop()
    {
        int roll = Random.Range(0, 100);
        //Debug.Log(roll);
        if (roll < 50)
        {
            if(weapons.Count != 0)
                SpawnWeapon();
        }
        if (roll < 75)
        {
            spawnGold();
        }
    }

    void BossDrop()
    {
        int maxDrop = 50;
        for (int i = 0; i < maxDrop; i++)
        {
            GameObject drop = (GameObject)Instantiate(coin, new Vector3(enemyPos.x, enemyPos.y, 0), Quaternion.identity);
            drop.transform.localScale = new Vector3(6, 6, 1);
            drop.tag = "BossGold";
            drop.rigidbody2D.AddForce(new Vector2(Random.Range(-300, 300), Random.Range(-300, 300)));
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        /*if (coll.gameObject.tag == "WaterTile")
        {
            Debug.Log("Enemy collided with water");
        }*/
        if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "WaterTile")
        {
            //Debug.Log("Enemy collided with: " + coll.gameObject.tag.ToString());
            target = new Vector2(-target.x, -target.y);
        }
    }

    void ShootSingle()
    {
        GameObject newBullet = InstantiateBullet();

        //Vector3 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector2 forceDirection = target - enemyPos;
        //Debug.Log("Force Direction: " + forceDirection.ToString() + " Normalised: " + forceDirection.normalized);

        float angle = Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg;
        Vector3 a = forceDirection.normalized * weaponSpd;

        //Takes into account the players current direction so that the player can not overtake his own shots
        newBullet.rigidbody2D.velocity = a - (currVel / 3);
        newBullet.transform.rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
    }

    void ShootSpread()
    {
        GameObject newBullet1 = InstantiateBullet();
        GameObject newBullet2 = InstantiateBullet();
        GameObject newBullet3 = InstantiateBullet();

        //Vector3 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector2 forceDirection = target - enemyPos;

        float angle = Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg;
        Vector3 a = forceDirection.normalized * weaponSpd;

        //Takes into account the players current direction so that the player can not overtake his own shots
        newBullet1.rigidbody2D.velocity = a - (currVel / 3);
        newBullet1.transform.rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);

        angle = Mathf.Atan2(forceDirection.y - 1, forceDirection.x - 1) * Mathf.Rad2Deg;
        a = (forceDirection - new Vector2(1, 1)).normalized * weaponSpd;
        newBullet2.rigidbody2D.velocity = a - (currVel / 3);
        newBullet2.transform.rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);

        angle = Mathf.Atan2(forceDirection.y + 1, forceDirection.x + 1) * Mathf.Rad2Deg;
        a = (forceDirection + new Vector2(1, 1)).normalized * weaponSpd;
        newBullet3.rigidbody2D.velocity = a - (currVel / 3);
        newBullet3.transform.rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
    }

    void ShootRotary(float i)
    {
        // This is a dirty hack, but it's the only way I can think of doing this at the moment
        Vector2[] directions = {   
                                   new Vector2(0.0f, 1.0f), new Vector2(0.5f,1.0f), new Vector2(1.0f,1.0f), new Vector2(1.0f, 0.5f),          // Top-Right section
                                   new Vector2(1.0f, 0.0f), new Vector2(1.0f, -0.5f), new Vector2(1.0f, -1.0f), new Vector2(0.5f, -1.0f),     // Bottom-Right section
                                   new Vector2(0.0f, -1.0f), new Vector2(-0.5f, -1.0f), new Vector2(-1.0f, -1.0f), new Vector2(-1.0f, -0.5f), // Bottom-Left section
                                   new Vector2(-1.0f, 0.0f), new Vector2(-1.0f, 0.5f), new Vector2(-1.0f, 1.0f), new Vector2(-0.5f, 1.0f)     // Top-Left section
                               };   

        int directionCount = 0;
        float ang = 0;
        for (float angle = ang; angle < ang + 360; angle += 22.5f)
        {
            GameObject newBullet = InstantiateBullet();
            Vector2 forceDirection = Quaternion.Euler(new Vector3(0,0,i)) * directions[directionCount];
            //Debug.Log("Force Direction: " + forceDirection);
            Vector3 a = forceDirection * weaponSpd;

            newBullet.rigidbody2D.velocity = a - (currVel / 3);
            //Debug.Log("Bullet Velocity: " + newBullet.rigidbody2D.velocity.ToString());
            newBullet.transform.rotation = Quaternion.AngleAxis(-angle + 45 + i, Vector3.forward);
            directionCount++;
        }
    }

   /* void ShootRotaryBoss(float i)
    {
        // This is a dirty hack, but it's the only way I can think of doing this at the moment
        Vector2[] directions = {   
                                   new Vector2(0.0f, 1.0f), new Vector2(0.5f,1.0f), new Vector2(1.0f,1.0f), new Vector2(1.0f, 0.5f),          // Top-Right section
                                   new Vector2(1.0f, 0.0f), new Vector2(1.0f, -0.5f), new Vector2(1.0f, -1.0f), new Vector2(0.5f, -1.0f),     // Bottom-Right section
                                   new Vector2(0.0f, -1.0f), new Vector2(-0.5f, -1.0f), new Vector2(-1.0f, -1.0f), new Vector2(-1.0f, -0.5f), // Bottom-Left section
                                   new Vector2(-1.0f, 0.0f), new Vector2(-1.0f, 0.5f), new Vector2(-1.0f, 1.0f), new Vector2(-0.5f, 1.0f)     // Top-Left section
                               };

        int directionCount = 0;
        float ang = 0;
        for (float angle = ang; angle < ang + 360; angle += 22.5f)
        {
            GameObject newBullet1 = InstantiateBullet();
            GameObject newBullet2 = InstantiateBullet();
            GameObject newBullet3 = InstantiateBullet();

            Vector2 forceDirection = Quaternion.Euler(new Vector3(0, 0, i)) * directions[directionCount];
            Vector3 a = forceDirection * weaponSpd;
            newBullet1.rigidbody2D.velocity = a - (currVel / 3);
            newBullet1.transform.rotation = Quaternion.AngleAxis(-angle + 45 + i, Vector3.forward);

            float modangle = angle - 22.5f;
            a = (forceDirection - new Vector2(0.1f, 0.1f)).normalized * weaponSpd;
            newBullet2.rigidbody2D.velocity = a - (currVel / 3);
            newBullet2.transform.rotation = Quaternion.AngleAxis(-modangle + 45 + i, Vector3.forward);

            modangle = angle + 22.5f;
            a = (forceDirection + new Vector2(0.1f, 0.1f)).normalized * weaponSpd;
            newBullet3.rigidbody2D.velocity = a - (currVel / 3);
            newBullet3.transform.rotation = Quaternion.AngleAxis(-modangle + 45 + i, Vector3.forward);

            directionCount++;
        }
    }*/

    GameObject InstantiateBullet()
    {
        float xPos = enemyPos.x;
        float yPos = enemyPos.y;
        GameObject newBullet = (GameObject)Instantiate(bulletPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
        newBullet.SendMessage("setDamage", GetDmg());
        newBullet.AddComponent("Rigidbody2D");
        newBullet.rigidbody2D.gravityScale = 0.0f;
        Physics2D.IgnoreCollision(collider2D, newBullet.collider2D);
        newBullet.GetComponent<SpriteRenderer>().sprite = projectile;
        newBullet.GetComponent<BulletScript>().range = range;
        newBullet.GetComponent<BulletScript>().source = "Enemy";
        newBullet.layer = 16;

        if (isBoss)
            newBullet.transform.localScale = new Vector3(5, 5, 0);

        return newBullet;
    }

    void spawnGold()
    {
        int maxDrop = Mathf.RoundToInt(((Mathf.Pow(gameController.difficultyLevel, 2) + gameController.difficultyLevel)/2 + 1)/4);

        if (maxDrop < 5)
            maxDrop = 5;

        int tmp = Random.Range(1, maxDrop);
        for (int i = 0; i < tmp; i++)
        {
            GameObject drop = (GameObject)Instantiate(coin, new Vector3(enemyPos.x, enemyPos.y, 0), Quaternion.identity);
            drop.rigidbody2D.AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 150)));
        }
    }

    void SpawnWeapon()
    {
        int tmp = Random.Range(0, weapons.Count);
        GameObject drop = (GameObject)Instantiate(weapons[tmp], new Vector3(enemyPos.x, enemyPos.y, 0), Quaternion.identity);
        drop.rigidbody2D.AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)));
        drop.rigidbody2D.AddTorque(Random.Range(-75, 75));
    }

    float GetDmg()
    {
        float tmp = (dmg + str) * (str / 5 + dmg / 10);

        //Debug.Log(Random.Range((tmp * 0.9f), (tmp * 1.1f)));

        return Random.Range((tmp * 0.9f), (tmp * 1.1f));
    }

    void ScaleStats()
    {
        int lvl = gameController.difficultyLevel;
        double tmp = health;

        health = tmp * (0.007 * (lvl * lvl)) + tmp;
        str = lvl;
    }


















    // Old pathfinding stuff
    //void GeneratePathTo(int x, int y)
    //{
    //    navGraph = levelGen.navGraph;
    //    currentPath = null;

    //    Dictionary<Node, float> distance = new Dictionary<Node, float>();
    //    Dictionary<Node, Node> previous = new Dictionary<Node, Node>();

    //    // List of nodes we haven't checked yet
    //    List<Node> unvisited = new List<Node>();

    //    Node source = currentPosNode;
    //    Node target = navGraph[x, y];
    //    distance[source] = 0.0f;
    //    previous[source] = null;

    //    // Initialise everything to have infinity distance.
    //    foreach (Node vertex in navGraph)
    //    {
    //        if (vertex != source)
    //        {
    //            distance[vertex] = Mathf.Infinity;
    //            previous[vertex] = null;
    //        }
    //        unvisited.Add(vertex);
    //    }

    //    while (unvisited.Count > 0)
    //    {
    //        // Unvisited node with smallest distance
    //        Node unvisitedNode = null; //unvisited.OrderBy(n => distance[n]).First();

    //        foreach (Node possibleUnvisited in unvisited)
    //        {
    //            /*Debug.Log("Possible Unvisited: " + possibleUnvisited.worldPos);
    //            try
    //            {
    //                Debug.Log(distance[possibleUnvisited]);
    //            }
    //            catch(System.Exception e)
    //            {
    //                Debug.Log("Possible Unvisited:" + e.Message);
    //            }

    //            try
    //            {
    //                if (unvisited != null)
    //                {
    //                    Debug.Log(distance[unvisitedNode]);
    //                }
    //                else
    //                {
    //                    Debug.Log("Null");
    //                }
    //            }
    //            catch (System.Exception e)
    //            {
    //                Debug.Log("Unvisited: " + e.Message);
    //            }
    //             * */

    //            if (unvisitedNode == null || distance[possibleUnvisited] < distance[unvisitedNode])
    //            {
    //                unvisitedNode = possibleUnvisited;
    //            }
    //        }

    //        if (unvisitedNode == target)
    //            break;

    //        unvisited.Remove(unvisitedNode);

    //        foreach (Node vertex in unvisitedNode.neighbours)
    //        {
    //            float alt = distance[unvisitedNode] + unvisitedNode.DistanceTo(vertex);
    //            if (alt < distance[vertex])
    //            {
    //                distance[vertex] = alt;
    //                previous[vertex] = unvisitedNode;
    //            }
    //        }
    //    }

    //    // Either a short route to our target has been found, or there is no route at all to our target
    //    if (previous[target] == null)
    //    {
    //        // No route between our target and the source
    //        return;
    //    }

    //    currentPath = new List<Node>();

    //    Node currentNode = target;
    //    // Step through the "previous" chain and add it to the path.
    //    while (currentNode != null)
    //    {
    //        currentPath.Add(currentNode);
    //        currentNode = previous[currentNode];
    //    }

    //    // Now currentPath describes a route from target to source, so we need to invert it.
    //    currentPath.Reverse();
    //    Debug.Log("Success!");
    //}

    //public void MoveNextNode()
    //{
    //    if (currentPath == null)
    //        return;

    //    currentPath.RemoveAt(0);

    //    gameObject.transform.position = currentPath[0].worldPos;

    //    if (currentPath.Count == 1)
    //    {
    //        // We've reached our destination
    //        currentPath = null;
    //        hasTarget = false;
    //    }
    //}

    //Node ConvertToNode(float x, float y)
    //{
    //    Node result = new Node();
    //    foreach (Node nodes in navGraph)
    //    {
    //        if (nodes.worldPos.x <= x + 2 && nodes.worldPos.x >= x - 2 && nodes.worldPos.y <= y + 2 && nodes.worldPos.y <= y - 2)
    //        {
    //            result = nodes;
    //        }
    //    }
    //    //Debug.Log(result.worldPos);
    //    return result;
    //}
}

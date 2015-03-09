using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Enemy : MonoBehaviour {

    public int health = 500;
	public float speed = 1;
    public int wepDmg = 5;
	
	public enum state { Roam, Attack };
	public state aiState = state.Roam;
	bool hasTarget = false;
    GameObject gameController;
	GameObject player;
    Movement moveScript;

    // Pathfinding Stuff
    LevelGenerator levelGen;
    Node[,] navGraph;
    Node currentPosNode;
    List<Node> currentPath = null;

	//Node target = new Node();
    Vector2 target = new Vector2();
	Vector2 enemyPos = new Vector2();
	Vector2 playerPos = new Vector2();
//	Vector2 mousePos = new Vector2(); // For testing only

	// Weapon related variables
	Vector3 currVel;
	public GameObject bulletPrefab;
	public float weaponSpd = 15f; // Speed of projectile
	public float weaponRefresh = 0.1f; // Shooting speed
	float refreshCounter;

    public int experience = 25;

	// Use this for initialization
	void Start () 
    {
        gameController = GameObject.Find("GameController");
        levelGen = gameController.GetComponent<LevelGenerator>();
        navGraph = levelGen.navGraph;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
            player.SendMessage("AddExperience", experience);
            gameController.SendMessage("EnemyDied");
        }

		player = GameObject.Find ("Player");
        moveScript = player.GetComponent<Movement>();
        // mousePos = Input.mousePosition; // For testing only

		enemyPos.x = gameObject.transform.position.x;
		enemyPos.y = gameObject.transform.position.y;
		playerPos.x = player.transform.position.x;
		playerPos.y = player.transform.position.y;

        //currentPosNode = ConvertToNode(enemyPos.x, enemyPos.y);
        
        Vector2 distToPlayer = playerPos - enemyPos;
        if (distToPlayer.magnitude < 6)
            aiState = state.Attack;
        else
            aiState = state.Roam;

        if (!moveScript.playerLiving && aiState == state.Attack)
        {
            aiState = state.Roam;
            if (target == playerPos)
                hasTarget = false;
        }

		if (aiState == state.Roam)
		{
			if (!hasTarget)
			{
				// target = new Vector2(Random.Range(-5, 5), Random.Range (-5, 5));
				Vector2 currentPosition = gameObject.transform.position;
				target = new Vector2(Random.Range(currentPosition.x - 5, currentPosition.x + 5), Random.Range(currentPosition.y  - 5, currentPosition.y + 5));
				hasTarget = true;
			}
			else if (hasTarget)
			{
				Debug.DrawLine(new Vector3(enemyPos.x, enemyPos.y, 1), new Vector3(target.x, target.y, 1), Color.red);
				if (enemyPos.x <= (target.x + 0.1) && enemyPos.x >= (target.x - 0.1) && enemyPos.y <= (target.y + 0.1) && enemyPos.y >= (target.y - 0.1))
				{
					hasTarget = false;
				}

				// Causes enemy to move towards player
				Vector2 vec2Target = target - enemyPos;
				vec2Target.Normalize();
				vec2Target.x = vec2Target.x * speed * Time.deltaTime;
				vec2Target.y = vec2Target.y * speed * Time.deltaTime;
				transform.Translate(vec2Target);
			}
            /*
            if (!hasTarget)
            {
                int xPos = (int)gameObject.transform.position.x + Random.Range(-10, 10);
                int yPos = (int)gameObject.transform.position.y + Random.Range(-10, 10);
                GeneratePathTo(xPos, yPos);
				hasTarget = true;
            }
            else if (hasTarget)
            {
                MoveNextNode();
            }
             * */
		}
		else if (aiState == state.Attack)
		{
            
			target = playerPos;
			Debug.DrawLine(new Vector3(enemyPos.x, enemyPos.y, 1), new Vector3(target.x, target.y, 1), Color.red);

			// Causes enemy to move towards player
			Vector2 vec2Target = target - enemyPos;
			if (vec2Target.magnitude > 2)
			{
				vec2Target.Normalize();
				vec2Target.x = vec2Target.x * speed * Time.deltaTime;
				vec2Target.y = vec2Target.y * speed * Time.deltaTime;
				transform.Translate(vec2Target);
			}
             

            
			if (refreshCounter == 0.0f)
			{
				float xPos = enemyPos.x;
				float yPos = enemyPos.y;
				
				GameObject newBullet = (GameObject)Instantiate(bulletPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                newBullet.SendMessage("setDamage", wepDmg);
				newBullet.AddComponent("Rigidbody2D");
				newBullet.rigidbody2D.gravityScale = 0.0f;
				Physics2D.IgnoreCollision(collider2D, newBullet.collider2D);
				
				//Vector3 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
				Vector2 forceDirection = target - enemyPos;

				float angle = Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg;
				Vector3 a = forceDirection.normalized * weaponSpd;
				
				//Takes into account the players current direction so that the player can not overtake his own shots
				newBullet.rigidbody2D.velocity = a - (currVel/3);
				//newBullet.transform.Translate(a);
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
	}
    
    void ApplyDamage(int x)
    {
        health -= x;
    }

    void GeneratePathTo(int x, int y)
    {
        navGraph = levelGen.navGraph;
        currentPath = null;

        Dictionary<Node, float> distance = new Dictionary<Node, float>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();

        // List of nodes we haven't checked yet
        List<Node> unvisited = new List<Node>();

        Node source = currentPosNode;
        Node target = navGraph[x, y];
        distance[source] = 0.0f;
        previous[source] = null;

        // Initialise everything to have infinity distance.
        foreach (Node vertex in navGraph)
        {
            if (vertex != source)
            {
                distance[vertex] = Mathf.Infinity;
                previous[vertex] = null;
            }
            unvisited.Add(vertex);
        }

        while (unvisited.Count > 0)
        {
            // Unvisited node with smallest distance
            Node unvisitedNode = null; //unvisited.OrderBy(n => distance[n]).First();

            foreach(Node possibleUnvisited in unvisited)
            {
                /*Debug.Log("Possible Unvisited: " + possibleUnvisited.worldPos);
                try
                {
                    Debug.Log(distance[possibleUnvisited]);
                }
                catch(System.Exception e)
                {
                    Debug.Log("Possible Unvisited:" + e.Message);
                }

                try
                {
                    if (unvisited != null)
                    {
                        Debug.Log(distance[unvisitedNode]);
                    }
                    else
                    {
                        Debug.Log("Null");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.Log("Unvisited: " + e.Message);
                }
                 * */

                if (unvisitedNode == null || distance[possibleUnvisited] < distance[unvisitedNode])
                {
                    unvisitedNode = possibleUnvisited;
                }
            }

            if (unvisitedNode == target)
                break;

            unvisited.Remove(unvisitedNode);

            foreach(Node vertex in unvisitedNode.neighbours)
            {
                float alt = distance[unvisitedNode] + unvisitedNode.DistanceTo(vertex);
                if (alt < distance[vertex])
                {
                    distance[vertex] = alt;
                    previous[vertex] = unvisitedNode;
                }
            }
        }

        // Either a short route to our target has been found, or there is no route at all to our target
        if (previous[target] == null)
        {
            // No route between our target and the source
            return;
        }

        currentPath = new List<Node>();

        Node currentNode = target;
        // Step through the "previous" chain and add it to the path.
        while (currentNode != null)
        {
            currentPath.Add(currentNode);
            currentNode = previous[currentNode];
        }

        // Now currentPath describes a route from target to source, so we need to invert it.
        currentPath.Reverse();
        Debug.Log("Success!");
    }

    public void MoveNextNode()
    {
        if (currentPath == null)
            return;

        currentPath.RemoveAt(0);

        gameObject.transform.position = currentPath[0].worldPos;

        if (currentPath.Count == 1)
        {
            // We've reached our destination
            currentPath = null;
            hasTarget = false;
        }
    }

    Node ConvertToNode(float x, float y)
    {
        Node result = new Node();
        foreach(Node nodes in navGraph)
        {
            if (nodes.worldPos.x <= x + 2 && nodes.worldPos.x >= x - 2 && nodes.worldPos.y <= y + 2 && nodes.worldPos.y <= y - 2)
            {
                result = nodes;
            }
        }
        //Debug.Log(result.worldPos);
        return result;
    }
}

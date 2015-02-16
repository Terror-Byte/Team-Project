using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    Vector3 origin;
    public bool mouseEdge = false;
    int screenX;
    int screenY;
    float t = 0.0f;
    public int dist = 5;
    public int edge = 50;

	public GameObject player;
	public int playerXp = 0;

    GameObject[] gos;
    public int totalEnemies;

	// Use this for initialization
	void Start () 
    {
        origin = Camera.main.transform.position;

        screenX = Screen.width;
        screenY = Screen.height;

        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Application.loadedLevelName == "Scene1")
        {
			//if (player == null)
				//player = GameObject.Find ("Player");
            gos = GameObject.FindGameObjectsWithTag("Enemy");
            totalEnemies = gos.Length;
        }

        if (Application.loadedLevelName == "Select")
            origin = new Vector3(0, 0, -10);

        if (GameObject.Find("Player") != null)
        {
            origin = GameObject.Find("Player").transform.position;
            origin.z = -10.0f;
        }

        t = 2f;

        //move camera right
        if (Input.mousePosition.x > screenX - edge && Input.mousePosition.x < screenX + edge)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + Vector3.right * dist, t * Time.deltaTime);
        }
        else
        {
            Camera.main.transform.position = (Vector3.Lerp(Camera.main.transform.position, origin, t * Time.deltaTime));
        }

        //move camera left
        if (Input.mousePosition.x < 0 + edge && Input.mousePosition.x > 0 - edge)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + Vector3.left * dist, t * Time.deltaTime);
        }
        else
        {
            Camera.main.transform.position = (Vector3.Lerp(Camera.main.transform.position, origin, t * Time.deltaTime));
        }

        //move camera up
        if (Input.mousePosition.y > screenY - edge && Input.mousePosition.y < screenY + edge)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + Vector3.up * dist, t * Time.deltaTime);
        }
        else
        {
            Camera.main.transform.position = (Vector3.Lerp(Camera.main.transform.position, origin, t * Time.deltaTime));
        }

        //move camera down
        if (Input.mousePosition.y < 0 + edge && Input.mousePosition.y > 0 - edge)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + Vector3.down * dist, t * Time.deltaTime);
        }
        else
        {
            Camera.main.transform.position = (Vector3.Lerp(Camera.main.transform.position, origin, t * Time.deltaTime));
        }

        if (Application.loadedLevelName == ("Scene1") && totalEnemies == 0)
        {
            Application.LoadLevel("Select");
            //Selector maxDifficultyCompleted = leveldifficulty completed
        }

	}

    void EnemyDied()
    {
        totalEnemies--;
    }

	void SendXPToPlayer()
	{
		player.SendMessage ("LoadXPFromController", playerXp);
	}
}
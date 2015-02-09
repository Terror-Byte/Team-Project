using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public GameObject bulletPrefab;
	public float speed = 1;

	public float weaponSpd = 2.5f;
	public float weaponRefresh = 1.0f;
	float refreshCounter;

    Vector3 currVel;

	public int health = 100;

	// Use this for initialization
	void Start () 
	{
		refreshCounter = 0;
        StartCoroutine(CalcVelocity());
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (health <= 0)
			Destroy (this.gameObject);

        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //Debug.Log(move);

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
                newBullet.AddComponent("Rigidbody2D");
                newBullet.rigidbody2D.gravityScale = 0.0f;
                Physics2D.IgnoreCollision(collider2D, newBullet.collider2D);

                Vector3 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
                Vector3 mousePos = Input.mousePosition;
                Vector3 forceDirection = mousePos - playerPos;
				forceDirection.Normalize ();
                float angle = Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg;

                Vector3 a = forceDirection * weaponSpd;

                //Takes into account the players current direction so that the player can not overtake his own shots
                newBullet.rigidbody2D.velocity = a - (currVel/3);
                newBullet.transform.rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);

                //Debug.Log(currVel.magnitude);
                //Debug.Log(forceDirection);
                //Debug.Log(forceDirection - currVel.normalized * (weaponSpd + currVel.magnitude));

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
}

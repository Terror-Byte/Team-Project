using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {

    public Vector2 level = new Vector2(0,0);
    public bool isChanged = false;
    public float flashTime = 0.5f;
    float t = 1.0f;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        t -= Time.deltaTime;

        if (t <= 0.0f)
        {
            this.gameObject.renderer.enabled = !this.gameObject.renderer.enabled;
           
            t = flashTime;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Translate(new Vector2(0,2));
            level.y++;
            this.gameObject.renderer.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Translate(new Vector2(0, -2));
            level.y--;
            this.gameObject.renderer.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Translate(new Vector2(-2, 0));
            level.x--;
            this.gameObject.renderer.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Translate(new Vector2(2, 0));
            level.x++;
            this.gameObject.renderer.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
	}
}

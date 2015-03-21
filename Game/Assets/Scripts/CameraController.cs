using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Vector3 origin;
    Vector3 offset;
    public float t = 5.0f;
    public float speed = 0.3f;
    bool freeCamera = false;
    float scroll;
	// Use this for initialization
	void Start () 
    {
        origin = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        switch (Application.loadedLevelName)
        {
            case "Start":

                if (freeCamera)
                    origin = Camera.main.transform.position;
                else if (!freeCamera)
                    origin = new Vector3(0, 2.5f, -17.34f);

                offset = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2), 0.0f));
                Camera.main.transform.position = origin + offset * t;

                break;
            case "Select":
                origin = GameObject.FindGameObjectWithTag("Player").transform.position;
                origin.z = -15.0f;
                break;
            case "Scene1":
            case "ActionScene":

                if (freeCamera)
                    origin = Camera.main.transform.position;
                else if (!freeCamera)
                    origin = GameObject.FindGameObjectWithTag("Player").transform.position;


                origin.z = -15.0f;
                offset = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2), 0.0f));
                Camera.main.transform.position = origin + offset * t;
                break;
        }
        ZoomEnable();
        if (Input.GetKeyUp(KeyCode.Tab))
            freeCamera = !freeCamera;

	}

    void ZoomEnable()
    {
        scroll = Camera.main.orthographicSize;
        //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        //scroll -= Input.GetAxisRaw("Mouse ScrollWheel") * 100;
        if (Input.GetAxis("Mouse ScrollWheel") == 0.1f)
        {
            scroll = 3;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") == -0.1f)
        {
            scroll = 10;
        }

        //Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, scroll, speed * Time.deltaTime);
        
    }

    void FixedUpdate()
    {
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, scroll, speed);
    }
}

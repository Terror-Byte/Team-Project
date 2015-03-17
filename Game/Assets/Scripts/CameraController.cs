using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Vector3 origin;
    Vector3 offset;
    public float t = 2.0f;

    bool freeCamera = false;

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
                //Uncomment the line below for fun on the start screen (:
                //origin = Camera.main.transform.position;
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
                else
                    origin = GameObject.FindGameObjectWithTag("Player").transform.position;


                origin.z = -15.0f;
                offset = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2), 0.0f));
                Camera.main.transform.position = origin + offset * t;
                break;

        }

        if (Input.GetKeyUp(KeyCode.Tab))
            freeCamera = !freeCamera;

	}
}

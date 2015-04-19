using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Vector3 origin;
    Vector3 offset;
    public float t = 5.0f;
    public float time = 0.0f;
    public float speed = 0f;
    bool offsetOn = false;
    public bool lerping = false;
    public float lerpStart = 0;
    public float lerpEnd = 1;
    bool freeCamera = false;
    //public float scroll = 7;
	// Use this for initialization
	void Start () 
    {
        lerpStart = lerpEnd = Camera.main.orthographicSize;
        origin = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        switch (Application.loadedLevelName)
        {
            case "Start":
                if (freeCamera)
                {
                    origin = Camera.main.transform.position;
                    t = 2;
                }
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
                {
                    origin = Camera.main.transform.position;
                    t = 2;
                }
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
        //if (Input.GetAxis("Mouse ScrollWheel") == 0.1f)
        //{
        //    //time = 0;
        //    if (!lerping)
        //    {
        //        lerpStart = Camera.main.orthographicSize;
        //        lerpStart = 20;
        //        time = 0;
        //    }
        //    lerpEnd = Mathf.Clamp(Camera.main.orthographicSize - 1, 4, 20);

        //    speed = Mathf.Clamp(speed + 0.2f, 1, 4);
        //    lerping = true;
        //}
        //else if (Input.GetAxis("Mouse ScrollWheel") == -0.1f)
        //{
        //    //time = 0;
        //    if (!lerping) 
        //    { 
        //        lerpStart = Camera.main.orthographicSize;
        //        lerpStart = 4;
        //        time = 0;
        //    }
        //    lerpEnd = Mathf.Clamp(Camera.main.orthographicSize + 1, 4, 20);

        //    speed = Mathf.Clamp(speed + 0.2f, 1, 4);
        //    lerping = true;
        //}

        //Fuck linear interpolation
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - 10 * Input.GetAxis("Mouse ScrollWheel"), 4, 20);        
    }

    //void FixedUpdate()
    //{
    //    if (lerping)
    //        Camera.main.orthographicSize = Mathf.LerpAngle(lerpStart, lerpEnd, time);

    //    if (time >= 1)
    //    {
    //        lerping = false;
    //        speed = 1;
    //        time = 0;
    //    }
    //    if (!lerping)
    //        speed = 1;
    //    time += Time.deltaTime;
    //}
}

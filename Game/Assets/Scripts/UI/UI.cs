using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    int width = Screen.width;
    int height = Screen.height;
    public GUISkin customSkin;
    public Text gameName;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.skin = customSkin;

        GUI.Label(new Rect(width / 2, height / 2, 750, 60), "Plunder");

        if(GUI.Button(new Rect(width/2 - 250, height/2 - 30, 500, 60), "Start game"))
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }   
    }
}

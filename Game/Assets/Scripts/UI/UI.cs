using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    int width = Screen.width;
    int height = Screen.height;
    public float speed = 2;
	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
        width = Screen.width;
        height = Screen.height;

        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(0.6f, 0.6f, 0.0f), speed * Time.deltaTime);
    }
    
    void OnMouseOver()
    {
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(0.8f, 0.8f, 0.0f), speed * Time.deltaTime);
    }

    void OnMouseDown()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}

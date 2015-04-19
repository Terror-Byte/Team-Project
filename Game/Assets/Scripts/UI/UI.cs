using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {


    int width = Screen.width;
    int height = Screen.height;
    Vector3 scale;
    public Vector3 zoom = new Vector3(1.3f, 1.3f);
    public float speed = 2;

    // Use this for initialization
    void Start()
    {
        scale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        width = Screen.width;
        height = Screen.height;
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, scale, speed * Time.deltaTime);
    }

    void OnMouseOver()
    {
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, scale + zoom, speed * Time.deltaTime);
    }

    void OnMouseDown()
    {
        GameObject.Find("Sounds").audio.Play();

        Application.LoadLevel(Application.loadedLevel + 1);
    }

    
}

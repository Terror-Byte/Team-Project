using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    public int speed = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Rotate(0, 0, Time.deltaTime * speed);
	}
}

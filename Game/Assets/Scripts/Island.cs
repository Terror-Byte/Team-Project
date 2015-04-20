using UnityEngine;
using System.Collections;

public class Island : MonoBehaviour {

    public Sprite[] islands;
    public bool isPlayable = true;
    public Vector2 place;
	// Use this for initialization
	void Start () 
    {
        GetComponent<SpriteRenderer>().sprite = islands[Random.Range(0, islands.Length)];
        this.transform.localScale *= Random.Range(0.8f, 1.2f);
        place = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}

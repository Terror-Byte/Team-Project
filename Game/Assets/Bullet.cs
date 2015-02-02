using UnityEngine;
using System.Collections;

[System.Serializable]
public class Bullet {

	// GameObject to store the prefab that represents the bullet
	public GameObject visualPrefab = GameObject.Find ("BulletPrefab");
	public bool isInstantiated = false;

	// Constructors
	public Bullet()
	{

	}

	public Bullet(Vector2 position, Quaternion rotation)
	{

	}

	/*
	public GameObject VisualPrefab
	{
		get
		{
			return visualPrefab;
		}
		set
		{
			visualPrefab = value;
		}
	}
	*/
}

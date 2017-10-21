using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour {

	public Vector3 offset;
	public GameObject Player;

	// Use this for initialization
	void Start () {
		offset = transform.position - Player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 newPosition = Player.transform.position + offset; 
		transform.position = newPosition;
	}
}

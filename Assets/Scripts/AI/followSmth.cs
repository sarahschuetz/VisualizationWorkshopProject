using UnityEngine;
using System.Collections;

public class followSmth : MonoBehaviour {

	public Vector3 offset = new Vector3(0, 0, 0);
	
	// Distance the follower starts following
	public int followDistance = 0;
	
	// Object which the current object will followDistance
	public Transform target;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = target.position + offset;
	}
}
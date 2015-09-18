using UnityEngine;
using System.Collections;

public class followPlayer : MonoBehaviour {

	// How fast does the follower run after you
	public float speed = 10f;
	
	// How fast does the follower turns around
	public float rotationSpeed = 10f;
	
	// Distance the follower starts following
	public int followDistance = 100;
	
	// Transformation of player and follower
	private Transform player;
	private Transform follower;
	
	// What is the distance between you and your follower
	private float distance;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		this.player = GameObject.Find("player").transform;
		this.follower = GameObject.Find("bear").transform; // **** TODO:  DO NOT USE BEAR!!!
		
		this.distance = Vector3.Distance(player.position, follower.position);
		
		if(this.distance <= this.followDistance) {
			
			// rotate follower
			// Vector of position, in which bear should face
			Vector3 followerDirection = follower.position - player.position;
			
			float rotateStep = speed * Time.deltaTime;
			Vector3 newDirection = Vector3.RotateTowards(follower.forward, followerDirection, rotateStep, 0.0F);
			Debug.DrawRay(follower.position, newDirection, Color.red);
        	follower.rotation = Quaternion.LookRotation(newDirection);
			
			// move follower
			float step = speed * Time.deltaTime;
		    follower.position = Vector3.MoveTowards(follower.position, player.position, step);
		}
	
		
	}
}

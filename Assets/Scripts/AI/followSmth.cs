using UnityEngine;
using System.Collections;

public class followSmth : MonoBehaviour {

	// How fast does the follower run after you
	public float speed = 10f;
	
	// How fast does the follower turns around
	public float rotationSpeed = 10f;
	
	// Should the follower be at the same position
	public bool samePosition = false;
	
	public Vector3 offset = new Vector3(0, 0, 0);
	
	// Distance the follower starts following
	public int followDistance = 100;
	
	// Object which the current object will followDistance
	public Rigidbody target;
	
	// What is the distance between you and your follower
	private float distance;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if(this.samePosition) {
			
			transform.position = target.transform.position + offset;
		}
		else {
			this.distance = Vector3.Distance(target.transform.position, transform.position);
			
			if(this.distance <= this.followDistance) {
				
				// rotate follower
				Vector3 followerDirection = transform.position - target.transform.position;
				
				float rotateStep = speed * Time.deltaTime;
				
				Vector3 newDirection = Vector3.RotateTowards(transform.forward, followerDirection, rotateStep, 0.0F);
				Debug.DrawRay(transform.position, newDirection, Color.red);
				transform.rotation = Quaternion.LookRotation(newDirection);
				
				// move follower
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
			}
		}
	}
}
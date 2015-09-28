using UnityEngine;
using System.Collections;

public class followNavMesh : MonoBehaviour {
	
	// Object which should follow something
	NavMeshAgent agent;
	
	// Object which the current object will follow
	public Transform target;
	
	// Distance the follower starts following
	public int followDistance = 100;
	
	// What is the distance between you and your follower
	private float distance;
	
	
	

	// Use this for initialization
	void Start () {
		
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		
		this.distance = Vector3.Distance(target.position, transform.position);
		
		if(this.distance <= this.followDistance) {
			agent.SetDestination(target.position);
		}
	}
}

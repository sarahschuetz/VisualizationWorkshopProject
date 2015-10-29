using UnityEngine;
using UnityEngine.Events;
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

	// Is there already a message sent, that the object is currently following the target
	private bool followingMessageSent;

	private bool following;
	


	// Use this for initialization
	void Start () {
		
		this.agent = GetComponent<NavMeshAgent>();
		this.followingMessageSent = false;
		this.following = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		this.distance = Vector3.Distance(target.position, transform.position);

		// Debug.Log(this.distance);
		
		if(this.distance <= this.followDistance) {

			if(!this.followingMessageSent) {
				this.followingMessageSent = true;
				EventManager.triggerEvent("startFollowing");
			} else {

				if(this.following) {

					Debug.Log("START MOVING");
					agent.Resume();
					agent.SetDestination(target.position);
				}
			}
		}
	}

	// EventManager
	private UnityAction listener; 
	
	void Awake() {
		this.listener = new UnityAction(startMoving);
	}
	
	void OnEnable() {
		EventManager.startListening("startFollowMoving", this.listener);
	}
	
	void OnDisable() {
		EventManager.stopListening("startFollowMoving", this.listener);
	}
	
	void startMoving() {
		this.following = true;;
	}
}

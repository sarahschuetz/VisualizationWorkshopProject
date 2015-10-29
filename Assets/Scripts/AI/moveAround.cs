using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class moveAround : MonoBehaviour {

	public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;

	// Use this for initialization
	void Start () {

		agent = GetComponent<NavMeshAgent>();

		// Disabling auto-braking allows for continuous movement
		// between points (ie, the agent doesn't slow down as it
		// approaches a destination point).
		agent.autoBraking = true;
	}
	
	// Update is called once per frame
	void Update () {
	
		// Choose the next destination point when the agent gets
		// close to the current one.
		// if there is a Path set yet
		if(agent.hasPath) {
			if (agent.remainingDistance < 0.5f) {
				GotoNextPoint();
			}
		}
	}

	void GotoNextPoint() {
		// Returns if no points have been set up
		if (points.Length == 0) {
			return;
		}
		
		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;
		
		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		destPoint = (destPoint + 1) % points.Length;
	}

	// EventManager
	private UnityAction startListener;
	private UnityAction stopListener;
	
	void Awake() {
		this.startListener = new UnityAction(startMoving);
		this.stopListener = new UnityAction(stopMoving);
	}
	
	void OnEnable() {
		EventManager.startListening("startMoving", this.startListener);
		EventManager.startListening("stopMoving", this.stopListener);
	}
	
	void OnDisable() {
		EventManager.stopListening("startMoving", this.startListener);
		EventManager.stopListening("stopMoving", this.stopListener);
	}

	void startMoving() {
		if(!agent.hasPath) {
			GotoNextPoint();
		} else {
			agent.Resume();
		}
	}
	
	void stopMoving() {
		agent.Stop();
	}
}

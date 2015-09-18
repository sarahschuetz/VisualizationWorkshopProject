using UnityEngine;
using System.Collections;

public class hitPlayer : MonoBehaviour {
	
	public Rigidbody player;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision hit) {
		if (hit.gameObject.name == player.name){
			print("YOU'R DONE");
		}
	}
}

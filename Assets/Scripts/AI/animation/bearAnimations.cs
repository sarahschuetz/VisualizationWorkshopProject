using UnityEngine;
using System.Collections;

public class bearAnimations : MonoBehaviour {

	private Animator animator;

	void Awake() {
		animator = gameObject.GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider other) {

		if(other.gameObject.tag == "Player") {
			animator.SetInteger("animationCount", 6);
			Debug.Log("KILL");
		}
	}
}

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class randomAnimations : StateMachineBehaviour {

	[System.Serializable]
	public class animationInfo {
		
		public string name;
		public AnimationClip clip;
		public float likelyhood;
	}
	
	// all animation which should be switched randomly
	public animationInfo[] randomAnimationsArray;

	private Animator animator;
	private GameObject gameObject;
	private bool following = false; 
	private bool firstAnimation = true;
	private int random;

	// used in Unity to compare Animations
	// bear
	// 0 = walk   1 = breath   2 = smell   3 = look   4 = roar   5 = run  7 = Exit
	private int animationCount = 1;




    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
		if(firstAnimation) {
			this.animator = animator;

			this.gameObject = animator.gameObject;
			
			// correct likelyhood of animations if they sum up to over 100% and set the right animation ranges
			this.corrAnimationLikelyhood();
			this.setAnimationRanges();
			this.firstAnimation = false;
		}
	}

	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		if(!this.following) {

			this.nextAnimation();

		} else { // only important for bear

			EventManager.triggerEvent("stopMoving");

			if(animationCount < 4) {

				this.animationCount = 4;
				this.animator.SetInteger("animationCount", 4);

			} else if(this.animationCount == 6){

				// assign value to animator
				this.animator.SetInteger("animationCount", 6);

			} else { 
				
				// assign value to animator
				this.animator.SetInteger("animationCount", 5);
				
				Debug.Log("RUN!!!!");
				EventManager.triggerEvent("startFollowMoving");
			}
		}
	}

	// OnStateMachineEnter is called when entering a statemachine via its Entry Node
	//	override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash) {
	//
	//		this.animator = animator;
	//		Debug.Log ("ANIMATOR ASIGNED");
	//		
	//		// correct likelyhood of animations if they sum up to over 100% and set the right animation ranges
	//		this.corrAnimationLikelyhood();
	//		this.setAnimationRanges();
	//	}

	// OnStateMachineExit is called when exiting a statemachine via its Exit Node
	//override public void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
	//
	//}
	
	private void nextAnimation() {
		
		// create a random number between 0 and 100%
		this.random = (int) Random.Range(0.0F, 100.0F);
		
		// calculate animation count
		this.setAnimationCount();
		
		// assign value to animator
		animator.SetInteger("animationCount", animationCount);
	}
	
	/// <summary>
	/// Corrects the animation likelyhood if the likelyhoods sum up to more then 100%.
	/// </summary>
	private void corrAnimationLikelyhood() {
		
		float sum = 0;
		
		// calculate sum
		foreach(animationInfo info in this.randomAnimationsArray) {
			sum += info.likelyhood;
		}
		
		// calculate percentages
		// int-values
		foreach(animationInfo info in this.randomAnimationsArray) {
			info.likelyhood = info.likelyhood * 100 / sum;
		} 
	}
	
	/// <summary>
	/// Sets the animation ranges.
	/// </summary>
	private void setAnimationRanges() {
		
		float currentHighestValue = 0;
		
		// calculate range of animations
		foreach(animationInfo info in this.randomAnimationsArray) {
			info.likelyhood += currentHighestValue;
			currentHighestValue = info.likelyhood;
		}
	}
	
	/// <summary>
	/// Calculates the animation count.
	/// </summary>
	private void setAnimationCount() {
		
		this.animationCount = 0;
		
		for(int i = 0; i < this.randomAnimationsArray.Length; i++) {
			
			if(this.random > this.randomAnimationsArray[i].likelyhood) {
				this.animationCount = i + 1;
			}
		}
	}
	
	// EventManager -- just for bear
	private UnityAction listener; 
	private UnityAction killListener; 
	
	void Awake() {
		this.listener = new UnityAction(startFollowing);
		this.killListener = new UnityAction(jumpAndKill);
	}
	
	void OnEnable() {
		EventManager.startListening("startFollowing", this.listener);
		EventManager.startListening("jumpAndKill", this.killListener);
	}
	
	void OnDisable() {
		EventManager.stopListening("startFollowing", this.listener);
		EventManager.stopListening("jumpAndKill", this.killListener);
	}
	
	void startFollowing() {

		if(this.gameObject.tag == "bear") {
			this.following = true;
		}
	}

	void jumpAndKill() {
		
		if(this.gameObject.tag == "bear") {
			Debug.Log("Start KILLing");
			this.animationCount = 6;
		}
	}
}

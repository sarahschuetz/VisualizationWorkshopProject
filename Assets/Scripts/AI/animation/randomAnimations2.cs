using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class randomAnimations2 : MonoBehaviour {

	[System.Serializable]
	public class animationInfo {

		public string name;
		public AnimationClip clip;
		public float likelyhood;
	}

	// all animation which should be switched randomly
	public animationInfo[] randomAnimationsArray;

	private Animator animator;
	private bool following = false; 
	private int random;

	// used in Unity to compare Animations
	// bear
	// 0 = walk   1 = breath   2 = smell   3 = look   4 = roar   5 = run  7 = Exit
	private int animationCount = 1;
	private float waitTime = 0;

	// Use this for initialization
	IEnumerator Start () {

		// correct likelyhood of animations if they sum up to over 100% and set the right animation ranges
		this.corrAnimationLikelyhood();
		this.setAnimationRanges();

		// start animation loop
		yield return StartCoroutine(animationLoop());
	}
	
	/// <summary>
	/// Loops the animation.
	/// Waits until the current animation is finished and then starts next animation.
	/// </summary>
	private IEnumerator animationLoop() {

		yield return new WaitForEndOfFrame();
		this.setWaitTime();

		yield return new WaitForSeconds (this.waitTime);
		this.nextAnimation();

		// continue animation loop (if bear is not following player)
		if(!this.following) {
			yield return StartCoroutine(animationLoop());

		} else {
			yield return StartCoroutine(followAnimation());
		}
	}

	private IEnumerator followAnimation() {

		yield return new WaitForEndOfFrame();
		this.setWaitTime();

		yield return new WaitForSeconds (this.waitTime);
		// assign value to animator
		animator.SetInteger("animationCount", 4);

		yield return new WaitForEndOfFrame();
		this.setWaitTime();
		
		yield return new WaitForSeconds (this.waitTime);
		// assign value to animator
		animator.SetInteger("animationCount", 5);
		EventManager.triggerEvent("startFollowing");
	}

	private void nextAnimation() {

		// create a random number between 0 and 100%
		this.random = (int) Random.Range(0.0F, 100.0F);

		// calculate animation count
		this.setAnimationCount();

		// assign value to animator
		animator.SetInteger("animationCount", animationCount);
		Debug.Log(animationCount);
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

	/// <summary>
	/// Calculates the animation time and sets the time for waitTime
	/// </summary>
	private void setWaitTime() {

		AnimatorClipInfo[] animationClips = animator.GetCurrentAnimatorClipInfo(0);
		AnimationClip animationClip = animationClips[0].clip;
		this.waitTime = animationClip.length;
	}

	// EventManager
	private UnityAction listener; 
	
	void Awake() {
		animator = gameObject.GetComponent<Animator>();
		this.listener = new UnityAction(startFollowing);
	}
	
	void OnEnable() {
		EventManager.startListening("startFollowing", this.listener);
	}
	
	void OnDisable() {
		EventManager.stopListening("startFollowing", this.listener);
	}
	
	void startFollowing() {
		this.following = true;
	}
}

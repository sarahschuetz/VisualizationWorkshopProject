using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class handleBearAnimations : MonoBehaviour {

	private Animator bearAnimator;

	private bool following = false; 

	// how likely is a particular animation (between 0 and 20)
	// when started the values of the highest possible percentage is stored here
	public int walk = 5;
	public int breath = 5;
	public int smell = 5;
	public int look = 5;
	public int roar = 5;

	private int random;

	// used in Unity to compare Animations
	// 0 = walk
	// 1 = breath
	// 2 = smell
	// 3 = look
	// 4 = roar
	// 5 = run
	// 6 = jump & kill
	// 7 = Exit
	private int animationCount = 1;
	private float waitTime = 0;

	// Use this for initialization
	IEnumerator Start () {

		bearAnimator = gameObject.GetComponent<Animator>();

		// correct likelyhood of animations if they sum up to over 100% and set the right animation ranges
		this.corrAnimationLikelyhood();
		this.setAnimationRanges();

		// start animation loop
		yield return StartCoroutine(animationLoop());
	}
	
	// Update is called once per frame
	void Update () {

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
			yield return StartCoroutine(animationLoop());;
		} else {
			yield return StartCoroutine(followAnimation());
		}
	}

	private IEnumerator followAnimation() {

		yield return new WaitForEndOfFrame();
		this.setWaitTime();

		yield return new WaitForSeconds (this.waitTime);
		// assign value to animator
		bearAnimator.SetInteger("animationCount", 4);

		yield return new WaitForEndOfFrame();
		this.setWaitTime();
		
		yield return new WaitForSeconds (this.waitTime);
		// assign value to animator
		bearAnimator.SetInteger("animationCount", 5);
		EventManager.triggerEvent("startMoving");
	}

	private void nextAnimation() {

		// create a random number between 0 and 100%
		this.random = (int) Random.Range(0.0F, 100.0F);

		// calculate animation count
		this.setAnimationCount();

		// assign value to animator
		bearAnimator.SetInteger("animationCount", animationCount);
	}

	/// <summary>
	/// Corrects the animation likelyhood if the likelyhoods sum up to more then 100%.
	/// </summary>
	private void corrAnimationLikelyhood() {

		int sum = this.walk + this.breath + this.smell + this.look + this.roar;

		// calculate percentages
		// int-values
		this.breath = this.breath * 100 / sum; 
		this.smell = this.smell * 100 / sum; 
		this.look = this.look * 100 / sum; 
		this.roar = this.roar * 100 / sum; 
	}

	/// <summary>
	/// Sets the animation ranges.
	/// </summary>
	private void setAnimationRanges() {
		
		// calculate range of animations
		this.smell = this.breath + this.smell;
		this.look = this.smell + this.look;
		this.roar = this.look + this.roar;
	}

	/// <summary>
	/// Calculates the animation count.
	/// </summary>
	private void setAnimationCount() {

		if(this.random <= this.breath) {
			this.animationCount = 1;
		} else if(this.random <= this.smell) {
			this.animationCount = 2;
		} else if(this.random <= this.look) {
			this.animationCount = 3;
		} else if(this.random <= this.roar) {
			this.animationCount = 4;
		} else {
			this.animationCount = 0;
		}
	}

	/// <summary>
	/// Calculates the animation time and sets the time for waitTime
	/// </summary>
	private void setWaitTime() {

		AnimatorClipInfo[] animationClips = bearAnimator.GetCurrentAnimatorClipInfo(0);
		AnimationClip animationClip = animationClips[0].clip;
		this.waitTime = animationClip.length;
	}

	// EventManager
	private UnityAction listener; 
	
	void Awake() {
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
		this.animationCount = 5;
	}
}

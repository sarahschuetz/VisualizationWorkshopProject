using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public string[] audioName;
	public AudioClip[] audioClip;

	private bool clipFound;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void play(string clipName) {
		for(int i = 0; i < audioName.Length; i++) {

			if(clipName == audioName[0]) { 
				//gameObject.audio. = audioClip[i];
			}
		}
	}
}

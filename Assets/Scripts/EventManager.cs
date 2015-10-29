using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager: MonoBehaviour {

	private Dictionary<string, UnityEvent> eventDictionary;

	private static EventManager eventManager;

	private static EventManager instance {
		get {
			if(!eventManager) {
				eventManager = FindObjectOfType(typeof (EventManager)) as EventManager;

				if(!eventManager) {
					// write Debug error message
					Debug.Log("There needs to be one active EventManager script on a GameObject in your scene.");
				} else {
					// initialize Eventmanager
					eventManager.init();
				}
			}

			return eventManager;
		}
	}

	void init() {
		if(this.eventDictionary == null) {
			this.eventDictionary = new Dictionary<string, UnityEvent>();
		}
	}

	public static void startListening(string eventName, UnityAction listener) {

		UnityEvent thisEvent = null; 

		if(instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			instance.eventDictionary.Add(eventName, thisEvent); 
		}
	} 

	public static void stopListening(string eventName, UnityAction listener) {

		if(eventManager == null) {
			return;
		}

		UnityEvent thisEvent = null; 

		if(instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}

	public static void triggerEvent(string eventName) {

		UnityEvent thisEvent = null; 

		if(instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.Invoke();
		}
	}
}

using UnityEngine;
using System.Collections;

public class showText : MonoBehaviour {
	
	public string[] messages;
	
	// reference of textPrefab
	public GameObject textPrefab;

	// distance to show hints
	public float rayLength = 10;

	// reference to the camera
	private Camera cam;

	// array with all GameObjects(messages)
	private GameObject[] allMessages;

	// Use this for initialization
	void Start () {

		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();

		allMessages = new GameObject[messages.Length];
		int count = 0;
		
		foreach (var text in messages)
		{
			GameObject g = Instantiate(textPrefab, transform.position, Quaternion.identity) as GameObject;	
			TextMesh t = g.GetComponent<TextMesh>();
			t.text = text;
			g.transform.parent = this.transform;
			allMessages[count] = g;
			count++;
			g.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {

		checkVisibility();

	}

	/// <summary>
	/// Checks the visibility using raycast.
	/// </summary>
	void checkVisibility() {

		Vector3 fwd = cam.transform.forward;
		RaycastHit hit;

		Debug.DrawLine(cam.transform.position, cam.transform.position + fwd * rayLength, Color.green);
		
		if (Physics.Raycast(cam.transform.position, fwd, out hit, rayLength)) {
			if(hit.transform.tag == "showHints") {

				Debug.DrawLine(cam.transform.position, cam.transform.position + fwd * rayLength, Color.red);

				foreach (var text in allMessages) {
					text.SetActive(true);
				}
			}

		} else {
			foreach (var text in allMessages) {
				text.SetActive(false);
			}
		}
	}
}

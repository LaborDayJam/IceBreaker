using UnityEngine;
using System.Collections;

public class escapeAndCursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Application.loadedLevelName == "GameCollect_SplitScreen") {

			Screen.showCursor = false;

		}
		else {
			Screen.showCursor = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}
}

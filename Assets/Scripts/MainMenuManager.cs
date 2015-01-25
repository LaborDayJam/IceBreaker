using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {


	public GameObject screen;

	// Use this for initialization
	void Start () {
		if (Random.Range (0, 6) > 1f) {
			screen.SetActive(false);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loadGame(){
		Application.LoadLevel ("GameCollect_SplitScreen");
	}

	public void loadCredits(){
		Application.LoadLevel ("creditsScreen");
	}

	public void loadMainMenu(){
		Application.LoadLevel ("mainMenu");
	}
}

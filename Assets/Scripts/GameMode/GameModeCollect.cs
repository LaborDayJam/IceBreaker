using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameModeCollect : GameMode {

	//Point Goal
	int winningScoreGoal = 5;

	private static GameModeCollect instance;
	public static  GameModeCollect Instance { get { return instance; } }

	public bool DEBUG = true;

	protected override void Awake()
	{
		if (instance == null) {
			base.Awake();
			instance = this;
		} else{
			Debug.Log("WARNING: DUPLICATE GAME MODE OBJECT. DESTROYING SELF");
			Destroy(gameObject);
		}
	}

	void Start()
	{
		StartGame ();
	}

	protected override void GameOver()
	{
		if (GetTeamOneScore () > GetTeamTwoScore ()) {
			Debug.Log ("Team Igloo Wins"); 
			iglooWinScreen.SetActive (true);
		} else {
			Debug.Log ("Team Icebreaker Wins");
			iceBreakerWinScreen.SetActive (true);
		}
		StartCoroutine (CR_RestartToMenu ());
	}

	public int GetTeamOneScore()
	{
		int teamOnePoints = 0;
		foreach (Player player in teamIgloo) {
			teamOnePoints += player.totalPoints;
		}
		return teamOnePoints;
	}

	public int GetTeamTwoScore()
	{
		int teamTwoPoints = 0;
		foreach (Player player in teamIcebreaker) {
			teamTwoPoints += player.totalPoints;
		}
		return teamTwoPoints;
	}

	protected override IEnumerator CR_GameLogicLoop()
	{
		while (GetTeamOneScore() < winningScoreGoal && GetTeamTwoScore() < winningScoreGoal) {
			Debug.Log("Igloo : " + GetTeamOneScore() + " | Icebreaker" + GetTeamTwoScore());
			yield return new WaitForSeconds(3.0f); //check every 3 seconds
		}
		GameOver();
	}

	public override void PlayerFell(Player player)
	{
		Debug.Log(player.name + " fell ");
		StartCoroutine (CR_Respawn (player));
	}
	
	IEnumerator CR_Respawn(Player player)
	{
		Camera playerCam = player.GetComponentInChildren<Camera> ();
		playerCam.enabled = false;

		yield return new WaitForSeconds (6);
		playerCam.enabled = true;
		player.transform.position = level.FindRespawnPosition ();
	}

}

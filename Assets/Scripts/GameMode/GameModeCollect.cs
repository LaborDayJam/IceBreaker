using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameModeCollect : GameMode {


	//Time Limit
	float timeLimit = 120; //seconds
	float timeLeft;

	//Point Goal
	public int winningScoreGoal = 5;

	private static GameModeCollect instance;
	public static  GameModeCollect Instance { get { return instance; } }

	void Awake()
	{
		if(instance == null)
			instance = this;
		else{
			Debug.Log("WARNING: DUPLICATE GAME MODE OBJECT. DESTROYING SELF");
			Destroy(gameObject);
		}
	}



	protected override void StartGame ()
	{
		base.StartGame ();
		StartCoroutine(CR_GameLogicLoop ());
	}

	protected override void GameOver()
	{
		print ("doing gameover stuff");
		if (GetTeamOneScore() > GetTeamTwoScore()) {
			Debug.Log ("Team Igloo Wins"); 
		} else
			Debug.Log ("Team Icebreaker Wins");
	}

	public int GetTeamOneScore()
	{
		int teamOnePoints = 0;
		print ("team size is " + teamIgloo.Count);
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
		/*
		while (timeLeft > 0) {
			timeLeft -= Time.deltaTime;
			yield return 0;
		}
		*/
		
		while (GetTeamOneScore() < winningScoreGoal && GetTeamTwoScore() < winningScoreGoal) {
			Debug.Log("Igloo : " + GetTeamOneScore() + " | Icebreaker" + GetTeamTwoScore() + " winning score " + winningScoreGoal);
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

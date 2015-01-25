﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameModeCollect : GameMode {


	//Time Limit
	float timeLimit = 120; //seconds
	float timeLeft;

	//Point Goal
	int winningScoreGoal = 10;

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



	protected override void GameOver()
	{
		if (GetTeamOneScore() > GetTeamTwoScore()) {
			Debug.Log ("Team Igloo Wins"); 
		} else
			Debug.Log ("Team Icebreaker Wins");
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
		/*
		while (timeLeft > 0) {
			timeLeft -= Time.deltaTime;
			yield return 0;
		}
		*/
		
		while (GetTeamOneScore() < winningScoreGoal || GetTeamTwoScore() < winningScoreGoal) {
			Debug.Log("Igloo : " + GetTeamOneScore() + " | Icebreaker" + GetTeamTwoScore());
			yield return new WaitForSeconds(3.0f); //check every 3 seconds
		}
		GameOver();
	}

}
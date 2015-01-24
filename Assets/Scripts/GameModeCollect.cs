using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameModeCollect : GameMode {

	//Team Properties
	public const int TEAM_IGLOO_INDEX = 0;
	public const int TEAM_ICEBREAKER_INDEX = 1;
	List<Player> teamIgloo;
	List<Player> teamIcebreaker;

	//Time Limit
	float timeLimit = 120; //seconds
	float timeLeft;

	//Point Goal
	int winningScoreGoal = 10;

	//Map 
	public Level level;

	//Prefab
	public GameObject prefabPlayer;
	public GameObject prefabDummyPlayer; //for local player two

	private static GameModeCollect instance;
	public GameModeCollect Instance { get { return instance; } }

	void Awake()
	{
		if(instance == null)
			instance = this;
		else{
			Debug.Log("WARNING: DUPLICATE GAME MODE OBJECT. DESTROYING SELF");
			Destroy(gameObject);
		}
	}

	void Start () 
	{
		teamIgloo = new List<Player> ();
		teamIcebreaker = new List<Player> ();
		StartGame ();
	}

	void StartGame()
	{
		timeLeft = timeLimit;
		SpawnPlayers ();
		StartCoroutine (CR_GameLogicLoop ());
		StartCoroutine (CR_CheckWinCondition ());
		StartCoroutine (CR_CheckLoseCondition ());
	}

	void SpawnPlayers()
	{
		GameObject playerOne = Instantiate (prefabPlayer, new Vector3 (0, 18, 0), Quaternion.identity) as GameObject;
		addPlayer (0, playerOne.GetComponent<Player>());

		//Dont instantiate dummy player if 2 player local is supported
		// use regular player
		GameObject playerTwo = Instantiate (prefabDummyPlayer, new Vector3 (0, 18, 0), Quaternion.identity) as GameObject;
		addPlayer (1, playerTwo.GetComponent<Player>());
	}

	void addPlayer(int team, Player player)
	{
		if (team == TEAM_IGLOO_INDEX) {
			teamIgloo.Add(player);
		} else {
			teamIcebreaker.Add(player);
		}
	}

	void removePlayer(int team, Player player)
	{
		if (team == TEAM_IGLOO_INDEX) {
			teamIgloo.Remove(player);
		} else {
			teamIcebreaker.Remove(player);
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

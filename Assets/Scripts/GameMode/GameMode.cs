using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMode : MonoBehaviour {

	
	//Team Properties
	public const int TEAM_IGLOO_INDEX = 0;
	public const int TEAM_ICEBREAKER_INDEX = 1;
	protected List<Player> teamIgloo;
	protected List<Player> teamIcebreaker;

	//Map 
	public Level level;
	
	//Prefab
	public GameObject prefabPlayer;
	public GameObject prefabDummyPlayer; //for local player two

	protected void Start () 
	{
		teamIgloo = new List<Player> ();
		teamIcebreaker = new List<Player> ();
		StartGame ();
	}
	
	protected virtual void StartGame()
	{
		SpawnPlayers ();
		StartCoroutine (CR_GameLogicLoop ());
		StartCoroutine (CR_CheckWinCondition ());
		StartCoroutine (CR_CheckLoseCondition ());
	}
	
	protected void SpawnPlayers()
	{
		GameObject playerOne = Instantiate (prefabPlayer, new Vector3 (0, 18, 0), Quaternion.identity) as GameObject;
		addPlayer (0, playerOne.GetComponent<Player>());
		
		//Dont instantiate dummy player if 2 player local is supported
		// use regular player
		GameObject playerTwo = Instantiate (prefabDummyPlayer, new Vector3 (0, 18, 0), Quaternion.identity) as GameObject;
		addPlayer (1, playerTwo.GetComponent<Player>());
	}
	
	protected void addPlayer(int team, Player player)
	{
		if (team == TEAM_IGLOO_INDEX) {
			teamIgloo.Add(player);
		} else {
			teamIcebreaker.Add(player);
		}
	}
	
	protected void removePlayer(int team, Player player)
	{
		if (team == TEAM_IGLOO_INDEX) {
			teamIgloo.Remove(player);
		} else {
			teamIcebreaker.Remove(player);
		}
	}

	protected virtual IEnumerator CR_CheckWinCondition()
	{
		yield return 0;
	}

	protected virtual IEnumerator CR_CheckLoseCondition()
	{
		yield return 0;
	}

	protected virtual IEnumerator CR_GameLogicLoop()
	{
		yield return 0;
	}

	protected virtual void GameOver() { }
	
}

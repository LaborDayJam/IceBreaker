using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum NetworkType{DEVELOPMENT, SPLIT, MULTIPLAYER, SIZE};
public class GameMode : MonoBehaviour {

	public NetworkType networkType = NetworkType.DEVELOPMENT;

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
		playerOne.name = "playerOne";
		playerOne.GetComponent<Player>().inputType = Player_Input_Type.PC;

		addPlayer (0, playerOne.GetComponent<Player>());

		//Dont instantiate dummy player if 2 player local is supported
		// use regular player
		GameObject playerTwo;
		if (networkType == NetworkType.MULTIPLAYER)
			playerTwo = Instantiate (prefabPlayer, new Vector3 (0, 18, 0), Quaternion.identity) as GameObject;
		else
			playerTwo = Instantiate (prefabDummyPlayer, new Vector3 (0, 18, 0), Quaternion.identity) as GameObject;
			
		playerOne.GetComponent<Player> ().team = 0;
		playerTwo.name = "playerTwo";


		playerTwo.GetComponent<Player> ().team = 1;
		playerTwo.GetComponent<Player>().inputType = Player_Input_Type.GAMEPAD;

		addPlayer (1, playerTwo.GetComponent<Player>());

		if (networkType == NetworkType.SPLIT) {
			playerOne.GetComponentInChildren<Camera> ().rect = new Rect (0, 0, .5f, 1);
			playerTwo.GetComponentInChildren<Camera> ().rect = new Rect (.5f, 0, .5f, 1);
		}
	}
	
	protected void addPlayer(int team, Player player)
	{
		if (team == TEAM_IGLOO_INDEX) {
			teamIgloo.Add(player);
		} else 
		{
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

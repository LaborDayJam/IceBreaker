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

	public Player[] players;		//Assign if you don't want to instantiate players on runtime

	protected void Start () 
	{
		teamIgloo = new List<Player> ();
		teamIcebreaker = new List<Player> ();
		StartGame ();
	}
	
	protected virtual void StartGame()
	{
		level.Init ();

		if (players.Length > 1)
			BindPlayers ();
		else
			SpawnPlayers ();

		foreach (Player player in players)
			level.PlacePlayersAtEdge(player, player.team);
			
		StartCoroutine (CR_GameLogicLoop ());
		StartCoroutine (CR_CheckWinCondition ());
		StartCoroutine (CR_CheckLoseCondition ());
	}
	
	protected void SpawnPlayers()
	{
		GameObject playerOneObj = Instantiate (prefabPlayer, new Vector3 (0, 18, 0), Quaternion.identity) as GameObject;
		Player playerOne = playerOneObj.GetComponent<Player> ();
		playerOne.BindControls ();
		playerOne.name = "playerOne";
		playerOne.team = 0;
		playerOne.inputType = Player_Input_Type.GAMEPAD;

		GameObject playerTwoObj;
		Player playerTwo;
		if (networkType == NetworkType.MULTIPLAYER)
			playerTwoObj = Instantiate (prefabPlayer, new Vector3 (0, 18, 0), Quaternion.identity) as GameObject;
		else
			playerTwoObj = Instantiate (prefabDummyPlayer, new Vector3 (0, 18, 0), Quaternion.identity) as GameObject;
			
		playerTwo = playerTwoObj.GetComponent<Player> ();

		playerTwo.BindControls ();
		playerTwo.name = "playerTwo";
		playerTwo.team = 1;
		playerTwo.inputType = Player_Input_Type.GAMEPAD;

		if (networkType == NetworkType.SPLIT) {
			playerOne.GetComponentInChildren<Camera> ().rect = new Rect (0, 0, .5f, 1);
			playerTwo.GetComponentInChildren<Camera> ().rect = new Rect (.5f, 0, .5f, 1);
		}

		addPlayer (0, playerOne);
		addPlayer (1, playerTwo);

		players = new Player[]{playerOne, playerTwo};
	}

	void BindPlayers()
	{
		foreach (Player player in players) {
			player.BindControls();
			
			if (player.team == TEAM_IGLOO_INDEX) {
				teamIgloo.Add(player);
			}
			else {
				teamIcebreaker.Add(player);
			}
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

	public virtual void PlayerFell(Player player) { }
}

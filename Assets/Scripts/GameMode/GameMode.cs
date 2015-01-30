using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum NetworkType{DEVELOPMENT, SPLIT, MULTIPLAYER, SIZE};
public class GameMode : Photon.MonoBehaviour {

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

	public GameObject iceBreakerWinScreen;
	public GameObject iglooWinScreen;

	protected virtual void Awake () 
	{
		teamIgloo = new List<Player> ();
		teamIcebreaker = new List<Player> ();
	}
	
	protected virtual void StartGame()
	{
		level.Init ();

		if (players.Length > 1)
			BindPlayers ();
		else
			SpawnPlayers ();

		//foreach (Player player in players)
		//	level.PlacePlayersAtEdge(player, player.team);
			
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

	protected void BindPlayer(Player player)
	{
		player.BindControls();
		player.EnableInput();
		
		if (player.team == TEAM_IGLOO_INDEX) {
			teamIgloo.Add(player);
		}
		else {
			teamIcebreaker.Add(player);
		}
	}

	protected void BindPlayers()
	{
		Debug.Log ("Bind Players");
		foreach (Player player in players) {
			BindPlayer(player);
		}
		if (networkType == NetworkType.SPLIT) {
			players[0].GetComponentInChildren<Camera> ().rect = new Rect (0, 0, .5f, 1);
			players[1].GetComponentInChildren<Camera> ().rect = new Rect (.5f, 0, .5f, 1);
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

	protected IEnumerator CR_RestartToMenu()
	{
		yield return new WaitForSeconds (2); //wait 2 seconds before listening to input
		while (!Input.anyKey) {
			yield return 0;
		}

		if (Input.anyKey) {
			Application.LoadLevel(0);
		}
	}

	public virtual void PlayerFell(Player player) { }
}

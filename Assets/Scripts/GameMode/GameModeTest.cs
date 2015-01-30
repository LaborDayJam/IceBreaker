using UnityEngine;
using System.Collections;

public class GameModeTest : GameMode {

	PhotonPlayer myPlayer;
	// Use this for initialization
	void Start () 
	{
	}

	void Initialize(Player player)
	{
		//if (isInitialized)
		//	return;
		level.Init ();
		
		//level.PlacePlayersAtEdge(player, player.team);
		BindPlayer (player);
	
		//foreach (Player player in players)
		//	level.PlacePlayersAtEdge(player, player.team);
		
		StartCoroutine (CR_GameLogicLoop ());
		//StartCoroutine (CR_CheckWinCondition ());
		//StartCoroutine (CR_CheckLoseCondition ());
	}
	
	void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		int playerCount = PhotonNetwork.playerList.Length;
		Debug.Log("ASDAS " + playerCount);
		
		//if(playerCount > 1 && state == WAITING_FOR_PLAYERS)
		//	StartCoroutine (CR_StartRace ());
	}
	
	void OnJoinedRoom()
	{
		int playerCount = PhotonNetwork.playerList.Length;
		print ("Player Count " + playerCount);

		Player player = players [playerCount - 1];
		player.photonView.RequestOwnership ();
		player.cam.tag = "MainCamera";
		Initialize (player);
		
		myPlayer = PhotonNetwork.player;
		
		for(int i = 0; i < players.Length; i++)
		{
			Debug.Log(i);
			if(i != (playerCount - 1))
			{
				Debug.Log("Yea " + i);
				Camera cam = players[i].GetComponentInChildren<Camera>();
				players[i].DisableInput();
				cam.enabled = false;
				cam.tag = "";
				players[i].GetComponentInChildren<AudioListener>().enabled = false;
			}
		}	
	}

}

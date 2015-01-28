using UnityEngine;
using System.Collections;

public class NGameModeCollect : GameModeCollect {


	private static NGameModeCollect instance;
	public static  NGameModeCollect Instance { get { return instance; } }

	bool isInitialized = false;

	void Awake()
	{
		if (instance == null) {
			base.Awake();
			instance = this;
		} else{
			Debug.Log("WARNING: DUPLICATE GAME MODE OBJECT. DESTROYING SELF");
			Destroy(gameObject);
		}
	}
	// Use this for initialization
	void Start () 
	{
	}

	void Initialize(Player player)
	{
		//if (isInitialized)
		//	return;
		
		level.Init ();

		level.PlacePlayersAtEdge(player, player.team);
		BindPlayer (player);
		/*
		if (players.Length > 1)
			BindPlayers ();
		else
			SpawnPlayers ();
		*/
		//foreach (Player player in players)
		//	level.PlacePlayersAtEdge(player, player.team);
		
		StartCoroutine (CR_GameLogicLoop ());
		//StartCoroutine (CR_CheckWinCondition ());
		//StartCoroutine (CR_CheckLoseCondition ());
		isInitialized = true;
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
		
		//if(playerCount == 1)
		Initialize (players[playerCount - 1]);

		for(int i = 0; i < players.Length; i++)
		{
			if(i == (playerCount - 1))
				players[i].EnableInput();
			else{
				players[i].DisableInput();
				players[i].GetComponentInChildren<Camera>().enabled = false;
				players[i].GetComponentInChildren<AudioListener>().enabled = false;
			}
		}	

	}

}

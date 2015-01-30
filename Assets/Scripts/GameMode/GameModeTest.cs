using UnityEngine;
using System.Collections;

public class GameModeTest : GameMode {

	public GameObject prefabPlayerOne;
	
	public GameObject prefabPlayerTwo;
	PhotonPlayer myPlayer;
	// Use this for initialization
	void Start () 
	{
		ScenePhotonView = this.GetComponent<PhotonView>();
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
		Debug.Log("OnPhotonPlayerConnected: " + newPlayer);
		
		// when new players join, we send "who's it" to let them know
		// only one player will do this: the "master"
		
		if (PhotonNetwork.isMasterClient)
		{
			TagPlayer(playerWhoIsIt);
		}
	}
	void OnJoinedRoom()
	{
		playerWhoIsIt = PhotonNetwork.player.ID;
		
		Debug.Log("playerWhoIsIt: " + playerWhoIsIt);
		MakePlayer ();
	}

	void MakePlayer()
	{
		int playerCount = PhotonNetwork.playerList.Length;
		print ("Player Count " + playerCount);

		GameObject playerObj = PhotonNetwork.Instantiate ("PlayerOne", new Vector3 (0, 10, 0), Quaternion.identity, 0);
		Player player = playerObj.GetComponent<NPlayer>();
		if (PhotonNetwork.player.isLocal) {
			player.photonView.RequestOwnership ();
			player.EnableInput();
			Debug.Log ("Getting Ownership");
		} else {
			Camera cam = player.GetComponentInChildren<Camera>();
			player.DisableInput();
			cam.enabled = false;
			cam.tag = "";
			player.GetComponentInChildren<AudioListener>().enabled = false;
			cam.GetComponent<MouseLook>().enabled = false;
			player.GetComponent<MouseLook>().enabled = false;
		}
		player.cam.tag = "MainCamera";
		Initialize (player);
		
		myPlayer = PhotonNetwork.player;
	}


	public static int playerWhoIsIt = 0;
	private static PhotonView ScenePhotonView;

	public static void TagPlayer(int playerID)
	{
		Debug.Log("TagPlayer: " + playerID);
		ScenePhotonView.RPC("TaggedPlayer", PhotonTargets.All, playerID);
	}
	
	[RPC]
	public void TaggedPlayer(int playerID)
	{
		playerWhoIsIt = playerID;
		Debug.Log("TaggedPlayer: " + playerID);
	}
	
	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.Log("OnPhotonPlayerDisconnected: " + player);
		
		if (PhotonNetwork.isMasterClient)
		{
			if (player.ID == playerWhoIsIt)
			{
				// if the player who left was "it", the "master" is the new "it"
				TagPlayer(PhotonNetwork.player.ID);
			}
		}
	}
	
	public void OnMasterClientSwitched()
	{
		Debug.Log("OnMasterClientSwitched");
	}
}

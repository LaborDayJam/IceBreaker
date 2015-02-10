using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameModeTest : GameMode {

	public PhotonPlayer myPlayer;

	// Use this for initialization
	void Start () 
	{
		ScenePhotonView = this.GetComponent<PhotonView>();
	}

	void Initialize(Player player)
	{
		PhotonNetwork.SetMasterClient (PhotonNetwork.player);
		level.Init ();
		BindPlayer (player);
		StartCoroutine (CR_GameLogicLoop ());
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

		GameObject playerObj = PhotonNetwork.Instantiate ((playerWhoIsIt == PhotonNetwork.player.ID) ? "IBPlayer" : "IBPlayer", new Vector3 (0, 10, 0), Quaternion.identity, 0);
		NPlayer player = playerObj.GetComponent<NPlayer>();
		if (PhotonNetwork.player.isLocal) {
			//player.photonView.RequestOwnership ();
			player.EnableInput();
			Debug.Log ("Getting Ownership");
		} 
		player.camera.SetActive (true);
		player.GetComponent<MouseLook>().enabled = true;

		player.cam.tag = "MainCamera";

		player.Init (PhotonNetwork.player);

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

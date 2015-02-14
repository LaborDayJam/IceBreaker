using UnityEngine;
using System.Collections;

public class NPlayer : Player {

	public PhotonPlayer photonPlayer;
	public GameObject camera;
	public void Init(PhotonPlayer player)
	{
		photonPlayer = player;
		gameObject.name = "Player" + player.ID;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			//We own this player: send the others our data
			//stream.SendNext((int)controllerScript._characterState);
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else
		{
			//Network player, receive data
			//controllerScript._characterState = (CharacterState)(int)stream.ReceiveNext();
			transform.position = (Vector3)stream.ReceiveNext();
			transform.rotation = (Quaternion)stream.ReceiveNext();
		}
	}
	
	[RPC]
	public void NetworkOnHit(PhotonMessageInfo info)
	{
		//if (photonView.viewID == info.photonView) {

		//}
	}

	
	
	[RPC]
	void playScores(int amount)
	{

	}

	[RPC]
	void playerPickup(PhotonPlayer player, Collectable collectable)
	{
		if (player.ID == PhotonNetwork.player.ID) {
			carryPoints += collectable.value;
			Debug.Log("Player " + player.ID + " has picked up " + collectable.value + " coins");
		}
	}
}

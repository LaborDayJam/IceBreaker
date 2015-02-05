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
}

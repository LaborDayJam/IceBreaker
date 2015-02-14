using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public int value = 1;

	//TODO fall if no cube underneath
	void OnTriggerEnter(Collider other)
	{
		/*
		Player player = other.GetComponent<Player> ();
		if (player == null)
			return;

		player.CarryPoints (value);
		Debug.Log (other.name + " collected " + name); 
		Destroy (gameObject);
		*/
		Player player = other.GetComponent<Player> ();
		if (player == null)
			return;

		if (player.photonView.isMine) {	
			player.CarryPoints (value);
			player.photonView.RPC ("playerPickup", PhotonTargets.Others, PhotonNetwork.player, value);	
		} else {

		}
	}
}

using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Dropped " + other.name);
		Player player = other.GetComponent<Player> ();
		if (player == null)
			return;
		GameModeCombat.Instance.PlayerFell(player);
	}
}

using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Dropped " + other.name);
		Player player = other.GetComponent<Player> ();
		if (player == null)
			return;

		if (GameModeCombat.Instance != null)
			GameModeCombat.Instance.PlayerFell (player);
		else if (GameModeCollect.Instance != null)
			GameModeCollect.Instance.PlayerFell (player);
	}
}

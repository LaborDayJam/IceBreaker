using UnityEngine;
using System.Collections;

public class Collector : MonoBehaviour {

	//TODO fall if no cube underneath
	void OnTriggerEnter(Collider other)
	{
		Player player = other.GetComponent<Player> ();
		if (player == null)
			return;
		
		player.ScorePoints ();
	}
}

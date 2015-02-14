using UnityEngine;
using System.Collections;

public class AutoMapPCControls : MonoBehaviour {

	Player player;
	// Use this for initialization
	void Start () {
		player = GetComponent<Player> ();
		player.inputType = Player_Input_Type.PC;
		InputManager.Instance.AssignControls (player, player.inputType);
	}
}

using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public int connectedControllers = 2;
	int nextAvailableControllerIndex = 1;
	public Player[] players = new Player[2];

	private static InputManager instance;
	public static InputManager Instance {get { 
		if(instance == null)
		{
			GameObject newInstance = GameObject.CreatePrimitive(PrimitiveType.Cube);
			newInstance.renderer.enabled = false;
			instance = newInstance.AddComponent<InputManager>();
		}

		return instance;
		} 
	}

	bool isKeyboardTaken = false;
	// Use this for initialization
	void Awake () {
		//connectedControllers = Input.GetJoystickNames().Length;
	}



	public void AssignControls(Player player, Player_Input_Type type)
	{
		if(type == Player_Input_Type.PC && !isKeyboardTaken)
		{
			player.moveX = "Horizontal";
			player.moveY = "Vertical";
			player.lookX = "Mouse X";
			player.lookY = "Mouse Y";
			isKeyboardTaken = true;
		}
		else if(type == Player_Input_Type.GAMEPAD && nextAvailableControllerIndex <= connectedControllers)
		{
			player.moveX = "HorizontalLeft" + nextAvailableControllerIndex;
			player.moveY = "VerticalLeft"  + nextAvailableControllerIndex;
			player.lookX = "HorizontalRight" +nextAvailableControllerIndex;
			player.lookY = "VerticalRight" + nextAvailableControllerIndex;
			player.performAction = "RB" + nextAvailableControllerIndex;
			player.switchAction = "Y" + nextAvailableControllerIndex;
			if(nextAvailableControllerIndex == 1){
				player.GetComponentInChildren<Camera>().cullingMask =  (1 << LayerMask.NameToLayer("TransparentFX")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("PlayerTwo"));
				player.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = LayerMask.NameToLayer("PlayerOne");
				player.rayGunForPlayer.layer = LayerMask.NameToLayer("PlayerTwo");
				player.rayGunForOtherPlayer.layer = LayerMask.NameToLayer("PlayerOne");
				player.pickAxeForPlayer.layer = LayerMask.NameToLayer("PlayerTwo");
				player.pickAxeForOtherPlayer.layer = LayerMask.NameToLayer("PlayerOne");
				player.playerNum = 1;
				players[0] = player;
			}
			else {
				player.GetComponentInChildren<Camera>().cullingMask =  (1 << LayerMask.NameToLayer("TransparentFX")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("PlayerOne"));
				player.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = LayerMask.NameToLayer("PlayerTwo");
				player.rayGunForPlayer.layer = LayerMask.NameToLayer("PlayerOne");
				player.rayGunForOtherPlayer.layer = LayerMask.NameToLayer("PlayerTwo");
				player.pickAxeForPlayer.layer = LayerMask.NameToLayer("PlayerOne");
				player.pickAxeForOtherPlayer.layer = LayerMask.NameToLayer("PlayerTwo");
				player.playerNum =2 ;
				players[1] = player;
				print ("got here");
			}
			connectedControllers++;
			nextAvailableControllerIndex++;

		}
		else
		{
			Debug.Log("INPUT HAS NOT BEEN ASSIGNED.");
		}
	}
}

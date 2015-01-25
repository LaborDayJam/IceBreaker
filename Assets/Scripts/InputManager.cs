using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public int connectedControllers = 0;
	int nextAvailableControllerIndex = 1;

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
		connectedControllers = Input.GetJoystickNames().Length;
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
			player.moveX = "Horizontal" + nextAvailableControllerIndex;
			player.moveY = "Vertical"  + nextAvailableControllerIndex;
			player.lookX = "HorizontalRight" +nextAvailableControllerIndex;
			player.lookY = "VerticalRight" + nextAvailableControllerIndex;
			player.performAction = "RB" + nextAvailableControllerIndex;
			player.switchAction = "Y" + nextAvailableControllerIndex;
			connectedControllers++;
		}
		else
		{
			Debug.Log("INPUT HAS NOT BEEN ASSIGNED.");
		}
	}
}

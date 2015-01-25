using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Player_Input_Type{PC, GAMEPAD, SIZE};
public class Player : BaseObject {

	int STATE_NORMAL = 0;
	int STATE_STUNNED = 1;
	int state = 0; 	//0 normal

	float stunDuration = 1.5f;

	public Camera cam;

	//Actions
	public Action currentAction;
	HitAction hitAction;
	ShootAction shootAction;

	//Input
	public Player_Input_Type inputType;
	public string moveX;
	public string moveY;
	public string lookX;
	public string lookY;
	public string switchAction;
	public string performAction;

	//Team
	public int team;
	public int playerName;

	public int totalPoints;

	public float knockbackForce = 60;

	public string actionButton = "Fire1";
	public FPSWalkerEnhanced controller;
	public int playerNum;

	bool isPerformActioning = false;

	void Awake()
	{
		hitAction = gameObject.AddComponent<HitAction>();
		shootAction = gameObject.AddComponent<ShootAction> ();
		currentAction = hitAction;
		//Debug.Log (Input.GetJoystickNames().Length);
	}

void Start()
{
	InputManager.Instance.AssignControls(this, inputType);

	if(inputType == Player_Input_Type.PC)
	{
		cam.GetComponent<MouseLook>().enabled = false;
	}
	//Debug.Log (Input.GetJoystickNames().Length);
}
	// Update is called once per frame
	void Update () {
		if (GameModeCollect.Instance.networkType == NetworkType.SPLIT) {
			Debug.Log( controller.player);
			if (controller.player.team == 1) {
				actionButton = "RB1";
				playerNum = 1;
			} else {
				actionButton = "RB2";
				playerNum = 2;
			}
		}
		HandleInput ();
	}

	void HandleInput()
	{
		switch(inputType)
		{
			case Player_Input_Type.PC:
			{
				if(Input.GetKeyDown (KeyCode.Alpha1))
					currentAction = hitAction;

				 if (Input.GetKeyDown (KeyCode.Alpha2))
				 	currentAction = shootAction;

			 	if(Input.GetMouseButton(0))
				 	isPerformActioning = true;
				else
					isPerformActioning = false;
			}break;
			case Player_Input_Type.GAMEPAD:
			{
				if (Input.GetButtonDown(switchAction) && currentAction == shootAction) {
					currentAction = hitAction;
					print ("changing player " + playerNum + " to  hit");

				} else if (Input.GetButtonDown(switchAction) && currentAction == hitAction) {
					currentAction = shootAction;
					print ("changing player " + playerNum + " to  shooting");
				}
				if(Input.GetButtonDown(performAction) && currentAction == shootAction)
					isPerformActioning = true;
				else if(Input.GetButton(performAction)&& currentAction == hitAction){
					isPerformActioning = true;
				Debug.Log (this.name + " is hitting");
				
				}
				else{
					isPerformActioning = false;
				}
		
			}break;
			default:
			break;
		}

		if(!isPerformActioning)

			return;

		switch(currentAction.type)
		{
		case ACTION_TYPE.ONCE:
		{
			currentAction.Do();
		}break;
		case ACTION_TYPE.CONTINOUS:
		{
			currentAction.Do();
		}break;
		default:
			break;
		}
	}

	//For when we are hit by the laser ray
	IEnumerator CR_Stunned()
	{
		state = STATE_STUNNED;
		Component motor = gameObject.GetComponent ("CharacterMotor");
		motor.SendMessage ("DisableMotor");

		yield return new WaitForSeconds (stunDuration);
		state = STATE_NORMAL;

		motor.SendMessage ("EnableMotor");
	}

	public override void onHit (GameObject other, float damage)
	{
		Player attacker = other.GetComponent<Player> ();
		if (attacker.currentAction is HitAction) {
			Vector3 direction = (transform.position - other.transform.position).normalized;
			GetComponent<ImpactReceiver>().AddImpact(direction, knockbackForce);
		} else if (attacker.currentAction is ShootAction) {
			StartCoroutine (CR_Stunned ());
		}
		base.onHit (other, damage);
	}

	public void ScorePoints(int points)
	{
		totalPoints += points;
		Debug.Log (totalPoints);
	}

	void OnDestroy()
	{
		StopAllCoroutines ();
	}
}

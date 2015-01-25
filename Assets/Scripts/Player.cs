using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : BaseObject {

	int STATE_NORMAL = 0;
	int STATE_STUNNED = 1;
	int state = 0; 	//0 normal

	float stunDuration = 1.5f;

	//Actions
	public Action currentAction;
	HitAction hitAction;
	ShootAction shootAction;

	//Team
	public int team;
	public int playerName;

	public int totalPoints;

	public float knockbackForce = 60;

	public string actionButton = "Fire1";
	public FPSWalkerEnhanced controller;
	public int playerNum;


	void Awake()
	{
		hitAction = gameObject.AddComponent<HitAction>();
		shootAction = gameObject.AddComponent<ShootAction> ();
		currentAction = hitAction;
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
		Debug.Log (Input.GetJoystickNames().Length);
	}

	void HandleInput()
	{
		if ((Input.GetKeyDown (KeyCode.Alpha1) || (Input.GetButtonDown("Y"+playerNum)) && currentAction == shootAction) ) {
			currentAction = hitAction;
			print ("changing player " + playerNum + " to  hit");

		} else if ((Input.GetKeyDown (KeyCode.Alpha2) || (Input.GetButtonDown("Y"+playerNum)) && currentAction == hitAction)) {
			currentAction = shootAction;
			print ("changing player " + playerNum + " to  shooting");
		}

		switch(currentAction.type)
		{
		case ACTION_TYPE.ONCE:
		{
			if(Input.GetButtonDown(actionButton))
			{
				print ("doing action");
				currentAction.Do();
			}
		}break;
		case ACTION_TYPE.CONTINOUS:
		{
			if(Input.GetButtonDown(actionButton))
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

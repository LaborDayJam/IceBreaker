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

	void Awake()
	{
		hitAction = gameObject.AddComponent<HitAction>();
		shootAction = gameObject.AddComponent<ShootAction> ();
		currentAction = hitAction;
	}

	// Update is called once per frame
	void Update () {
		HandleInput ();
	}

	void HandleInput()
	{
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentAction = hitAction;
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			currentAction = shootAction;
		}

		switch(currentAction.type)
		{
		case ACTION_TYPE.ONCE:
		{
			if(Input.GetMouseButtonDown(0))
			{
				currentAction.Do();
			}
		}break;
		case ACTION_TYPE.CONTINOUS:
		{
			if(Input.GetMouseButton(0))
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

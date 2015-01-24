using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : BaseObject {

	//Actions
	public Action currentAction;
	HitAction hitAction;
	ShootAction shootAction;

	//Team
	public int team;
	public int playerName;

	public int totalPoints;

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

	public void ScorePoints(int points)
	{
		totalPoints += points;
		Debug.Log (totalPoints);
	}
}

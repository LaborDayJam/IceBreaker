using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : BaseObject {

	//List<Action> actions;

	public Action currentAction;

	HitAction hitAction;
	ShootAction shootAction;

	void Awake()
	{
		//actions = new List<Action> ();
		hitAction = gameObject.AddComponent<HitAction>();
		shootAction = gameObject.AddComponent<ShootAction> ();
		currentAction = hitAction;
		//actions.Add (currentAction);

	}

	// Use this for initialization
	void Start () {
	
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
}

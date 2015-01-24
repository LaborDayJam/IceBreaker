using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : BaseObject {

	List<Action> actions;

	public Action currentAction;

	void Awake()
	{
		actions = new List<Action> ();
		currentAction = gameObject.AddComponent<HitAction>();
		actions.Add (currentAction);

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

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

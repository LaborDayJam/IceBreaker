using UnityEngine;
using System.Collections;

public class HitAction : Action {

	public float damage = 100;

	void Awake()
	{
		isReady = true;
		type = ACTION_TYPE.CONTINOUS;
		rateOfFire = 0;
	}

	public override void Do()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, range)) {
			BaseObject target = hit.transform.GetComponent<BaseObject>();
			if(target.isTargetable)
			{
				target.onHit(gameObject, damage * Time.deltaTime);
				Debug.Log("Hitting " + target.name + " Health " + target.health);
			}
		}
	}
}

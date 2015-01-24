using UnityEngine;
using System.Collections;

public class ShootAction : Action {

	float damage;
	void Awake()
	{
		actionName = "Solar Gun";
		isReady = true;
		type = ACTION_TYPE.ONCE;
		rateOfFire = 1;
		range = 10f;
		damage = 100;
	}
	
	public override void Do()
	{
		if (!isReady)
			return;

		//Find & hit target
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, range)) {
			BaseObject target = hit.transform.GetComponent<BaseObject>();
			if(target != null && target.isTargetable)
			{
				target.onHit(gameObject, damage);
			}
		}

		//Start Cooldown
		isReady = false;
		StartCoroutine (CR_StartCooldown ());
	}
}

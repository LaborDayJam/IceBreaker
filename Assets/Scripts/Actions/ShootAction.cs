using UnityEngine;
using System.Collections;

public class ShootAction : Action {

	float damage;
	Camera cam;
	void Awake()
	{
		actionName = "Solar Gun";
		isReady = true;
		type = ACTION_TYPE.ONCE;
		rateOfFire = 1;
		range = 10f;
		damage = 100;
	}

	void Start()
	{
		cam = GetComponentInChildren<Camera> ();
	}
	
	public override void Do()
	{
		if (!isReady)
			return;

		//Find & hit target
		Ray ray;

		if(GetComponent<Player>().team == 1)
			ray = cam.ScreenPointToRay (new Vector2(cam.pixelRect.center.x, cam.pixelHeight * .5f));
		else
			ray = cam.ScreenPointToRay (new Vector2(cam.pixelWidth * .5f, cam.pixelHeight * .5f));
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

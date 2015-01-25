using UnityEngine;
using System.Collections;

public class HitAction : Action {

	public float damage = 100;
	Camera cam;
	void Awake()
	{
		actionName = "Pickaxe";
		isReady = true;
		type = ACTION_TYPE.CONTINOUS;
		rateOfFire = 0;
		range = 2.5f;
	}
	void Start()
	{
		cam = GetComponentInChildren<Camera> ();
	}
	public override void Do()
	{
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
				target.onHit(gameObject, damage * Time.deltaTime);
				//Debug.Log("Hitting " + target.name + " Health " + target.health);
			}
		}
	}
}

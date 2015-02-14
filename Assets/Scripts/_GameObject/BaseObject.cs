using UnityEngine;
using System.Collections;

public class BaseObject : Photon.MonoBehaviour {
	public bool isTargetable;
	public float health;

	public virtual void onHit(GameObject other, float damage)
	{
		health -= damage;
		if (health <= 0) {
			Destroy (gameObject);
		}
	}
}

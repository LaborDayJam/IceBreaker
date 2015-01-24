using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour {
	public bool isTargetable;
	public float health;

	public virtual void onHit(GameObject other, float damage)
	{
		health -= damage;
		if (health <= 0) {
			Debug.Log("I shoudl be destroyed");
		
			Destroy (gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour {
	public bool isTargetable;
	public float health;
	public GameObject icebreak;

	public virtual void onHit(GameObject other, float damage)
	{
		
		health -= damage;
		if (health <= 0) {
			Instantiate(icebreak, this.transform.position, Quaternion.identity);	
			Destroy (gameObject);
		}
	}
}

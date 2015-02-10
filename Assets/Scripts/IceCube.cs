using UnityEngine;
using System.Collections;

public class IceCube : BaseObject 
{
	public int index;

	public bool isOccupied = false;
	public Level map;

	public GameObject icebreak;
	// Use this for initialization
	void Awake () 
	{
		Color original = renderer.material.color;
		renderer.material.color = new Color (original.r, original.g, original.b, Random.Range (.6f, 1f));
	}

	public override void onHit(GameObject other, float damage)
	{
		health -= damage;
		if (health <= 0) {
			Instantiate(icebreak, this.transform.position, Quaternion.identity);
			map.DestroyCube(this);			//Destroy (gameObject);
		}
	}

	public void SetLevel(Level level) { map = level; }
}

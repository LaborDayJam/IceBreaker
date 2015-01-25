using UnityEngine;
using System.Collections;

public class IceCube : BaseObject 
{
	public int index;

	// Use this for initialization
	void Awake () 
	{
		Color original = renderer.material.color;
		renderer.material.color = new Color (original.r, original.g, original.b, Random.Range (.5f, 1f));
	}
}

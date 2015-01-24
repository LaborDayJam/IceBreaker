using UnityEngine;
using System.Collections;

public class IceCube : BaseObject 
{
	public int index;
	bool isBreakable = true;


	// Use this for initialization
	void Awake () 
	{
		Color original = renderer.material.color;
		renderer.material.color = new Color (original.r, original.g, original.b, Random.Range (.25f, 1f));
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}

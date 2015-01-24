using UnityEngine;
using System.Collections;

public class IceCube : MonoBehaviour 
{

	private GameObject iceContainer;
	// Use this for initialization
	void Awake () 
	{
		this.transform.parent = GameObject.FindGameObjectWithTag("Container").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}

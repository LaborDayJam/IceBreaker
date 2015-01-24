using UnityEngine;
using System.Collections;

public class MapCreator : MonoBehaviour 
{
	public GameObject iceblock;

	// Use this for initialization
	void Awake () 
	{
		MapCreationa();
	}

	void MapCreation()
	{
		for(int y = 0; y < 10; y++)
			for(int x = 0; x < 20; x++)
				for(int z = 0; z < 20; z++)
					 Instantiate(iceblock, new Vector3(x,y,z),Quaternion.identity);

			
	}
}

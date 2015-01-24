using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {

	public int width;
	public int height;
	public int depth;
	 
	List<IceCube> map;

	public GameObject prefabIceCube;

	// Use this for initialization
	void Start () 
	{
		map = new List<IceCube>();
		GenerateMap ();
	}

	void GenerateMap()
	{
		int index = 0;
		for (int z = 0; z < depth; z++) {
			for (int y = 0; y < height; y++){
				for (int x = 0; x < width; x++) {
					index = x + y * width + z * (width * height);
					GameObject iceCube = Instantiate(prefabIceCube, new Vector3(x, y, z), Quaternion.identity) as GameObject;
					iceCube.GetComponent<IceCube>().index = index;
					map.Add(iceCube.GetComponent<IceCube>());
					iceCube.transform.parent = transform;
				}
			}
		}
	}

	public IceCube getCubeAtIndex(int index)
	{
		return map[index];
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {

	public int width;
	public int height;
	public int depth;
	 
	List<IceCube> map;
	List<Collectable> collectables;

	public GameObject prefabIceCube;
	public GameObject[] prefabCollectables;
	public int max_collectables = 10;

	public GameObject prefabCart;
	public List<Collector> carts;
	public int cart_count = 3;


	// Use this for initialization
	void Start () 
	{
		map = new List<IceCube>();
		collectables = new List<Collectable> ();
		carts = new List<Collector>();
		GenerateMap ();
		SpawnCollectables();
		SpawnCarts ();
	}

	void GenerateMap()
	{
		float cubeSize = prefabIceCube.transform.localScale.x;
		Debug.Log (cubeSize);
		int index = 0;
		for (int z = 0; z < depth; z++) {
			for (int y = 0; y < height; y++){
				for (int x = 0; x < width; x++) {
					index = x + y * width + z * (width * height);
					GameObject iceCube = Instantiate(prefabIceCube, new Vector3(x * cubeSize, y * cubeSize, z * cubeSize), Quaternion.identity) as GameObject;
					iceCube.GetComponent<IceCube>().index = index;
					map.Add(iceCube.GetComponent<IceCube>());
					iceCube.transform.parent = transform;
				}
			}
		}
	}

	void SpawnCollectables()
	{
		for(int i = 0; i < max_collectables; i++)
		{
			GameObject collectablePrefab = prefabCollectables[Random.Range(0, prefabCollectables.Length)];
			Vector3 position = getCubeAtIndex(Random.Range(0, width * height * depth)).transform.position;
			GameObject collectable = Instantiate(collectablePrefab, position, Quaternion.identity) as GameObject;
			collectable.transform.parent = transform.parent;
			collectables.Add(collectable.GetComponent<Collectable>());
		}
	}

	void SpawnCarts()
	{
		for(int i = 0; i < cart_count; i++)
		{
			Vector3 position = getCubeAtIndex(Random.Range(0, width * height * depth)).transform.position;
			GameObject cart = Instantiate(prefabCart, position, Quaternion.identity) as GameObject;
			cart.transform.parent = transform.parent;
			carts.Add(cart.GetComponent<Collector>());
		}
	}

	public IceCube getCubeAtIndex(int index)
	{
		return map[index];
	}

	public int getCollectableCount()
	{
		return collectables.Count;
	}
}

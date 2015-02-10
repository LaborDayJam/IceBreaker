using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : Photon.MonoBehaviour {

	public int width;
	public int height;
	public int depth;
	 
	public List<IceCube> map;
	List<Collectable> collectables;

	public GameObject prefabIceCube;
	public GameObject[] prefabCollectables;
	public int max_collectables = 10;

	public GameObject prefabCart;
	public List<Collector> carts;
	public int cart_count = 3;

	// Use this for initialization
	public void Init () 
	{
		map = new List<IceCube>();
		collectables = new List<Collectable> ();
		carts = new List<Collector>();
		GenerateMap ();
		SpawnCollectables();
		SpawnCarts ();

		Debug.Log ("Map: Finished Spawn Everything");
	}

	void GenerateMap()
	{
		float cubeSize = prefabIceCube.transform.localScale.x;
		int index = 0;
		for (int z = 0; z < depth; z++) {
			for (int y = 0; y < height; y++){
				for (int x = 0; x < width; x++) {
					index = x + y * width + z * (width * height);
					GameObject iceCubeObj = PhotonNetwork.InstantiateSceneObject("IBIceCube", new Vector3(x * cubeSize, y * cubeSize, z * cubeSize), Quaternion.identity, Global.ICE_CUBE_INDEX, null) as GameObject;
					IceCube iceCube = iceCubeObj.GetComponent<IceCube>();
					iceCube.index = index;
					iceCube.map = this;
					map.Add(iceCube);
					iceCubeObj.transform.parent = transform;
					iceCubeObj.name = x + "_" + y + "_" + z;
					iceCube.SetLevel(this);
				}
			}
		}
	}

	void SpawnCollectables()
	{
		IceCube tempCubeRef;
		for(int i = 0; i < max_collectables; i++)
		{
			tempCubeRef = null;
			while(tempCubeRef == null || tempCubeRef.isOccupied)
			{
				tempCubeRef = getCubeAtIndex(Random.Range(0, width * height * depth));
			}
			string treasurePrefabName = "IBTreasure";
			Vector3 position = tempCubeRef.transform.position;
			GameObject collectable = PhotonNetwork.InstantiateSceneObject("IBTreasure", position, Quaternion.identity, Global.ICE_CUBE_INDEX, null) as GameObject;
			Debug.Log(collectable);
			collectable.transform.parent = transform;
			collectables.Add(collectable.GetComponent<Collectable>());

			tempCubeRef.isOccupied = true;
		}
	}

	void SpawnCarts()
	{
		IceCube tempCubeRef;
		for(int i = 0; i < cart_count; i++)
		{
			tempCubeRef = null;		
			while(tempCubeRef == null || tempCubeRef.isOccupied)
			{
				tempCubeRef = getCubeAtIndex(Random.Range(0, width * height * depth));
			}
			Vector3 position = tempCubeRef.transform.position;
			GameObject cart = PhotonNetwork.InstantiateSceneObject(prefabCart.name, position, Quaternion.identity, Global.ICE_CUBE_INDEX, null) as GameObject;
			cart.transform.parent = transform;
			carts.Add(cart.GetComponent<Collector>());

			tempCubeRef.isOccupied = true;
		}
	}

	//Spawns players on opposite corners
	public void PlacePlayersAtEdge(Player player, int playerIndex)
	{
		/*
		IceCube cube;// = map [index];
		int mapIndex;
		if (playerIndex == 0) {
			mapIndex = (int)( width * height * .5f);
		} else {
			mapIndex = (int)((width * depth * height ) - (int)(width * height * .5f)) + width - 1;
		}
		cube = map [mapIndex];

		//Place the player
		Transform tf = map [mapIndex].transform;
		player.transform.position = tf.transform.position;

		//Destroy the cube
		Destroy (cube.gameObject);
		*/
	}

	public Vector3 FindRespawnPosition()
	{
		IceCube tempCubeRef = null;		
		while(tempCubeRef == null)
		{
			tempCubeRef = getCubeAtIndex(Random.Range(0, width * height * depth));
		}
		float cubeSize = prefabIceCube.transform.localScale.x;
		Vector3 position = new Vector3(tempCubeRef.transform.position.x, cubeSize * (height + 2), tempCubeRef.transform.position.z);
		return position;
	}

	public IceCube getCubeAtIndex(int index)
	{
		return map[index];
	}

	public int getCollectableCount()
	{
		return collectables.Count;
	}

	public void DestroyCube(IceCube cube)
	{
		//photonView.RPC ("DestroyMapObject", PhotonTargets.Others, cube.index);
		DestroyMapObject (cube.index);
	}

	[RPC]
	void DestroyMapObject(int index)
	{
		if (map [index] != null)
			PhotonNetwork.Destroy (map [index].gameObject);
	}
}

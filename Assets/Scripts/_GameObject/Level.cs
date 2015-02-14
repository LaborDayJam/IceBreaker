using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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

	public List<BaseObject> deleteQueue;

	void Start()
	{
		deleteQueue = new List<BaseObject> ();
	}
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
				tempCubeRef = getCubeAtIndex(UnityEngine.Random.Range(0, width * height * depth));
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
				tempCubeRef = getCubeAtIndex(UnityEngine.Random.Range(0, width * height * depth));
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
			tempCubeRef = getCubeAtIndex(UnityEngine.Random.Range(0, width * height * depth));
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
		if (deleteQueue.Contains (cube))
			return;
		//if(deleteQueue.Find
		if (PhotonNetwork.isMasterClient || cube.photonView.isMine)
			PhotonNetwork.Destroy (cube.gameObject);
		else {
			StartCoroutine(CR_DeleteBaseObject(cube));
		}
	}

	IEnumerator CR_DeleteBaseObject(BaseObject obj)
	{
		deleteQueue.Add (obj);
		obj.photonView.RequestOwnership ();
		while (!obj.photonView.isMine) {
			yield return 0;		
		}
		PhotonNetwork.Destroy(obj.gameObject);
	}

	[RPC]
	void DestroyMapObject(int index)
	{
		if (map [index] != null)
		{
			//map [index].photonView.RequestOwnership ();
		}
	}

	
	void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		//If I'm master (I have made the map)
		// send the info out to new players
		if (PhotonNetwork.isMasterClient)
		{
			photonView.RPC("SyncMap", PhotonTargets.OthersBuffered, getSerializedMap());
		}
	}

	String getSerializedMap()
	{	
		List<Dictionary<string,int>> tileList = new List<Dictionary<string, int>>();
		foreach(IceCube tile in map)
		{
			Dictionary<string,int> dTile = new Dictionary<string, int>();
			dTile["mapIndex"] = tile.index;
			dTile["photonViewId"] = tile.photonView.viewID;
			tileList.Add(dTile);
		}

		string serial = MiniJSON.Json.Serialize(tileList);
		print(serial);
		return serial;
	}

	[RPC]
	void SyncMap(String data)
	{
		Debug.Log ("Getting ... " + data);
		GameObject world = gameObject;

		map = new List<IceCube> (width * height * depth);
		IList mapJSON = (IList) MiniJSON.Json.Deserialize(data );
		print("Tile Count " + mapJSON.Count);
		GameObject[] cubes = GameObject.FindGameObjectsWithTag ("Blocks");

		IceCube currentBlock = null;
		foreach (IDictionary _tile in mapJSON) {
			int photonViewId = Convert.ToInt32(_tile["photonViewId"]);

			//TODO : REFACTOR AND OPTIMIZE
			for(int i = 0; i < cubes.Length; i++)
			{
				if(cubes[i].GetComponent<PhotonView>().viewID == photonViewId)
				{
					currentBlock = cubes[i].GetComponent<IceCube>();
					break;
				}
			}
			currentBlock.transform.parent = transform;
			currentBlock.index = Convert.ToInt32(_tile["mapIndex"]);
			map.Insert(currentBlock.index, currentBlock);
		} 
	}


/*
 * [RPC]
void ChatMessage(string a, string b, PhotonMessageInfo info)
{
    Debug.Log(String.Format("Info: {0} {1} {2}", info.sender, info.photonView,
info.timestamp));
}
 * */
}

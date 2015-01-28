using UnityEngine;
using System.Collections;

public class IBClient : MonoBehaviour {

	void Awake() {
		MasterServer.ClearHostList();
		MasterServer.RequestHostList("Collect");
		StartCoroutine (CR_GetServerList ());
	}

	IEnumerator CR_GetServerList()
	{
		while (MasterServer.PollHostList().Length == 0) {
			yield return new WaitForSeconds(.5f);
		}
		HostData[] hostData = MasterServer.PollHostList();
		int i = 0;
		while (i < hostData.Length) {
			Debug.Log("Game name: " + hostData[i].gameName);
			i++;
		}
		Debug.Log ("Conencting to server ..." + hostData [0].ip[0]);
		Network.Connect ("67.255.180.24", "23466");
		//MasterServer.ClearHostList();
	}

	void OnConnectedToServer() {
		Debug.Log("Connected to server");
	}

}

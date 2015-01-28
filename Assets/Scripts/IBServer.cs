using UnityEngine;
using System.Collections;

public class IBServer : MonoBehaviour {

	const string gameName = "IceBreaker";
	const string gameType = "Collect";

	void Start(){
		StartServer ();
	}
	// Use this for initialization
	void StartServer () {
		NetworkConnectionError err = Network.InitializeServer(2, 25002, !Network.HavePublicAddress());
		MasterServer.RegisterHost (gameType, gameName);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnServerInitialized()
	{
		Debug.Log("Server Initialized");
	}
	
	public void ServerStart()
	{
		Debug.Log("Starting server");
		//StartServer();
	}
	
	public void RefreshHosts()
	{
		Debug.Log("Refresh server");
	}

	void OnMasterServerEvent(MasterServerEvent mse)
	{
		if(mse == MasterServerEvent.RegistrationSucceeded)
		{
			Debug.Log("Registered a server");
			Debug.Log(MasterServer.ipAddress + "  " + MasterServer.port);
		}
	}

	void OnGUI()
	{
		GUI.Label (new Rect (0, 0, 100, 100), MasterServer.ipAddress + "  " + MasterServer.port);
	}

	void OnPlayerConnected()
	{
		Debug.Log ("Player Connected");
	}
}

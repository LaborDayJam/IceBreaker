using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour 
{

	
	public GameObject MainPanel;
	public GameObject GameModePanel;
	public GameObject ConnectionTypePanel;
	public GameObject InternetPanel;
	public GameObject AvaiableGamesPanel;
	
	
	private int gameMode = -1;
	private int gameType = -1;
	
	public void Awake()
	{
		MainPanel.SetActive(true);
	}
	public void StartPressed()
	{
		MainPanel.SetActive(false);
		GameModePanel.SetActive(true);
	}
	public void CreditsPressed(){}
	public void ExitPressed()
	{
		Application.Quit();
	}
	public void CombatPressed()
	{
		gameMode = 0;
		GameModePanel.SetActive(false);
		ConnectionTypePanel.SetActive(true);
	}

	public void FarmPressed()
	{
		GameModePanel.SetActive(false);
		ConnectionTypePanel.SetActive(true);
		gameMode = 1;
	}
	public void GameModeBackPressed()
	{
		gameMode = -1;
		GameModePanel.SetActive(false);
		MainPanel.SetActive(true);
	}
	public void VSPressed()
	{
		gameType = 0;
		ConnectionTypePanel.SetActive(false);
		//Todo: Start split screen game
	}
	public void LocalPressed()
	{
		gameType = 1;
		ConnectionTypePanel.SetActive(false);
		InternetPanel.SetActive(true);
	}
	public void InternetPressed()
	{
		gameType = 2;
		ConnectionTypePanel.SetActive(false);
		InternetPanel.SetActive(true);
		
	}
	public void ConnectionTypeBackPressed()
	{
		gameType = -1;
		ConnectionTypePanel.SetActive(false);
		GameModePanel.SetActive(true);
		
	}
	public void JoinPressed()
	{
		ConnectionTypePanel.SetActive(false);
		AvaiableGamesPanel.SetActive(true);
	}
	public void HostPressed(){}
	
	public void InternetBackPressed()
	{	
		InternetPanel.SetActive(false);
		ConnectionTypePanel.SetActive(true);
	}
	
	
	
	
}

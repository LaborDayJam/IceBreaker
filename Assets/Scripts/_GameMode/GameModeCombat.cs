using UnityEngine;
using System.Collections;

public class GameModeCombat : GameMode {
	
	private static GameModeCombat instance;
	public static GameModeCombat Instance { get { return instance; } }
	
	void Awake()
	{
		if(instance == null)
			instance = this;
		else{
			Debug.Log("WARNING: DUPLICATE GAME MODE OBJECT. DESTROYING SELF");
			Destroy(gameObject);
		}
	}

	void Start()
	{
		StartGame ();		
	}

	public override void PlayerFell(Player player)
	{
		GameOver();
		
		if (player.team == TEAM_ICEBREAKER_INDEX) {
			Debug.Log ("Team Igloo Wins"); 
		} else
			Debug.Log ("Team Icebreaker Wins");
	}
	
	protected override IEnumerator CR_GameLogicLoop()
	{
		while (true) {
			yield return new WaitForSeconds(3.0f); //check every 3 seconds
		}
	}
}

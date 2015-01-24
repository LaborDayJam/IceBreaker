using UnityEngine;
using System.Collections;

public class GameMode : MonoBehaviour {

	protected virtual IEnumerator CR_CheckWinCondition()
	{
		yield return 0;
	}

	protected virtual IEnumerator CR_CheckLoseCondition()
	{
		yield return 0;
	}

	protected virtual IEnumerator CR_GameLogicLoop()
	{
		yield return 0;
	}

	protected virtual void GameOver() { }
	
}

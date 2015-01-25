using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGemCounter : MonoBehaviour {

	public Image[] gemCounter;
	public void UpdateGemCounter(int gemCount)
	{
		if (gemCount >= gemCounter.Length) 
			return;

		int index = 0;
		while (index < gemCount) {
			gemCounter[index].enabled = true;
			index++;
		}
	}
}

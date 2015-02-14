using UnityEngine;
using System.Collections;

public class IBServer : MonoBehaviour {
	bool isRunning = false;

	IEnumerator MyCoroutine(){
		isRunning = true;
		print("started");
		yield return new WaitForSeconds(3);
		print("3 seconds elapsed");
		yield return new WaitForSeconds(3);
		print("more 3 seconds");
		yield return new WaitForSeconds(2);
		print("ended");
		isRunning = false;
	}
	
	void Update(){
		 StartCoroutine(MyCoroutine());
	}
}

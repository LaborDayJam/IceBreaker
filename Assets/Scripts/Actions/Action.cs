using UnityEngine;
using System.Collections;

public enum ACTION_TYPE {ONCE, CONTINOUS, SIZE};
public class Action : MonoBehaviour {

	public ACTION_TYPE type; 
	public float range = 1;
	public float rateOfFire = .5f;

	public bool isReady;

	public virtual void Do() { }

	protected IEnumerator CR_StartCooldown()
	{
		yield return new WaitForSeconds(rateOfFire);
		isReady = true;
	}
}

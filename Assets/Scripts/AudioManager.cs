using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;
	public AudioClip [] clips;
	
	public GameObject prefabSoundObject;
	
	// Use this for initialization
	void Awake () 
	{
		if (instance == null)
			instance = this;
		else {
			Debug.LogWarning("Duplicate AudioManager. Deleting this game object");	
			Destroy (this.gameObject);
		}	
	}
	
	public  void PlayOneShot(int sfx, Vector3 position)
	{
		GameObject clone = Instantiate(prefabSoundObject, position, Quaternion.identity)as GameObject;
		clone.audio.PlayOneShot(clips[sfx]);
		GameObject.Destroy (clone, clips [sfx].length);
	}
	
	public  void Play(bool loop, int track)
	{
		
	}
}
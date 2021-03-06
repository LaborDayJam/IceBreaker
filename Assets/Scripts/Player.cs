﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Player_Input_Type{PC, GAMEPAD, SIZE};
public class Player : BaseObject {

	int STATE_NORMAL = 0;
	int STATE_STUNNED = 1;
	int state = 0; 	//0 normal

	float stunDuration = 1.5f;

	public Camera cam;

	//Actions
	public Action currentAction;
	HitAction hitAction;
	ShootAction shootAction;

	//Input
	public Player_Input_Type inputType;
	public string moveX;
	public string moveY;
	public string lookX;
	public string lookY;
	public string switchAction;
	public string performAction;

	//Team
	public int team;
	public int playerName;

	public int totalPoints;
	public int carryPoints;

	public float knockbackForce = 60;

	public string actionButton = "Fire1";
	public FPSWalkerEnhanced controller;

	bool isPerformActioning = false;

	public Animator characterAnimations;
	public GameObject rayGunForOtherPlayer;
	public GameObject rayGunForPlayer;
	public GameObject pickAxeForPlayer;
	public GameObject pickAxeForOtherPlayer;

	public UIGemCounter gemCounter;

	public int playerNum;

	void Awake()
	{
		hitAction = gameObject.AddComponent<HitAction>();
		shootAction = gameObject.AddComponent<ShootAction> ();
		currentAction = hitAction;
		//Debug.Log (Input.GetJoystickNames().Length);
	}


	void Start()
	{
		gemCounter.UpdateGemCounter (0);
	}
	public void BindControls()
	{
		InputManager.Instance.AssignControls(this, inputType);

		if(inputType == Player_Input_Type.PC)
		{
			cam.GetComponent<MouseLook>().enabled = false;
		}
	}

	// Update is called once per frame
	void Update () {
		HandleInput ();
		print (cam.rect + " " + gameObject.name);
		
		if(cam.rect == new Rect (0, 0, .5f, 1)){
			cam.cullingMask =  (1 << LayerMask.NameToLayer("TransparentFX")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("PlayerOne"));
			gameObject.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = LayerMask.NameToLayer("PlayerTwo");
			rayGunForPlayer.layer = LayerMask.NameToLayer("PlayerOne");
			rayGunForOtherPlayer.layer = LayerMask.NameToLayer("PlayerTwo");
			pickAxeForPlayer.layer = LayerMask.NameToLayer("PlayerOne");
			pickAxeForOtherPlayer.layer = LayerMask.NameToLayer("PlayerTwo");
			
			print ("setting this stuff");
			
		}
		else {
			cam.cullingMask =  (1 << LayerMask.NameToLayer("TransparentFX")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("PlayerTwo"));
			gameObject.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = LayerMask.NameToLayer("PlayerOne");
			rayGunForPlayer.layer = LayerMask.NameToLayer("PlayerTwo");
			rayGunForOtherPlayer.layer = LayerMask.NameToLayer("PlayerOne");
			pickAxeForPlayer.layer = LayerMask.NameToLayer("PlayerTwo");
			pickAxeForOtherPlayer.layer = LayerMask.NameToLayer("PlayerOne");
			
		}
		
		

	}

	void HandleInput()
	{
		switch(inputType)
		{
			case Player_Input_Type.PC:
			{
				if(Input.GetKeyDown (KeyCode.Alpha1))
					currentAction = hitAction;

				 if (Input.GetKeyDown (KeyCode.Alpha2))
				 	currentAction = shootAction;

			 	if(Input.GetMouseButton(0))
				 	isPerformActioning = true;
				else
					isPerformActioning = false;
			}break;
			case Player_Input_Type.GAMEPAD:
			{
				if (Input.GetButtonDown(switchAction) && currentAction == shootAction) {
					currentAction = hitAction;
					rayGunForPlayer.SetActive(false);
					rayGunForOtherPlayer.SetActive(false);
					pickAxeForPlayer.SetActive(true);
					pickAxeForOtherPlayer.SetActive(true);




				} else if (Input.GetButtonDown(switchAction) && currentAction == hitAction) {
					currentAction = shootAction;
					rayGunForPlayer.SetActive(true);
					rayGunForOtherPlayer.SetActive(true);
					pickAxeForPlayer.SetActive(false);
					pickAxeForOtherPlayer.SetActive(false);


				}
				else {
					characterAnimations.SetBool("attacking",false);
					characterAnimations.SetBool("shooting",false);

				}
				if(Input.GetButtonDown(performAction) && currentAction == shootAction){
					isPerformActioning = true;
					characterAnimations.SetBool("shooting",true);
					characterAnimations.SetBool("walking",false);
					characterAnimations.SetBool("jumping",false);
					characterAnimations.SetBool("attacking",false);
					
				}
				else if(Input.GetButton(performAction)&& currentAction == hitAction){
					isPerformActioning = true;
					characterAnimations.SetBool("attacking",true);
					characterAnimations.SetBool("walking",false);
					characterAnimations.SetBool("jumping",false);
					characterAnimations.SetBool("shooting",false);
				}
				else{
					isPerformActioning = false;
				}
		
			}break;
			default:
			break;
		}

		if(!isPerformActioning)

			return;

		switch(currentAction.type)
		{
		case ACTION_TYPE.ONCE:
		{
			currentAction.Do();
		}break;
		case ACTION_TYPE.CONTINOUS:
		{
			currentAction.Do();
		}break;
		default:
			break;
		}
	}

	//For when we are hit by the laser ray
	IEnumerator CR_Stunned()
	{
		state = STATE_STUNNED;
		yield return new WaitForSeconds (stunDuration);
		state = STATE_NORMAL;
	}

	public override void onHit (GameObject other, float damage)
	{
		Player attacker = other.GetComponent<Player> ();
		if (attacker.currentAction is HitAction) {
			Vector3 direction = (transform.position - other.transform.position).normalized;
			GetComponent<ImpactReceiver>().AddImpact(direction, knockbackForce);
		} else if (attacker.currentAction is ShootAction) {
			Vector3 direction = (transform.position - other.transform.position).normalized;
			GetComponent<ImpactReceiver>().AddImpact(direction, knockbackForce* 10);
		}
		//base.onHit (other, damage);
	}

	public void CarryPoints(int points)
	{
		carryPoints += points;
		Debug.Log (totalPoints);
	}

	public void ScorePoints()
	{
		totalPoints += carryPoints;
		carryPoints = 0;
		gemCounter.UpdateGemCounter (totalPoints);
		Debug.Log ("Total Points for " + name + " : " + totalPoints);
	}

	public void OnFall()
	{
		carryPoints = 0;
		
		if (GameModeCombat.Instance != null)
			GameModeCombat.Instance.PlayerFell (this);
		else if (GameModeCollect.Instance != null)
			GameModeCollect.Instance.PlayerFell (this);
	}

	void OnDestroy()
	{
		StopAllCoroutines ();
	}
}

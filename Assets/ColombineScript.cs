﻿using UnityEngine;
using System.Collections;

public class ColombineScript : MonoBehaviour {
	
	public GameManager gameManager;
		
	public void talk(int type){
		StartCoroutine (talkCoroutine (type));
	}
	public void touch(int type){
		StartCoroutine (touchCoroutine (type));
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

		
	}
	
	IEnumerator talkCoroutine(int type){
		
		gameManager.guiManager.active = false;
		
		Vector3 moveEvent = new Vector3 (-10, 7, 30);
		gameManager.character.goTo (moveEvent);
		
		while (gameManager.character.transform.position !=  moveEvent) {
			yield return null;
		}
		
		if(type == 0)
			gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("niceTalking");
		else if (type == 1)
			gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("angryTalking");
		
		
		yield return new WaitForSeconds(gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);
		
		if (type == 1) {
			gameManager.capitaine.GetComponent<CharacterController>().sprite.SetTrigger("peur");
			//feed back du souffleur
		}

		gameManager.guiManager.active = true;
		
		yield break;
	}
	
	IEnumerator touchCoroutine(int type){
		
		gameManager.guiManager.active = false;
		
		Vector3 moveEvent = new Vector3 (-10, 7, 30);
		gameManager.character.goTo (moveEvent);
		
		while (gameManager.character.transform.position !=  moveEvent) {
			yield return null;
		}
		
		if(type == 0)
			gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("poke");
		else if (type == 1)
			gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("frappe");
		
		
		yield return new WaitForSeconds(gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);	
		
		
		if (type == 1) {
			gameManager.capitaine.GetComponent<CharacterController>().sprite.SetTrigger("peur");
			//feed back du souffleur
		}

		
		gameManager.guiManager.active = true;
		
		yield break;
	}
}

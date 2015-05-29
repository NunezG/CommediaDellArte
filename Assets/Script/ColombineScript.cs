using UnityEngine;
using System.Collections;

public class ColombineScript : MonoBehaviour {
	
	public GameManager gameManager;
	private int actionCount = 0;


	public void talk(int type){
		StartCoroutine (talkCoroutine (type));
	}
	public void touch(int type){
		StartCoroutine (touchCoroutine (type));
	}
	
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}
	
	IEnumerator talkCoroutine(int type){
		
		gameManager.guiManager.active = false;
		
		Vector3 moveEvent = new Vector3 (11, 7, 30);
		gameManager.character.goTo (moveEvent);
		
		while (gameManager.character.transform.position !=  moveEvent) {
			yield return null;
		}
				
		gameManager.character.transform.Rotate (0, 180, 0);

		if (type == 0) {
			gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("niceTalking");
			while(gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).shortNameHash !=  Animator.StringToHash("Nice Talking") ){
				yield return null;
			}
		}
		else if (type == 1) {
			gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("angryTalking");
			while(gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).shortNameHash !=  Animator.StringToHash("Angry Talking") ){
				yield return null;
			}
		}
		
		yield return new WaitForSeconds(gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);

		if(type == 0){
			gameManager.souffleur.giveFeedback (2, 0);
			gameManager.publicOnScene.addValue(10);
		}
		else if (type == 1){
			gameManager.souffleur.giveFeedback (2, 1);
			gameManager.publicOnScene.subValue(10);
		}
		gameManager.character.transform.Rotate (0, 180, 0);

		gameManager.guiManager.active = true;

		actionCount++;

		if (actionCount >= 2) {
			StartCoroutine(gameManager.lazziEvent());
		}

		yield break;
	}
	
	IEnumerator touchCoroutine(int type){
		
		gameManager.guiManager.active = false;
		
		Vector3 moveEvent = new Vector3 (11, 7, 30);
		gameManager.character.goTo (moveEvent);
		
		while (gameManager.character.transform.position !=  moveEvent) {
			yield return null;
		}

		gameManager.character.transform.Rotate (0, 180, 0);
		
		if (type == 0) {
			gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("poke");
			while(gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).shortNameHash !=  Animator.StringToHash("poke") ){
				yield return null;
			}
		}
		else if (type == 1) {
			gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("frappe");
			while(gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).shortNameHash !=  Animator.StringToHash("frappe") ){
				yield return null;
			}
			gameManager.character.GetComponent<AudioSource> ().PlayOneShot (gameManager.character.GetComponent<ArlequinScript>().coup,1);
		}


		yield return new WaitForSeconds (gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).length);		

		gameManager.character.transform.Rotate (0, 180, 0);

		if(type == 0){
			gameManager.souffleur.giveFeedback (2, 0);
			gameManager.publicOnScene.addValue(10);
		}
		else if (type == 1){
			gameManager.souffleur.giveFeedback (2, 1);
			gameManager.publicOnScene.subValue(10);
		}

		gameManager.guiManager.active = true;

		actionCount++;

		if (actionCount >= 2) {
			StartCoroutine(gameManager.lazziEvent());
		}

		yield break;
	}
}

using UnityEngine;
using System.Collections;

public class ColombineScript : MonoBehaviour {
	
	public GameManager gameManager;
	private int actionCount = 3;


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

		if(type == 0)
			gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("niceTalking");
		else if (type == 1)
			gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("angryTalking");
		
		
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

		if (actionCount >= 4) {
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
		
		if (type == 0)
			gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("poke");
		else if (type == 1)
			gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("frappe");
		
		
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

		if (actionCount >= 4) {
			StartCoroutine(gameManager.lazziEvent());
		}

		yield break;
	}
}

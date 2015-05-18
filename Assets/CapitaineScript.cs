using UnityEngine;
using System.Collections;

public class CapitaineScript : MonoBehaviour {

	public GameManager gameManager;
	
	private int scaryValue = 0;
	private bool talkDone = false, touchDone = false;

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
			this.GetComponent<CharacterController> ().sprite.SetTrigger ("peur");
			//feed back du souffleur
			gameManager.souffleur.giveFeedback (2, 0);

			if (!talkDone) {
				scaryValue += 50;
				gameManager.publicOnScene.addValue (30);
				talkDone = true;
			}
			yield return new WaitForSeconds (gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).length+0.3f);
		}

		gameManager.guiManager.active = true;

		if (scaryValue >= 100) {
			StartCoroutine(goAway());
		}

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
			gameManager.capitaine.GetComponent<CharacterController> ().sprite.SetTrigger ("peur");
			//feed back du souffleur
			gameManager.souffleur.giveFeedback (2, 0);
			
			if (!touchDone) {
				scaryValue += 50;
				gameManager.publicOnScene.addValue (30);
				touchDone = true;
			}
			yield return new WaitForSeconds (gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).length+0.3f);	
		}

		gameManager.guiManager.active = true;

		if (scaryValue >= 100) {
			StartCoroutine(goAway());
		}

		yield break;
	}

	IEnumerator goAway(){

		Vector3 moveEvent = new Vector3 (-20, 7, 50);

		this.GetComponent<CharacterController>().goTo(moveEvent);
		yield return new WaitForSeconds(1.5f);
		gameManager.publicOnScene.addValue(20);
		StartCoroutine (gameManager.event3 ());
		yield break;
	}
}

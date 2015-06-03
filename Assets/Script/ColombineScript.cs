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
        gameManager.getCharacterGameobject("Arlequin").GetComponent<CharacterController>().goTo(moveEvent);
		
		while (gameManager.getCharacterGameobject("Arlequin").transform.position !=  moveEvent) {
			yield return null;
		}
				
		gameManager.getCharacterGameobject("Arlequin").transform.Rotate (0, 180, 0);

		if (type == 0) {
			gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator> ().SetTrigger ("niceTalking");
			while(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).shortNameHash !=  Animator.StringToHash("niceTalking") ){
				yield return null;
			}
		}
		else if (type == 1) {
			gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator> ().SetTrigger ("angryTalking");
			while(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).shortNameHash !=  Animator.StringToHash("angryTalking") ){
				yield return null;
			}
		}
		
		yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);

		if(type == 0){
			gameManager.souffleur.giveFeedback (2, 0, 0);
			gameManager.publicOnScene.addValue(10);
		}
		else if (type == 1){
			gameManager.souffleur.giveFeedback (2, 0, 1);
			gameManager.publicOnScene.subValue(10);
		}
		gameManager.getCharacterGameobject("Arlequin").transform.Rotate (0, 180, 0);

		actionCount++;

		if (actionCount >= 2) {
            //yield return new WaitForSeconds(3);
			//StartCoroutine(gameManager.lazziEvent());
            gameManager.startEvent("Tutorial_4", gameManager.getEventList());
		}
        else
            gameManager.guiManager.active = true;

		yield break;
	}
	
	IEnumerator touchCoroutine(int type){
		
		gameManager.guiManager.active = false;
		
		Vector3 moveEvent = new Vector3 (11, 7, 30);
		gameManager.getCharacterGameobject("Arlequin").GetComponent<CharacterController>().goTo (moveEvent);
		
		while (gameManager.getCharacterGameobject("Arlequin").transform.position !=  moveEvent) {
			yield return null;
		}

		gameManager.getCharacterGameobject("Arlequin").transform.Rotate (0, 180, 0);
		
		if (type == 0) {
			gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator> ().SetTrigger ("poke");
			while(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).shortNameHash !=  Animator.StringToHash("poke") ){
				yield return null;
			}
		}
		else if (type == 1) {
			gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator> ().SetTrigger ("frappe");
			while(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).shortNameHash !=  Animator.StringToHash("frappe") ){
				yield return null;
			}
			gameManager.getCharacterGameobject("Arlequin").GetComponent<AudioSource> ().PlayOneShot (gameManager.getCharacterGameobject("Arlequin").GetComponent<ArlequinScript>().coup,1);
		}


		yield return new WaitForSeconds (gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).length);		

		gameManager.getCharacterGameobject("Arlequin").transform.Rotate (0, 180, 0);

		if(type == 0){
			gameManager.souffleur.giveFeedback (2, 0, 0);
			gameManager.publicOnScene.addValue(10);
		}
		else if (type == 1){
			gameManager.souffleur.giveFeedback (2, 0, 1);
			gameManager.publicOnScene.subValue(10);
		}


		actionCount++;

		if (actionCount >= 2) {
           // yield return new WaitForSeconds(3);
			//StartCoroutine(gameManager.lazziEvent());
            gameManager.startEvent("Tutorial_4", gameManager.getEventList());
		}
        else 
            gameManager.guiManager.active = true;

		yield break;
	}
}

using UnityEngine;
using System.Collections;

public class CoffreScript : MonoBehaviour {

	public GameManager gameManager;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void jugggle(){
		StartCoroutine (juggleCoroutine ());
	}
	public void eggJugggle(){
		StartCoroutine (eggJuggleCoroutine ());
	}



	IEnumerator juggleCoroutine(){

		gameManager.guiManager.active = false;

		Vector3 moveToChestEvent = new Vector3 (13, 7, 30);
		gameManager.character.goTo (moveToChestEvent);
		while (gameManager.character.transform.position !=  moveToChestEvent) {
			yield return null;
		}

		//animation prendre;

		Vector3 goToCenterEvent = new Vector3 (-1.5f, 7, 30);
		gameManager.character.goTo (goToCenterEvent);
		while (gameManager.character.transform.position !=  goToCenterEvent) {
			yield return null;
		}

		gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("juggling");
		yield return new WaitForSeconds(gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length + 0.2f);

		//retour negatif du souffleur

		gameManager.character.goTo (moveToChestEvent);
		while (gameManager.character.transform.position !=  moveToChestEvent) {
			yield return null;
		}

		//animation reposer

		gameManager.guiManager.active = true;

		yield break;
	}

	IEnumerator eggJuggleCoroutine(){
		
		gameManager.guiManager.active = false;
		
		Vector3 moveEvent = new Vector3 (13, 7, 30);
		gameManager.character.goTo (moveEvent);
		
		while (gameManager.character.transform.position !=  moveEvent) {
			yield return null;
		}

		//animation prendre

		gameManager.character.GetComponentInChildren<Animator> ().SetTrigger ("eggJuggling");
		yield return new WaitForSeconds(gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);
			
		//animation depit

		gameManager.publicOnScene.happy (2, 2);

		gameManager.guiManager.active = true;
		
		yield break;
	}


}



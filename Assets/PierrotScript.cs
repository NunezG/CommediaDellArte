using UnityEngine;
using System.Collections;

public class PierrotScript : MonoBehaviour {


	public GameManager gameManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void juggle(){

	}

	
	IEnumerator juggleCoroutine(){
		
		gameManager.guiManager.active = false;
	
		Vector3 goToCenterEvent = new Vector3 (-1.5f, 7, 30);
		Vector3 goAwayEvent = new Vector3 (-1.5f, 7, 30);

		this.GetComponent<CharacterController>().goTo (goToCenterEvent);
		while (gameManager.character.transform.position !=  goToCenterEvent) {
			yield return null;
		}
		
		this.GetComponentInChildren<Animator> ().SetTrigger ("juggling");
		yield return new WaitForSeconds(gameManager.character.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length + 0.2f);
	
		gameManager.publicOnScene.addValue (100);

		this.GetComponent<CharacterController>().goTo (goToCenterEvent);
		while (gameManager.character.transform.position !=  goToCenterEvent) {
			yield return null;
		}
	
		gameManager.guiManager.active = true;
		
		yield break;
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PierrotScript : MonoBehaviour {
	
	public GameManager gameManager;

	private CharacterController _characterController;

	void Start () {
		_characterController = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void juggle(){

	}
	
	IEnumerator juggleCoroutine(){
		
		gameManager.guiManager.active = false;
	
		Vector3 goToCenterEvent = new Vector3 (-1.5f, 10, 30);
		Vector3 goAwayEvent = new Vector3 (50f, 10, 30);
		Vector3 zoomLazziEvent = new Vector3 (0, 13, 10);

		_characterController.goTo (goToCenterEvent);
		while (this.transform.position !=  goToCenterEvent) {
			yield return null;
		}

		//zoom
		gameManager.camera.moveTo (zoomLazziEvent);

		this.GetComponentInChildren<Animator> ().SetTrigger ("juggling");
		yield return new WaitForSeconds(this.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length + 0.2f);
	
		gameManager.publicOnScene.setValue (75);

		//dezoom
		gameManager.camera.resetPosition ();

		_characterController.goTo (goAwayEvent);
		while (this.transform.position !=  goAwayEvent) {
			yield return null;
		}
	
		gameManager.guiManager.active = true;
		
		yield break;
	}

}

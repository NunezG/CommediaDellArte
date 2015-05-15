using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public CharacterController character;
	public SouffleurScript souffleur;
	public PublicScript publicOnScene;
	public CameraScript camera;
	public GUIManager guiManager;

	bool test = true;
	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
		if(test)
			StartCoroutine (event1 ());
		test = false;
	}

	IEnumerator event1(){

		Vector3 moveEvent1 =  new Vector3(-16,7,28);
		character.goTo (moveEvent1);

		while (character.transform.position != moveEvent1) {
			yield return null;
		}

		souffleur.saySomething (souffleur.textList1);
		
		while (souffleur.talking == true) {
			yield return null;
		}

		Vector3 zoomPublicEvent = new Vector3(0,6,0);

		publicOnScene.addValue (20);
		souffleur.saySomething (souffleur.textList2);
		camera.moveTo (zoomPublicEvent);

		while (souffleur.talking == true) {
			yield return null;
		}
		camera.resetPosition ();

		guiManager.active = false;
		yield return new WaitForSeconds (1.0f);

		Vector3 zoomCoffreEvent = new Vector3(17,10,16);

		souffleur.saySomething (souffleur.textList3);
		camera.moveTo (zoomCoffreEvent);

		while (souffleur.talking == true) {
			yield return null;
		}
		camera.resetPosition ();

		yield break;



	}

}

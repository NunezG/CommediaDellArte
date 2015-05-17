using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public CharacterController character;
	public CapitaineScript capitaine;
	public PantaloneScript pantalone;
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

	//Intro avec le souffleur
	public IEnumerator event1(){

		guiManager.active = false;

		Vector3 moveEvent1 =  new Vector3(-16,7,30);
		character.goTo (moveEvent1);

		while (character.transform.position != moveEvent1) {
			yield return null;
		}

		souffleur.saySomething (souffleur.textList1, false);

		while (souffleur.talking == true) {
			yield return null;
		}

		Vector3 zoomPublicEvent = new Vector3(0,6,0);

		publicOnScene.addValue (20);
		souffleur.saySomething (souffleur.textList2, false);
		camera.moveTo (zoomPublicEvent);

		while (souffleur.talking == true) {
			yield return null;
		}
		camera.resetPosition ();

		guiManager.active = false;
		yield return new WaitForSeconds (1.0f);

		Vector3 zoomCoffreEvent = new Vector3(17,10,16);

		souffleur.saySomething (souffleur.textList3, false);
		camera.moveTo (zoomCoffreEvent);

		while (souffleur.talking == true) {
			yield return null;
		}
		camera.resetPosition ();

		guiManager.active = true;

		yield break;

	}

	//Arrivé du capitaine
	public IEnumerator event2(){

		guiManager.active = false;

		Vector3 moveEvent1 =  new Vector3(-16,7,30);
		capitaine.GetComponent<CharacterController>().goTo (moveEvent1);

		while (capitaine.transform.position !=  moveEvent1) {
			yield return null;
		}

		capitaine.GetComponent<CharacterController>().sprite.SetTrigger ("moquerie");

		souffleur.saySomething (souffleur.textList5, false);
		while (souffleur.talking == true) {
			yield return null;
		}


		guiManager.active = true;

		yield break;
		
	}

    //Arrivé de pantalone et colombine
    public IEnumerator event3(){
        
        guiManager.active = false;

        souffleur.saySomething (souffleur.textList6, false);
        while (souffleur.talking == true) {
            yield return null;
        }

		pantalone.GetComponentInChildren<Animator> ().SetTrigger("asking");
         //bulle aide pantalone
		yield return new WaitForSeconds(pantalone.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);

        guiManager.active = true;
        
        yield break;
        
    }

}

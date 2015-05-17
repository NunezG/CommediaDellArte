using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public CharacterController character;
	public CapitaineScript capitaine;
	public PantaloneScript pantalone;
	public ColombineScript colombine;
	public SouffleurScript souffleur;
	public CoffreScript coffre;
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
			StartCoroutine (event3 ());
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


		//fondu noir 

		coffre.gameObject.SetActive (false);
		pantalone.transform.position = new Vector3 (-16, 7, 30);
		character.setPositionAndGoal (new Vector3 (-8, 7, 30));
		colombine.transform.position = new Vector3 (16, 7, 30);


		Vector3 zoomPantaloneEvent = new Vector3(-15,12,11);
		Vector3 zoomColombineEvent = new Vector3(17,10,16);

        souffleur.saySomething (souffleur.textList6, false);
        while (souffleur.talking == true) {

			if(souffleur.getIndex() == 1 && Input.GetButtonDown("Fire1")){
				camera.moveTo (zoomPantaloneEvent);
				yield return new WaitForSeconds(2f);
				camera.moveTo (zoomColombineEvent);
				yield return new WaitForSeconds(2f);
				camera.resetPosition ();
				break;
			}
            yield return null;
		}   
		while (souffleur.talking == true) {
			yield return null;
		}

		camera.resetPosition ();
		yield return new WaitForSeconds (2.0f);

		pantalone.GetComponentInChildren<Animator> ().SetTrigger("asking");
		pantalone.GetComponentInChildren<bulleInfoScript> ().showBubble (1.5f);
		yield return new WaitForSeconds(pantalone.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);
		pantalone.GetComponentInChildren<Animator> ().SetTrigger("asking");
		yield return new WaitForSeconds(pantalone.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);

        guiManager.active = true;
        
        yield break;
        
    }

}

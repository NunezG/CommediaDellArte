using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

	public CharacterController character;
	public CapitaineScript capitaine;
	public PantaloneScript pantalone;
	public ColombineScript colombine;
	public SouffleurScript souffleur;
	public PierrotScript pierrot;
	public CoffreScript coffre;
	public PublicScript publicOnScene;
	public BoxCollider2D cloche;
	public CameraScript camera;
	public GUIManager guiManager;
	public FadeBlackScript fadeBlack;

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

	public void launchEndEvent(){
		StartCoroutine (eventFinTuto());
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

		capitaine.GetComponent<AudioSource> ().PlayOneShot (capitaine.moquerie,1);

		souffleur.saySomething (souffleur.textList5, false);
		while (souffleur.talking == true) {
			yield return null;
		}

		//activation du capitaine
		capitaine.GetComponent<Collider2D> ().enabled = true;

		guiManager.active = true;

		yield break;
		
	}

    //Arrivé de pantalone et colombine
    public IEnumerator event3(){
        
        guiManager.active = false;

		//fondu noir 
		fadeBlack.fadeToBlack (1.0f);
		yield return new WaitForSeconds (1/fadeBlack.fadeSpeed);
		yield return new WaitForSeconds (1);

		coffre.gameObject.SetActive (false);
		pantalone.transform.position = new Vector3 (-16, 7, 30);
		character.setPositionAndGoal (new Vector3 (-8, 7, 30));
		colombine.transform.position = new Vector3 (16, 7, 30);

		yield return new WaitForSeconds (1/fadeBlack.fadeSpeed);
		yield return new WaitForSeconds (0.5f);

		Vector3 zoomPantaloneEvent = new Vector3(-15,12,11);
		Vector3 zoomColombineEvent = new Vector3(17,10,16);
		bool eventPantaloneDone = false, eventColombineDone = false;

        souffleur.saySomething (souffleur.textList6, false);

        while (souffleur.talking == true) {

			if(!eventPantaloneDone && souffleur.getIndex() == 1){
				camera.moveTo (zoomPantaloneEvent);
			}
			if(!eventColombineDone && souffleur.getIndex() == 2){
				camera.moveTo (zoomColombineEvent);				
            }
            yield return null;
		}

        camera.resetPosition ();
		yield return new WaitForSeconds (2.0f);

		pantalone.GetComponentInChildren<Animator> ().SetTrigger("asking");
		pantalone.GetComponentInChildren<bulleInfoScript> ().showBubble (1.5f);
		yield return new WaitForSeconds(pantalone.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);

        guiManager.active = true;
        
        yield break;
        
    }

	//Arrivé du lazzi
	public IEnumerator lazziEvent(){

		guiManager.active = false;

		publicOnScene.subValue (100);

		souffleur.saySomething (souffleur.textList7, false);

		Vector3 zoomClocheEvent = new Vector3 (20, 23, 27);
		
		while (souffleur.talking == true) {
			if(souffleur.getIndex() == 1){
				camera.moveTo(zoomClocheEvent);
				cloche.enabled = true;
				break;
			}
			yield return null;
		}
		while (souffleur.talking == true) {
			yield return null;			
		}
		camera.resetPosition();
		
		guiManager.active = true;

		yield break;
	}


	IEnumerator eventFinTuto(){
		
		guiManager.active = false;

		Vector3 goToCenterEvent = new Vector3 (-1.5f, 10, 30);
		Vector3 zoomLazziEvent = new Vector3 (0, 13, 5);
		
		pierrot.GetComponent<CharacterController>().goTo (goToCenterEvent);
		while (pierrot.transform.position !=  goToCenterEvent) {
			yield return null;
		}
		
		//zoom
		camera.moveTo (zoomLazziEvent);
		
		pierrot.GetComponentInChildren<Animator> ().SetTrigger ("juggling");
		while(pierrot.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).shortNameHash !=  Animator.StringToHash("pierrot_jongle") ){
			yield return null;
		}
		yield return new WaitForSeconds(pierrot.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length );
		
		publicOnScene.setValue (75);
				
		Vector3 goTalkToTpantalone = new Vector3 (-8, 7, 30);
		character.goTo (goTalkToTpantalone);
		while (character.transform.position !=  goTalkToTpantalone) {
			yield return null;
		}	

		pantalone.GetComponentInChildren<Animator> ().SetTrigger ("dispute");
		character.GetComponentInChildren<Animator> ().SetTrigger ("angryTalking");
		yield return new WaitForSeconds(pantalone.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);

		Vector3 moveToColombine = new Vector3 (11, 10, 30);
		pierrot.GetComponent<CharacterController>().goTo (moveToColombine);
		while (pierrot.transform.position !=  moveToColombine) {
			yield return null;
		}
		pierrot.GetComponentInChildren<Animator> ().SetTrigger ("range");
		colombine.gameObject.SetActive (false);

		while(pierrot.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).shortNameHash != Animator.StringToHash("pierrot_range_colombine") ){
			yield return null;
		}
		yield return new WaitForSeconds(pierrot.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);
		

		Vector3 goAwayEvent = new Vector3 (50f, 10, 30);
		pierrot.GetComponent<CharacterController>().goTo (goAwayEvent);
		while (pierrot.transform.position !=  goAwayEvent) {
			yield return null;
		}

		// compte rendu 
		//fadeBlack.fadeToBlack (Mathf.Infinity);
		GameObject.Find ("Gazette").GetComponent<Image>().color = new Color (1, 1, 1, 1);

		while (true) {
			if(Input.GetButtonDown("Fire1")){
				Application.LoadLevel("saynete_1");
				break;
			}
			yield return null;
		}

		yield break;
	}

}

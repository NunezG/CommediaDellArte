using UnityEngine;
using System.Collections;

public class CoffreScript : MonoBehaviour {

	public GameManager gameManager;

	public AudioClip juggle_sound;

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
	public void talk(int type){
		StartCoroutine (talkCoroutine (type));
	}
	public void touch(int type){
		StartCoroutine (touchCoroutine (type));
	}


	IEnumerator juggleCoroutine(){

		gameManager.guiManager.active = false;

		Vector3 moveToChestEvent = new Vector3 (13, 7, 30);
        gameManager.getCharacterGameobject("Arlequin").GetComponent<CharacterController>().goTo(moveToChestEvent);
        while (gameManager.getCharacterGameobject("Arlequin").transform.position != moveToChestEvent)
        {
			yield return null;
		}

		//animation prendre;
        gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("take");
        yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length - 0.1f);
		
		Vector3 goToCenterEvent = new Vector3 (-1.5f, 7, 30);
        gameManager.getCharacterGameobject("Arlequin").GetComponent<CharacterController>().goTo(goToCenterEvent);
        while (gameManager.getCharacterGameobject("Arlequin").transform.position != goToCenterEvent)
        {
			yield return null;
		}

        gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("juggling");
        while (gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).shortNameHash != Animator.StringToHash("juggling"))
        {
			yield return null;
		}

        gameManager.getCharacterGameobject("Arlequin").GetComponent<AudioSource>().PlayOneShot(juggle_sound, 1);

        yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);

		//retour negatif du souffleur
		gameManager.souffleur.giveFeedback (2,1, 0);

        gameManager.getCharacterGameobject("Arlequin").GetComponent<CharacterController>().goTo(moveToChestEvent);
        while (gameManager.getCharacterGameobject("Arlequin").transform.position != moveToChestEvent)
        {
			yield return null;
		}

		//animation reposer
        gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("take");
        yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);


		gameManager.guiManager.active = true;

		yield break;
	}

	IEnumerator eggJuggleCoroutine(){
		
		gameManager.guiManager.active = false;
		
		Vector3 moveEvent = new Vector3 (13, 7, 30);
        gameManager.getCharacterGameobject("Arlequin").GetComponent<CharacterController>().goTo(moveEvent);

        while (gameManager.getCharacterGameobject("Arlequin").transform.position != moveEvent)
        {
			yield return null;
		}

		//animation prendre
        gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("take");
        yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);

        gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("eggJuggling");
        while (gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).shortNameHash != Animator.StringToHash("eggJuggling"))
        {
			yield return null;
		}

	    gameManager.getCharacterGameobject("Arlequin").GetComponent<AudioSource>().PlayOneShot(juggle_sound, 1);
		
        yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);

        gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("brokenEggs");
        yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);

        gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("disappointed");
        gameManager.getCharacterGameobject("Arlequin").GetComponent<AudioSource>().PlayOneShot(gameManager.getCharacterGameobject("Arlequin").GetComponent<ArlequinScript>().depit, 1);

		yield return new WaitForSeconds (0.2f);
		gameManager.publicOnScene.happy (2, 2);
        yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length - 0.2f);

		gameManager.guiManager.active = true;

        
		//StartCoroutine (gameManager.event2 ());
        gameManager.startEvent("Tutorial_2", gameManager.getEventList());

        //yield return new WaitForSeconds(5f);
        //gameManager.capitaine.GetComponent<Collider2D>().enabled = true;

		yield break;
	}

	IEnumerator talkCoroutine(int type){

		gameManager.guiManager.active = false;
		
		Vector3 moveEvent = new Vector3 (13, 7, 30);
        gameManager.getCharacterGameobject("Arlequin").GetComponent<CharacterController>().goTo(moveEvent);

        while (gameManager.getCharacterGameobject("Arlequin").transform.position != moveEvent)
        {
			yield return null;
		}

        gameManager.getCharacterGameobject("Arlequin").transform.Rotate(0, 180, 0);

		if(type == 0)
            gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("niceTalking");
		else if (type == 1)
            gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("angryTalking");


        yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length / 2);
		gameManager.souffleur.saySomething ("Mais que fais-tu ? Ce coffre n'est pas un comedien !", 0 , false);

		while (gameManager.souffleur.souffleurArray[0].talking == true) {
			yield return null;
		}

        gameManager.getCharacterGameobject("Arlequin").transform.Rotate(0, 180, 0);


		gameManager.guiManager.active = true;

		yield break;
	}

	IEnumerator touchCoroutine(int type){
		
		gameManager.guiManager.active = false;
		
		Vector3 moveEvent = new Vector3 (13, 7, 30);
        gameManager.getCharacterGameobject("Arlequin").GetComponent<CharacterController>().goTo(moveEvent);

        while (gameManager.getCharacterGameobject("Arlequin").transform.position != moveEvent)
        {
			yield return null;
		}

        gameManager.getCharacterGameobject("Arlequin").transform.Rotate(0, 180, 0);
		if (type == 0)
            gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("poke");
		else if (type == 1) {
            gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("frappe");
            yield return new WaitForSeconds(0.5f);
            gameManager.getCharacterGameobject("Arlequin").GetComponent<SoundController>().playSound("Frappe");
		}


        yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length / 2);
		gameManager.souffleur.saySomething ("Mais que fais-tu ? Ce coffre ne t'as rien fait !",0, false);

        while (gameManager.souffleur.souffleurArray[0].talking == true)
        {
			yield return null;
		}
        gameManager.getCharacterGameobject("Arlequin").transform.Rotate(0, 180, 0);

		gameManager.guiManager.active = true;
		
		yield break;
	}



}



using UnityEngine;
using System.Collections;

public class CapitaineScript : MonoBehaviour {

	public GameManager gameManager;
	public AudioClip moquerie;

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
        gameManager.getCharacterGameobject("Arlequin").GetComponent<CharacterController>().goTo(moveEvent);

        while (gameManager.getCharacterGameobject("Arlequin").transform.position != moveEvent)
        {
			yield return null;
		}

		if(type == 0)
            gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("niceTalking");
		else if (type == 1)
            gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("angryTalking");


        yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);
				
		if (type == 1) {			
			this.GetComponent<Collider2D>().enabled = false;
			this.GetComponent<CharacterController> ().sprite.SetTrigger ("peur");
			//feed back du souffleur
			gameManager.souffleur.giveFeedback (2,0, 0);

			if (!talkDone) {
				scaryValue += 50;
				gameManager.publicOnScene.addValue (20);
				talkDone = true;
			}
            yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.3f);
			this.GetComponent<Collider2D>().enabled = true;
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
        gameManager.getCharacterGameobject("Arlequin").GetComponent<CharacterController>().goTo(moveEvent);

        while (gameManager.getCharacterGameobject("Arlequin").transform.position != moveEvent)
        {
			yield return null;
		}
		
		if (type == 0)
            gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("poke");
		else if (type == 1) {
            gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("frappe");
            gameManager.getCharacterGameobject("Arlequin").GetComponent<AudioSource>().PlayOneShot(gameManager.getCharacterGameobject("Arlequin").GetComponent<ArlequinScript>().coup, 1);
		}

        yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);	


		if (type == 1) {
			this.GetComponent<Collider2D>().enabled = false;
            gameManager.getCharacterGameobject("Capitaine").GetComponent<CharacterController>().sprite.SetTrigger("peur");
			//feed back du souffleur
			gameManager.souffleur.giveFeedback (2,0, 0);
			
			if (!touchDone) {
				scaryValue += 50;
				gameManager.publicOnScene.addValue (20);
				touchDone = true;
			}
            yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.3f);
			this.GetComponent<Collider2D>().enabled = true;

		}

		gameManager.guiManager.active = true;

		if (scaryValue >= 100) {
			StartCoroutine(goAway());
		}

		yield break;
	}

	IEnumerator goAway(){

		Vector3 moveEvent = new Vector3 (-20, 7, 50);

		this.GetComponent<Collider2D> ().enabled = false;

		this.GetComponent<CharacterController>().goTo(moveEvent);
		yield return new WaitForSeconds(1.5f);

		//desactiver le capitaine

		gameManager.publicOnScene.addValue(20);

        StartCoroutine( gameManager.startEventCoroutine("Tutorial_3", gameManager.getEventList(), gameManager.GameAsset));

		yield break;
	}
}

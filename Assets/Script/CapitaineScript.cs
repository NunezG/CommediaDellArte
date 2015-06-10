using UnityEngine;
using System.Collections;

public class CapitaineScript : MonoBehaviour {

	public GameManager gameManager;
	public AudioClip moquerie;

	private int scaryValue = 0;
	private bool talkDone = false, touchDone = false;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void capitaineEvent(string eventName)
    {

        if (!talkDone && eventName == "parler_mechamment")
        {
            scaryValue++;
        }
        else if (!touchDone && eventName == "toucher_mechamment")
        {
            scaryValue++;
        }


        if (scaryValue >= 2)
        {
           StartCoroutine( goAwayCoroutine(eventName));
        }
        else
        {
            XmlManager.launchEvent(eventName, "capitaine_tuto");
        }
  
       
    }

    private IEnumerator goAwayCoroutine(string eventName)
    {
        yield return StartCoroutine(XmlManager.launchEventCoroutine(eventName, "capitaine_tuto"));

        yield return StartCoroutine(XmlManager.launchEventCoroutine("fuite", "capitaine_tuto"));

        XmlManager.launchEvent("Introduction", "scene_1");

        yield break;
    }


	public void talk(int type){
		StartCoroutine (talkCoroutine (type));
	}
	public void touch(int type){
		StartCoroutine (touchCoroutine (type));
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

        if (type == 1)
        {
            this.GetComponent<Collider2D>().enabled = false;
            this.GetComponent<CharacterController>().sprite.SetTrigger("peur");
            //feed back du souffleur

            gameManager.souffleur.giveFeedback(2, 0, 0);

            if (!talkDone)
            {
                scaryValue += 50;
                gameManager.publicOnScene.addValue(20);
                talkDone = true;
            }
            yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.3f);
            this.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            gameManager.souffleur.giveFeedback(2, 1, 0);
        }

		gameManager.guiManager.active = true;

		if (scaryValue >= 100) {
            yield return new WaitForSeconds(2);
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
            yield return new WaitForSeconds(0.5f);
            gameManager.getCharacterGameobject("Arlequin").GetComponent<SoundController>().playSound("Frappe");
		}

        yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);


        if (type == 1)
        {
            this.GetComponent<Collider2D>().enabled = false;
            gameManager.getCharacterGameobject("Capitaine").GetComponent<CharacterController>().sprite.SetTrigger("peur");
            //feed back du souffleur
            gameManager.souffleur.giveFeedback(2, 0, 0);

            if (!touchDone)
            {
                scaryValue += 50;
                gameManager.publicOnScene.addValue(20);
                touchDone = true;
            }
            yield return new WaitForSeconds(gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.3f);
            this.GetComponent<Collider2D>().enabled = true;

        }
        else
        {
            gameManager.souffleur.giveFeedback(2, 1, 0);

        }

       


		gameManager.guiManager.active = true;

		if (scaryValue >= 100) {
            yield return new WaitForSeconds(2);
			StartCoroutine(goAway());
		}

		yield break;
	}

	IEnumerator goAway(){

		Vector3 moveEvent = new Vector3 (-16.7f, -2.2f, -10.5f);
        Vector3 moveEvent2 = new Vector3 (-16.7f, -2.2f, 23.32f);

		this.GetComponent<Collider2D> ().enabled = false;

		this.GetComponent<CharacterController>().goTo(moveEvent2);

		yield return new WaitForSeconds(1f);
        this.GetComponent<CharacterController>().goTo(moveEvent);
		//desactiver le capitaine

		gameManager.publicOnScene.addValue(20);

        StartCoroutine( gameManager.startEventCoroutine("Tutorial_3", gameManager.getEventList(), gameManager.GameAsset));

		yield break;
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.IO;

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

    public TextAsset GameAsset;

    private List<Character> characterList;
    private List<IEnumerator> eventTest;

    private bool nextAction = false;

	bool test = false;
	// Use this for initialization
	void Start () {
        characterList = new List<Character>();
        eventTest = new List<IEnumerator>();

        characterList.Add(new Character(character.gameObject, "Arlequin"));
        characterList.Add(new Character(capitaine.gameObject, "Capitaine"));
        characterList.Add(new Character(pantalone.gameObject, "Pantalone"));
        characterList.Add(new Character(colombine.gameObject, "Colombine"));
        characterList.Add(new Character(pierrot.gameObject, "Pierrot"));

        loadEvent();
        StartCoroutine( startEvent(0));
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

    
    IEnumerator startEvent(int id)
    {
        nextAction = true;
        int i = 0;

        while (i < eventTest.Count)
        {
            if (nextAction)
            {
                nextAction = false;
                Debug.Log("Execution de l'action n°" + i + ".");
                StartCoroutine(eventTest[i]);
                i++;
            }
            yield return null;
        }
        yield break;
    }

    //permet de recuperer le gameobject d'un personnage a partir de son nom
    private GameObject getCharacterGameobject(string name){
        
        for (int i = 0; i < characterList.Count; i++){
            Debug.Log("research : " + name + " , actually this is :" + characterList[i]._characterName);
            if (name == characterList[i]._characterName){
                Debug.Log("Found");
                return characterList[i]._characterGameobject;
            }
        }
        Debug.Log("Not Found");
        return null;
    }

    //Chargment deun event a partir d'un fichier xml
    void loadEvent()
    {

        XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
        xmlDoc.LoadXml(GameAsset.text); // load the file.

        XmlNodeList eventList = xmlDoc.GetElementsByTagName("event"); // liste des evenements

        Debug.Log("Il y a " + eventList.Count + " node d'evenements a charger.");

        //test avec le premier event
        XmlNodeList actionsList = eventList[0].ChildNodes;
        Debug.Log("Il y a " + actionsList.Count + " node d'actions a charger dans l'event n° 0");

        for (int i = 0; i < actionsList.Count; i++)
        {
            Debug.Log(" nom de la node n°" + i + " :" + actionsList.Item(i).Name);
            //action des personnages
            if (actionsList.Item(i).Name == "character")
            {
                Debug.Log("On cible le personnage " + actionsList.Item(i).Attributes["name"].Value + ".");
                XmlNodeList characterActionsList = actionsList.Item(i).ChildNodes;
                for (int j = 0; j < characterActionsList.Count; j++)
                {
                    if (characterActionsList.Item(j).Name == "deplacement")
                    {
                        Debug.Log("Deplacement du personnage en : " + characterActionsList[j].Attributes["x"].Value + ", " +
                                  characterActionsList[j].Attributes["y"].Value + ", " +
                                  characterActionsList[j].Attributes["z"].Value + " .");
                        IEnumerator action = deplacementCoroutine(actionsList.Item(i).Attributes["name"].Value,
                         new Vector3(
                         float.Parse(characterActionsList[j].Attributes["x"].Value),
                         float.Parse(characterActionsList[j].Attributes["y"].Value),
                         float.Parse(characterActionsList[j].Attributes["z"].Value)));

                        eventTest.Insert(eventTest.Count, action);
                    }
                    else if (characterActionsList.Item(j).Name == "animation")
                    {
                        Debug.Log("Activation de l'animation d'un personnage : " + characterActionsList[j].Attributes["name"].Value);
                        IEnumerator action = animationCoroutine(actionsList.Item(i).Attributes["name"].Value, characterActionsList[j].Attributes["name"].Value);
                        eventTest.Insert(eventTest.Count, action);
                    }
                    else if (characterActionsList.Item(j).Name == "rotation")
                    {
                        Debug.Log("Rotation du personnage de : " + characterActionsList[j].Attributes["x"].Value + ", " +
                                  characterActionsList[j].Attributes["y"].Value + ", " +
                              characterActionsList[j].Attributes["z"].Value + " .");

                        IEnumerator action = rotationCoroutine(actionsList.Item(i).Attributes["name"].Value,
                        new Vector3(
                            float.Parse( characterActionsList[j].Attributes["x"].Value),
                            float.Parse( characterActionsList[j].Attributes["y"].Value),
                            float.Parse( characterActionsList[j].Attributes["z"].Value)));

                        eventTest.Insert(eventTest.Count, action);
                    }
                }
            }
            //activation/desactivation du GUImanager
            if (actionsList.Item(i).Name == "guiManager")
            {
                Debug.Log("Modification du GUIManager en " + actionsList[i].Attributes["active"].Value + ".");
                bool temp = false;
                if (actionsList[i].Attributes["active"].Value == "true" ||
                    actionsList[i].Attributes["active"].Value == "True" ||
                    actionsList[i].Attributes["active"].Value == "Vrai" ||
                    actionsList[i].Attributes["active"].Value == "vrai")
                {
                    temp = true;
                }
                else if (actionsList[i].Attributes["active"].Value == "false" ||
                    actionsList[i].Attributes["active"].Value == "False" ||
                    actionsList[i].Attributes["active"].Value == "Faux" ||
                    actionsList[i].Attributes["active"].Value == "faux")
                {
                    temp = false;
                }
                IEnumerator action = guiCoroutine(temp);
                eventTest.Insert(eventTest.Count, action);
            }
            if (actionsList.Item(i).Name == "wait")
            {

            }
            if (actionsList.Item(i).Name == "fadetoblack")
            {

            }
            //action du souffleur
            if (actionsList.Item(i).Name == "souffleur")
            {
                Debug.Log("On cible le souffleur ");

                XmlNodeList characterActionsList = actionsList.Item(i).ChildNodes;
                for (int j = 0; j < characterActionsList.Count; j++)
                {
                    if (characterActionsList.Item(j).Name == "talk")
                    {
                        Debug.Log("Le souffleur veut dire un truc ");
                        XmlNodeList souffleurText = characterActionsList.Item(j).ChildNodes;
                        List<string> text = new List<string>();
                        for (int k = 0; k < souffleurText.Count; k++)
                        {
                            text.Add(souffleurText[k].InnerText);
                        }
                        IEnumerator action = talkCoroutine(text);
                        eventTest.Insert(eventTest.Count, action);
                    }
                    else if (characterActionsList.Item(j).Name == "feedback")
                    {
                        Debug.Log("Le souffleur envoie un feedback de type : " + characterActionsList[j].Attributes["type"].Value +
                                  " d'une durée de " + characterActionsList[j].Attributes["time"].Value + "s.");
                        IEnumerator action = feedbackCoroutine(characterActionsList[j].Attributes["type"].Value, float.Parse(characterActionsList[j].Attributes["time"].Value));
                        eventTest.Insert(eventTest.Count, action);
                    }
                }
            }
            //action du public
            if (actionsList.Item(i).Name == "public")
            {
                Debug.Log("On cible le public");
                XmlNodeList publicActionsList = actionsList.Item(i).ChildNodes;
                for (int j = 0; j < publicActionsList.Count; j++)
                {
                    if (publicActionsList.Item(j).Name == "laugh")
                    {
                        Debug.Log("Le public rigole pendant "+publicActionsList.Item(j).Attributes["time"].Value+"s.");
                        IEnumerator action = laughCoroutine(float.Parse( publicActionsList.Item(j).Attributes["time"].Value));
                        eventTest.Insert(eventTest.Count, action);
                    }
                }
            }
            //action de la camera
            if(actionsList.Item(i).Name == "camera")
            {
                Debug.Log("On cible la camera");

                XmlNodeList cameraActionsList = actionsList.Item(i).ChildNodes;
                for (int j = 0; j < cameraActionsList.Count; j++)
                {
                    if (cameraActionsList.Item(j).Name == "deplacement")
                    {
                        Debug.Log("Deplacement de la camera en : " + cameraActionsList[j].Attributes["x"].Value + ", " +
                                  cameraActionsList[j].Attributes["y"].Value + ", " +
                                  cameraActionsList[j].Attributes["z"].Value + " .");
                        IEnumerator action = deplacementCameraCoroutine(  new Vector3(
                         float.Parse(cameraActionsList[j].Attributes["x"].Value),
                         float.Parse(cameraActionsList[j].Attributes["y"].Value),
                         float.Parse(cameraActionsList[j].Attributes["z"].Value)));

                        eventTest.Insert(eventTest.Count, action);
                    }
                    if (cameraActionsList.Item(j).Name == "reset")
                    {
                        Debug.Log("Reset de la position de la camera.");
                        IEnumerator action = resetCameraCoroutine();
                        eventTest.Insert(eventTest.Count, action);
                    }
                }
            }
        }
    }

    IEnumerator guiCoroutine(bool active)
    {
        if (active)
        {
            Debug.Log("Activation du GUImanager.");
        }
        else
        {
            Debug.Log("Desactivation du GUImanager.");

        }
        guiManager.active = active;
        nextAction = true;
        yield break;
    }

    IEnumerator deplacementCoroutine(string characterName, Vector3 position)
    {
        Debug.Log("Execution d'un deplacement de " + characterName + " en " + position + ".");
        CharacterController character = getCharacterGameobject(characterName).GetComponent<CharacterController>();
        character.goTo(position);

        while (character.transform.position != position)
        {
            yield return null;
        }
        nextAction = true;
        yield break;
    }

    IEnumerator rotationCoroutine(string characterName, Vector3 rotation) 
    {
        Debug.Log("Execution d'une rotation de " + characterName + " en " + rotation + ".");
        GameObject character = getCharacterGameobject(characterName);

        character.transform.Rotate(rotation);
        nextAction = true;
        yield break;
    }

    IEnumerator animationCoroutine(string characterName, string animationName)
    {
        Debug.Log("Execution d'une animation de " + characterName + " , qui est \"" + animationName + "\".");
        Animator characterAnimator = getCharacterGameobject(characterName).GetComponentInChildren<Animator>();

        characterAnimator.SetTrigger (animationName);
		while(characterAnimator.GetCurrentAnimatorStateInfo (0).shortNameHash !=  Animator.StringToHash(animationName) )
        {
				yield return null;
		}		
		yield return new WaitForSeconds(characterAnimator.GetCurrentAnimatorStateInfo(0).length);
        nextAction = true;
        yield break;
    }

    IEnumerator feedbackCoroutine( string type, float time)
    {
        Debug.Log("Envoie d'un feedback de type :" + type + " , de " + time + "s.");
        if(type == "good")
        {
            souffleur.giveFeedback(time, 0);
        }
        else if(type == "bad")
        {
            souffleur.giveFeedback(time, 1);
        }
        nextAction = true;
        yield break;
    }

    IEnumerator talkCoroutine(List<string> text)
    {
        Debug.Log("Le souffleur parle.");
        souffleur.saySomething(text, false);
        while (souffleur.talking == true)
        {
            yield return null;
        }
        nextAction = true;
        yield break;
    }

    IEnumerator laughCoroutine(float time)
    {
        Debug.Log("Le public rigole.");
        publicOnScene.happy(time);
        nextAction = true;
        yield break;
    }

    IEnumerator deplacementCameraCoroutine(Vector3 position)
    {
        Debug.Log("La camera se deplace en :" + position + ".");
        camera.moveTo(position);
        while ( (position - camera.transform.position).magnitude > 1)
        {
            yield return null;
        }
        nextAction = true;
        yield break;
    }

    IEnumerator resetCameraCoroutine()
    {
        Debug.Log("On reset la position de la camera");
        camera.resetPosition();
        while ((camera.getOriginalPosition() - camera.transform.position).magnitude > 1)
        {
            yield return null;
        }
        nextAction = true;
        yield break;
    }

    IEnumerator waitCoroutine(float time) 
    {
        yield return new WaitForSeconds(time);
        yield break;
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

    //Event avec pierrot
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

class Character
{
    public GameObject _characterGameobject;
    public string _characterName;
    public Character(GameObject g, string s )
    {
        _characterGameobject = g;
        _characterName = s;
    }
}
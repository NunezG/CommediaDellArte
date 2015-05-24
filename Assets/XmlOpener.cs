using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.IO;

public class XmlOpener : MonoBehaviour {
	
	public TextAsset GameAsset;
    
	// Use this for initialization
	void Start () {
	

	}

	// Update is called once per frame
	void Update () {
	
		if(Input.GetButton("Fire1")){
			loadEvent();
		}

	}
	
	void loadEvent(){

		XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
		xmlDoc.LoadXml(GameAsset.text); // load the file.

		XmlNodeList eventList = xmlDoc.GetElementsByTagName("event"); // liste des evenements

		Debug.Log ("Il y a " + eventList.Count + " node d'evenements a charger.");

		//test avec le premier event
		XmlNodeList actionsList = eventList [0].ChildNodes;
		Debug.Log ("Il y a " + actionsList.Count + " node d'actions a charger dans l'event n° 0");

		for (int i = 0; i < actionsList.Count; i++) {
			Debug.Log (" nom de la node n°"+i+" :"+ actionsList.Item(i).Name);

			if(actionsList.Item(i).Name == "character"){
				Debug.Log ("On cible le personnage "+actionsList.Item(i).Attributes["name"].Value+".");

				XmlNodeList characterActionsList = actionsList.Item(i).ChildNodes;
				for (int j = 0; j < characterActionsList.Count; j++) {

					if(characterActionsList.Item(j).Name == "deplacement"){
						Debug.Log("Deplacement du personnage en : " + characterActionsList[j].Attributes["x"].Value + ", "+
						          characterActionsList[j].Attributes["z"].Value +", "+
						          characterActionsList[j].Attributes["z"].Value + " .");
					}
					else if(characterActionsList.Item(j).Name == "animation"){
						Debug.Log("Activation de l'animation d'un personnage : " + characterActionsList[j].Attributes["name"].Value);
					}
					else if(characterActionsList.Item(j).Name == "rotation"){
							Debug.Log("Rotation du personnage de : " + characterActionsList[j].Attributes["x"].Value + ", "+
							          characterActionsList[j].Attributes["z"].Value +", "+
						          characterActionsList[j].Attributes["z"].Value + " .");
                    }
					else if(characterActionsList.Item(j).Name == "talk"){
						Debug.Log("Le personnage veut dire : " + characterActionsList[j].Attributes["text"].Value + ". ");
					}
					else if(characterActionsList.Item(j).Name == "feedback"){
						Debug.Log("Le personnage envoie un feedback de type : " + characterActionsList[j].Attributes["type"].Value +
						          " d'une durée de "+characterActionsList[j].Attributes["time"].Value+"s.");
					}
				}                                                
			}
			if(actionsList.Item(i).Name == "guiManager"){
				Debug.Log("Modification du GUIManager en " + actionsList[i].Attributes["active"].Value+".");
				}  
			}
	}


	IEnumerator deplacementCoroutine(string characterName, Vector3 position){

		Vector3 moveEvent = position;
		CharacterController character =  GameObject.Find (characterName).GetComponent<CharacterController>();
		character.goTo (moveEvent);
		
		while (character.transform.position !=  moveEvent) {
			yield return null;
		}

		yield break;
    }

	IEnumerator rotationCoroutine(string characterName, Vector3 rotation){
		
		Vector3 rotationEvent = rotation;
		GameObject character = GameObject.Find (characterName);

		character.transform.Rotate (rotation);

        yield break;
    }  
    

}

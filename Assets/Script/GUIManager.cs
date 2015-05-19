using UnityEngine;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour {

	public bool active = true;
	public CursorScript cursor;
	public List<ButtonScript> objectList; // listes des objet de la scene avec interactions possibles
	List<ButtonScript> waitingObject = new List<ButtonScript>();

	public AudioClip open;
	public float volumeOpen;
	public AudioClip clicOnAction;
	public float volumeAction;

	private bool cursorAnim = false;

	// Use this for initialization
	void Start () {

		List<ButtonScript> tempObjectList = new List<ButtonScript>();

		for (int i = 0; i< objectList.Count; i++) {
			addRadialMenu(objectList[i].gameObject, tempObjectList);
		}
		objectList.InsertRange (0,tempObjectList);
	}
	
	// Update is called once per frame
	void Update () {

		cursorAnim = false;

		if (active) {

			ButtonScript firstObject = null;
			bool impossibleAction = false;

			//on cherche l'objet survolé le plus hauit dans le layer
			for (int i = 0; i < objectList.Count; i++) {
				if (objectList [i].GetComponent<Collider2D> ().enabled && objectList [i].GetComponent<Collider2D> ().overlapMouse ()) {
					//Gestion du curseur
					cursorAnim = true;
					//gestion des radialButton

					if (objectList [i].transform.GetComponent<RadialButtonScript> () != null) {
						if (objectList [i].transform.GetComponent<RadialButtonScript> ().active == false){
							impossibleAction = true;
							break;
						}
					}
					//gestion du highlight
					if (objectList [i].transform.GetComponentInChildren<Highlight> () != null)
						objectList [i].transform.GetComponentInChildren<Highlight> ().overlap = true;
					//gestion des jauges
					if (objectList [i].transform.GetComponentInChildren<JaugeScript> () != null)
						objectList [i].transform.GetComponentInChildren<JaugeScript> ().show = true;

					firstObject = objectList [i];
					break;
				}

			}

			cursor.setInteractionAnim (cursorAnim);

				
			//Gestion du clic dans le jeu pour chaque objets
			if (Input.GetButtonDown ("Fire1") && !impossibleAction) {
				//update des objets qui attendent un clic
				if (waitingObject.Count > 0) {
					while (waitingObject.Count > 0) {
						if (! waitingObject [0].GetComponent<Collider2D> ().overlapMouse ()) {
							waitingObject [0].GetComponent<ButtonScript> ().onClicAway.Invoke ();
						}
						waitingObject.RemoveAt (0);
					}
				}
				//RAZ des objets en attentes
				waitingObject.Clear ();

				//lancement de l'evenement clic de l'objet survolé
				if (firstObject != null){
					firstObject.onClic.Invoke ();

					if(firstObject.tag.Contains("Action")){
					   this.GetComponent<AudioSource>().PlayOneShot(clicOnAction);
					}
					if(firstObject.GetComponent<RadialMenuScript>() != null){
						this.GetComponent<AudioSource>().PlayOneShot(open);
					}
				}
			}
		}
	}

	
	public void addWaitingObject( RadialButtonScript button){
		waitingObject.Add(button.GetComponent<ButtonScript>());
	}
	public void addWaitingObject(RadialMenuScript radialMenu){
		addWaitingObject(radialMenu.buttonList);
	}
	public void addWaitingObject(RadialButtonScript[] buttonList){
		for (int i = 0; i< buttonList.Length; i++) {
			addWaitingObject(buttonList[i]);
		}
	}
	
	private void addRadialMenu(GameObject gameobject, List<ButtonScript> list){

		RadialMenuScript radialMenu = gameobject.GetComponent<RadialMenuScript>();

		if (radialMenu != null) {
			int lenght = radialMenu.buttonList.Length;
			for( int i = 0; i < lenght; i++){
				list.Add( radialMenu.buttonList[i].GetComponent<ButtonScript>());
				addRadialMenu(radialMenu.buttonList[i].gameObject, list);
			}
		} 
		else {
			return;
		}
	}

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (Collider2D))]


public class RadialMenuScript : MonoBehaviour {



	//Liste des actions possibles pour cet objet
	public RadialButtonScript[] buttonList;
	//distance des buttons sur le cercle par rapport au centre de l'objet
	public float radius = 2;
	//angle de repartition des buttons
	public float startAngle, endAngle;

	//private GameObject[] buttonList;
	private bool show = false;


    void Start () {
		createRadialMenu ( buttonList);
	}
	
	void Update () {}

	public void openMenu(){
		for (int i = 0; i < buttonList.Length; i ++) {
			buttonList[i].GetComponent<RadialButtonScript>().setDisplay(true);
		}
	}

	public void closeMenu(){		
		for (int i = 0; i < buttonList.Length; i ++) {
			buttonList[i].GetComponent<RadialButtonScript>().setDisplay(false);
		}
	}


	//useless
	/*
	public void displayManagement(){
		
		if (!show) {
			
			show = true;
			
			for (int i = 0; i < buttonList.Length; i ++) {
				buttonList[i].GetComponent<RadialButtonScript>().setDisplay(true);
			}		
			
			this.transform.GetChild(0).GetComponent<Highlight>().disable(); // desactivation du highlight de l'objet
			
			//desactivation du menu parent  si il y en a un
			if (this.transform.parent != null) {
				this.transform.parent.GetComponent<Collider2D>().enabled = false;
				RadialMenuScript parentRadialMenu = this.transform.parent.GetComponent<RadialMenuScript> ();
				if (parentRadialMenu != null) {
					parentRadialMenu.onlyDisplay (this.GetComponent<RadialButtonScript>().index);
					
				}
			}
		} 
		else {
			
			show = false;
			
			for (int i = 0; i < buttonList.Length; i ++) {
				buttonList[i].GetComponent<RadialButtonScript>().setDisplay(false);
			}
			
			this.transform.GetChild(0).GetComponent<Highlight>().enable(); // activation du highlight de l'objet
			
			//activation du menu parent si il y en a un
			if (this.transform.parent != null) {
				this.transform.parent.GetComponent<Collider2D>().enabled = true;
				RadialMenuScript parentRadialMenu = this.transform.parent.GetComponent<RadialMenuScript> ();
				if (parentRadialMenu != null) {
					parentRadialMenu.setDisplay (true);
				}
			}
		}		
		
	}
	*/


	//gere l'affichage de tout les buttons  
	void setDisplay(bool display){
		for (int i = 0; i < buttonList.Length; i ++) {
				buttonList[i].GetComponent<RadialButtonScript>().setDisplay(display);
		}
	}
	//affiche seulement un button
	void onlyDisplay(int index){
		for (int i = 0; i < buttonList.Length; i ++) {
			if(index != i)
				buttonList[i].GetComponent<RadialButtonScript>().setDisplay(false);
		}
	}

	//desactive ou active les collider d'un button
	void disableButtonCollider(int index){
			buttonList[index].GetComponent<Collider2D> ().enabled = false;
	}
	void enableButtonCollider(int index){
			buttonList[index].GetComponent<Collider2D> ().enabled = true;
	}	
	//desactive ou active les collider des buttons
	void disableButtonsCollider(){
		for (int i = 0; i < buttonList.Length; i ++) {
			buttonList[i].GetComponent<Collider2D> ().enabled = false;
		}
	}
	void enableButtonsCollider(){
		for (int i = 0; i < buttonList.Length; i ++) {
			buttonList[i].GetComponent<Collider2D> ().enabled = true;
		}
	}


	//initialise le menu avec un alpha de 0
	public void createRadialMenu(RadialButtonScript[] buttonList){
		int i;
		float circlePos = startAngle * Mathf.Deg2Rad;
		Vector3 buttonOffset = new Vector3(Mathf.Cos(circlePos)*radius, Mathf.Sin(circlePos) * radius,0);
		
		for (i = 0; i < buttonList.Length; i ++) {
			
			buttonList[i].transform.position = this.transform.position + buttonOffset;
			buttonList[i].parentObject = this;
			buttonList[i].positionOffset = buttonList[i].transform.position - this.transform.position;

			circlePos += ((endAngle-startAngle) * Mathf.Deg2Rad)/buttonList.Length;
			buttonOffset = new Vector3(Mathf.Cos(circlePos)*radius, Mathf.Sin(circlePos) * radius,0);
			
			buttonList[i].GetComponent<Collider2D>().enabled = false;
			buttonList[i].GetComponent<RadialButtonScript>().index = i;
			
		}
	}


}

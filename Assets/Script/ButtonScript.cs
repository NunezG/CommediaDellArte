using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent (typeof (Collider2D))]

public class ButtonScript : MonoBehaviour {


	//Event lancé lors du clic
	public UnityEvent onClic;
	//Event lancé si l'action attend un clic et que le joueur clic ailleur au prochain clic
	public UnityEvent onClicAway;


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Fire1")) {
			if( this.GetComponent<Collider2D>().overlapMouse()){
				//onClic.Invoke ();
			}
		}
	
	}




}



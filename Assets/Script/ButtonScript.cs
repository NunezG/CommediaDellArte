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
	//Event lancé si on survol le button
	public UnityEvent onOverlapEnter,onOverlap,onOverlapExit;

	private bool overlap = false;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	}

	public void overlapEvent(){

		if (!overlap) {
			onOverlapEnter.Invoke();
			overlap = true;
		} 
		else {
			onOverlap.Invoke();
		}
	}
	
	public void overlapExitEvent(){
		onOverlapExit.Invoke ();
		overlap = false;
	}

}



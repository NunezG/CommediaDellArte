using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Reflection;
using System;


public class EventScript : MonoBehaviour {



	public UnityEvent sastisfiedEvent;// event lors de la satisfaction des conditions
	public bool test = true;

	public List<LogicalTest> logicalTestList; // listes des conditions pour effectuer l'action
	public List<UnityEngine.Object> objectList;//object analysé pour recuperer les composants
	public List<int> componentIndexList, propertyIndexList; //liste des index des choix des composant et attributs
	public Component[] componentList;  // array pour stocker les composant de l'objet
	public FieldInfo[] fieldList;// array pour stocker les attributs du composant

	public List<String> comparisonTestList;

	private bool eventDone = false;// permet d'effectuer une seule fois l'event
	
	// Use this for initialization
	void Start () {

		for (int i = 0; i < logicalTestList.Count; i++) {
			Debug.Log(( logicalTestList[i]).getTestType() );
		}
		for (int i = 0; i < objectList.Count; i++) {
			Debug.Log( objectList[i].name);
		}
	}
	
	// Update is called once per frame
	void Update () {

		//test = !test;
		Debug.Log ("test value : " + test);

		if (!eventDone) {
			for (int i = 0; i < logicalTestList.Count; i++) {
				if (!logicalTestList [i].checkTest ()) 
					return;
			}
			sastisfiedEvent.Invoke ();
			//eventDone = true;
		}


	}

	public void ayyLMAO(){
		Debug.Log ("Ayy LMAO");
	}
}



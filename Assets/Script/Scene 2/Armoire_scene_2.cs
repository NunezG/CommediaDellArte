﻿using UnityEngine;
using System.Collections;

public class Armoire_scene_2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void armoireEvent(string eventName){
		XmlManager.launchEvent(eventName, "armoire_scene_2");
	}
}

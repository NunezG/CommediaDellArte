using UnityEngine;
using System.Collections;

public class colombine_scene_2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void colombineEvent(string eventName){
		XmlManager.launchEvent(eventName, "colombine_scene_2");
	}
}

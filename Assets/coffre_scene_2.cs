using UnityEngine;
using System.Collections;

public class coffre_scene_2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void coffreEvent(string eventName){
		XmlManager.launchEvent(eventName, "coffre_scene_2");
	}
}

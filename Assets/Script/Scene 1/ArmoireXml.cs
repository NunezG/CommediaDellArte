using UnityEngine;
using System.Collections;

public class ArmoireXml : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void ArmoireEvent(string eventName)
    {

     XmlManager.launchEvent(eventName, "armoire");
        
    }
}

using UnityEngine;
using System.Collections;

public class VaseXmlScript : MonoBehaviour {

    public int num = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void VaseEvent(string eventName)
    {
        if(num == 0)
             XmlManager.launchEvent(eventName, "vase0");
        else if (num == 1)
            XmlManager.launchEvent(eventName, "vase1");

    }
}

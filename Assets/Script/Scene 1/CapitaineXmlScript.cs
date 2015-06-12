using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CapitaineXmlScript : MonoBehaviour
{

    public int scaryValue = 0; 
    private int _touchCount = 0, _talkCount = 0;

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void capitaineEvent(string eventName)
    {

        if (eventName == "toucher_gentiment")
        {
            _touchCount++;
            if (_touchCount > 1)
            {
                XmlManager.launchEvent("toucher_gentiment_2", "capitaine");
                this.GetComponent<RadialMenuScript>().buttonList[1].GetComponent<RadialMenuScript>().buttonList[0].setActive(false);
            }
            else
            {
                XmlManager.launchEvent("toucher_gentiment", "capitaine");
            }
        }
        else if (eventName == "parler_moqueur")
        {
            _talkCount++;
            if (_talkCount > 1)
            {
                XmlManager.launchEvent("parler_moqueur_2", "capitaine");
                this.GetComponent<RadialMenuScript>().buttonList[1].GetComponent<RadialMenuScript>().buttonList[2].setActive(false);
            }
            else
            {
                XmlManager.launchEvent("parler_moqueur", "capitaine");
            }
        }
        else
        {
            XmlManager.launchEvent(eventName, "capitaine");
        }
    }

    public void goAway()
    {
        LoadingScreen.instance.loadLevel("Scene 2 - Colombine");
    }
}
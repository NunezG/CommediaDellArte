using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CapitaineXmlScript : MonoBehaviour
{

    private int _touchCount = 0;


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
            }
            else
            {
                XmlManager.launchEvent("toucher_gentiment", "capitaine");
            }
        }
        else
        {
            XmlManager.launchEvent(eventName, "capitaine");
        }
    }
}
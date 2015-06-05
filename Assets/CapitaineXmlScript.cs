using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CapitaineXmlScript : MonoBehaviour
{


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
        XmlManager.launchEvent(eventName, "capitaine");

    }
}
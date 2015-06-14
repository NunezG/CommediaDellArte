using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class PantaloneChaiseScript : MonoBehaviour {


    public TextAsset XmlDoc;
    public GameManager gamemanager;
    private List<Evenement> eventList;



	// Use this for initialization
	void Start () {
        eventList = gamemanager.loadEvent(XmlDoc);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void pantaloneEvent(string eventName)
    {
        StartCoroutine( pantaloneEventCoroutine( eventName));
    }


    public IEnumerator pantaloneEventCoroutine(string eventName){

        if (eventName == "soulever_mechamment")
        {
             yield return StartCoroutine( XmlManager.launchEventCoroutine("soulever_mechamment", "pantalone"));
            CapitaineXmlScript.scaryValue += 100;
            if (CapitaineXmlScript.scaryValue >= 100)
            {
                yield return StartCoroutine(XmlManager.launchEventCoroutine("fuite", "capitaine"));
                CapitaineXmlScript.goAway();
            }
        }
        else
        {
            XmlManager.launchEvent(eventName, "pantalone");
        }
        yield break;
    }





    public void parlerEvent()
    {
        StartCoroutine(gamemanager.startEventCoroutine("parler_gentiment", eventList, XmlDoc));
        //StartCoroutine(questionCoroutine());
    }


    public void questionEvent()
    {
        StartCoroutine(questionCoroutine());
    }

    IEnumerator questionCoroutine()
    {

        yield return StartCoroutine(gamemanager.startEventCoroutine("question", eventList, XmlDoc));

        //explication de la scene

        yield break;
    }

}

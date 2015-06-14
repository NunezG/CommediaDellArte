using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CapitaineXmlScript : MonoBehaviour
{

    public static int scaryValue = 0; 
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
        StartCoroutine(capitaineEventCoroutine(eventName));
    }



    public IEnumerator capitaineEventCoroutine(string eventName)
    {

        if (eventName == "toucher_gentiment")
        {
            _touchCount++;
            if (_touchCount > 1)
            {
                yield return StartCoroutine(XmlManager.launchEventCoroutine("toucher_gentiment_2", "capitaine"));
                this.GetComponent<RadialMenuScript>().buttonList[1].GetComponent<RadialMenuScript>().buttonList[0].setActive(false);
                scaryValue += 0;
            }
            else
            {
                yield return StartCoroutine(XmlManager.launchEventCoroutine("toucher_gentiment", "capitaine"));
                scaryValue += 0;
            }
        }
        else if (eventName == "parler_moqueur")
        {
            _talkCount++;
            if (_talkCount > 1)
            {
                yield return StartCoroutine(XmlManager.launchEventCoroutine("parler_moqueur_2", "capitaine"));
                this.GetComponent<RadialMenuScript>().buttonList[1].GetComponent<RadialMenuScript>().buttonList[2].setActive(false);
                scaryValue += 0;
            }
            else
            {
                yield return StartCoroutine(XmlManager.launchEventCoroutine("parler_moqueur", "capitaine"));

            }
        }
        else if (eventName == "parler_mechamment")
        {
            yield return StartCoroutine(XmlManager.launchEventCoroutine("parler_mechamment", "capitaine"));
            scaryValue += 0;
        }
        else if (eventName == "parler_gentiment")
        {
            yield return StartCoroutine(XmlManager.launchEventCoroutine("parler_gentiment", "capitaine"));
            scaryValue += 0;
        }
        else if (eventName == "toucher_mechamment")
        {
            yield return StartCoroutine(XmlManager.launchEventCoroutine("toucher_mechamment", "capitaine"));
            scaryValue += 0;
        }
        else if (eventName == "faire_peur")
        {
            yield return StartCoroutine(XmlManager.launchEventCoroutine("faire_peur", "capitaine"));
            scaryValue += 50;
        }

        if (scaryValue >= 100)
        {
            yield return StartCoroutine(XmlManager.launchEventCoroutine("fuite", "capitaine"));
            goAway();
        }
        yield break;
    }

    public static void goAway()
    {
        LoadingScreen.instance.loadLevel("Scene 2 - Colombine");
    }
}
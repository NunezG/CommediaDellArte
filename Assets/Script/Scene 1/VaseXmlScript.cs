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

        StartCoroutine(vaseCoroutine(eventName, num));


    }


    public IEnumerator vaseCoroutine(string eventName, int vaseNum)
    {
        string target = null;

        if (num == 0)
            target = "vase0";
        else if (num == 1)
            target = "vase1";


        if (eventName == "briser")
        {
            yield return StartCoroutine(XmlManager.launchEventCoroutine("briser", target));
            CapitaineXmlScript.scaryValue += 20;
            if (CapitaineXmlScript.scaryValue >= 100)
            {
                yield return StartCoroutine(XmlManager.launchEventCoroutine("fuite", "capitaine"));
                CapitaineXmlScript.goAway();
            }
        }
        else 
        {
            yield return StartCoroutine(XmlManager.launchEventCoroutine(eventName, target));   
        }
        yield break;
    }
}

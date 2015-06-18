using UnityEngine;
using System.Collections;

public class Armoire_scene_2 : MonoBehaviour {


    public endScene endScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void armoireEvent(string eventName)
    {
        StartCoroutine(armoireEventCoroutine(eventName));
    }

    public IEnumerator armoireEventCoroutine(string eventName)
    {

        yield return StartCoroutine(XmlManager.launchEventCoroutine(eventName, "armoire_scene_2"));
        endScript.gameObject.SetActive(true);
        endScript.playEndScene(2);
    }
}

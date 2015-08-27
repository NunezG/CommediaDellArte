using UnityEngine;
using System.Collections;

public class coffre_scene_2 : MonoBehaviour {


    public endScene endScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void coffreEvent(string eventName)
    {
        StartCoroutine(coffreEventCoroutine(eventName));
    }

	public IEnumerator coffreEventCoroutine(string eventName){

		yield return StartCoroutine( XmlManager.launchEventCoroutine(eventName, "coffre_scene_2"));
        endScript.gameObject.SetActive(true);
        endScript.playEndScene(1);
	}
}

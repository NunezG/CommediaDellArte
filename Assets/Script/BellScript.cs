using UnityEngine;
using System.Collections;

public class BellScript : MonoBehaviour {

	public AudioClip sound;

	// Use this for initialization
	void Start () {	

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ring(){
		this.GetComponent<AudioSource> ().PlayOneShot (sound);
        XmlManager.launchEvent("lazzi", "cloche");
	}
}

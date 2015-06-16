using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(AudioSource))]
public class PlayMovie : MonoBehaviour {

	public MovieTexture movie;

	void Start () {
		GetComponent<Renderer>().material.mainTexture = movie;
		GetComponent<AudioSource>().clip = movie.audioClip;
		movie.Play();
		movie.loop = true;

		GetComponent<AudioSource>().Play();
	}
	void Update(){
	}
}
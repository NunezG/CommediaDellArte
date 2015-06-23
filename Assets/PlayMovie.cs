using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(AudioSource))]
public class PlayMovie : MonoBehaviour {

	public MovieTexture movie;
    public float duration;
    public string scene;

	void Start () {
		GetComponent<Renderer>().material.mainTexture = movie;
		GetComponent<AudioSource>().clip = movie.audioClip;
		movie.Play();
		//movie.loop = true;
		GetComponent<AudioSource>().Play();
        StartCoroutine(waitBeforeLoad());

	}

	void Update(){

	}

    IEnumerator waitBeforeLoad()
    {
        yield return new WaitForSeconds(duration);
        LoadingScreen.instance.loadLevel(scene);
    }
}
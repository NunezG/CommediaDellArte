using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(AudioSource))]
public class PlayMovie : MonoBehaviour {

	public MovieTexture movie;
    public float duration;
    public string scene;
	private IEnumerator coroutine;
	private bool tooLate = false;

	void Start () {
		GetComponent<Renderer>().material.mainTexture = movie;
		GetComponent<AudioSource>().clip = movie.audioClip;
		movie.Play();
		//movie.loop = true;
		GetComponent<AudioSource>().Play();
		coroutine = waitBeforeLoad ();
		StartCoroutine(coroutine);

	}

	void Update(){
		if(Input.GetButtonDown("Fire1") && !tooLate){
			StopCoroutine(coroutine);
			GetComponent<AudioSource>().Stop();
			LoadingScreen.instance.loadLevel(scene);
		}
	}

    IEnumerator waitBeforeLoad()
    {
        yield return new WaitForSeconds(duration);
			tooLate = true;
		GetComponent<AudioSource>().Stop();
        LoadingScreen.instance.loadLevel(scene);
    }
}
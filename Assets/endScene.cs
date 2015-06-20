using UnityEngine;
using System.Collections;

public class endScene : MonoBehaviour {

    public GameObject temp;
    public MovieTexture _end1, _end2;
    public float _duration;
    public string _scene;

    void Start()
    {
       

    }

    void Update()
    {

    }

    public void playEndScene(int scene)
    {
        ThemePlayerScript.instance.GetComponent<AudioSource>().Stop();
        Camera.main.transform.Rotate(0,180,0);
        MovieTexture movie = null;
        if(scene == 1)
        {
            movie = _end1;
        }
        else if (scene == 2)
        {
            movie = _end2;
        }

        GetComponent<Renderer>().material.mainTexture = movie;
        GetComponent<AudioSource>().clip = movie.audioClip;
        movie.Play();
        GetComponent<AudioSource>().Play();
        StartCoroutine(waitBeforeLoad());
    }

    IEnumerator waitBeforeLoad()
    {
        yield return new WaitForSeconds(_duration);
        LoadingScreen.instance.loadLevel(_scene);
    }
}

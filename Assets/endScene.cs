using UnityEngine;
using System.Collections;

public class endScene : MonoBehaviour {


    public GUIManager gui;
    public GameObject credit, cursor;
    public MovieTexture _end1, _end2, compteRendu;
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
        ThemePlayerScript.instance.resetList();
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

        Destroy(GameObject.Find("MANAGER"));
        Destroy(GameObject.Find("CANVAS"));
        Destroy(GameObject.Find("PUBLIC"));
        Destroy(GameObject.Find("SCENE"));
        Destroy(GameObject.Find("WALKER Background"));

        MovieTexture movie = compteRendu; 
        GetComponent<Renderer>().material.mainTexture = movie;
        GetComponent<AudioSource>().clip = movie.audioClip;
        movie.Play();
        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(25);

        ThemePlayerScript.instance.addMusic("Commedia dell'Arte Theme Principal", -1);
		
        cursor.SetActive(true);
        credit.SetActive(true);
        credit.GetComponent<CreditScript>().appear();
        LoadingScreen.instance.loadLevel("Menu");
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SouffleurManager : MonoBehaviour {

    public SouffleurScript[] souffleurArray;
    public static int talking = 0;
    public Image UIimage, UICursor;
    public Text UIText;
    public float fadeSpeed = 4, maxAlpha = 1, minAlpha = 0;

    /*Position
    milieu :0
    haut = 1
    droite = 2 
    gauche = 3   
      */

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (talking > 0)
        {
            UIimage.color = new Color(1, 1, 1, Mathf.Lerp(UIimage.color.a, maxAlpha, fadeSpeed * Time.deltaTime));
            UIText.color = new Color(0, 0, 0, Mathf.Lerp(UIimage.color.a, maxAlpha, fadeSpeed * Time.deltaTime));
        }
        else
        {
            UIimage.color = new Color(1, 1, 1, Mathf.Lerp(UIimage.color.a, minAlpha, fadeSpeed * Time.deltaTime));
            UIText.color = new Color(0, 0, 0, Mathf.Lerp(UIimage.color.a, minAlpha, fadeSpeed * Time.deltaTime));
        }
	

	}


    public void giveFeedback(float time, int imageIndex, int souffleurIndex)
    {
        souffleurArray[souffleurIndex].giveFeedback(time, imageIndex);
    }

    public void giveFeedback(float time, string text, int souffleurIndex)
    {
        souffleurArray[souffleurIndex].giveFeedback(time, text);
    }

    public void saySomething(string s, int souffleurIndex, bool reactiveGUI = true)
    {
        souffleurArray[souffleurIndex].saySomething(s, reactiveGUI);
    }

    public void saySomething(List<string> text, int souffleurIndex, bool reactiveGUI = true)
    {
        souffleurArray[souffleurIndex].saySomething(text, reactiveGUI);
    }


}

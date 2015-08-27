using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class fadeStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Image>().color = new Color(1,1,1,1);
	    fadeToBlack(1.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void fadeToBlack(float duration){
		StartCoroutine (fadeCoroutine (duration));
	}


    IEnumerator fadeCoroutine(float fadeSpeed)
    {
        Image _image = this.GetComponent<Image>();

        while (_image.color.a != 0)
        {

            if (_image.color.a <= 0.05)
                _image.color = new Color(0, 0, 0, 0);
            else
                _image.color = new Color(0, 0, 0, Mathf.Lerp(_image.color.a, 0, fadeSpeed * Time.deltaTime));
            yield return null;
        }
        yield break;
    }
			
}

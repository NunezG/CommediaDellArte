using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class fadeAtStart : MonoBehaviour {

    public float fadeSpeed;
    public List<Image> imageList;
    public List<Text> textList;


	// Use this for initialization
	void Start () {
        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].color = new Color(imageList[i].color.r, imageList[i].color.g, imageList[i].color.b,0);
        }
        for (int j = 0; j < textList.Count; j++)
        {
            textList[j].color = new Color(textList[j].color.r, textList[j].color.g, textList[j].color.b, 0);
        }
        StartCoroutine(fade());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void appearMenu()
    {
        StartCoroutine(appear());
    }

    public IEnumerator appear()
    {
        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].color = new Color(imageList[i].color.r, imageList[i].color.g, imageList[i].color.b, 0);
        }
        for (int j = 0; j < textList.Count; j++)
        {
            textList[j].color = new Color(textList[j].color.r, textList[j].color.g, textList[j].color.b, 0);
        }

        while (imageList[0].color.a < 0.95)
        {
            for (int i = 0; i < imageList.Count; i++)
            {
                imageList[i].color = new Color(imageList[i].color.r, imageList[i].color.g, imageList[i].color.b, Mathf.Lerp(imageList[i].color.a, 1, fadeSpeed * Time.deltaTime));
            }
            for (int j = 0; j < textList.Count; j++)
            {
                textList[j].color = new Color(textList[j].color.r, textList[j].color.g, textList[j].color.b, Mathf.Lerp(textList[j].color.a, 1, fadeSpeed * Time.deltaTime));
            }
            yield return null;
        }

        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].color = new Color(imageList[i].color.r, imageList[i].color.g, imageList[i].color.b, 1);
        }
        for (int j = 0; j < textList.Count; j++)
        {
            textList[j].color = new Color(textList[j].color.r, textList[j].color.g, textList[j].color.b, 1);
        }

        yield break;
    }


    IEnumerator fade()
    {

        while (imageList[0].color.a < 0.95)
        {
            for (int i = 0; i < imageList.Count; i++)
            {
                imageList[i].color = new Color(imageList[i].color.r, imageList[i].color.g, imageList[i].color.b, Mathf.Lerp(imageList[i].color.a, 1, fadeSpeed * Time.deltaTime));
            }
            for (int j = 0; j < textList.Count; j++)
            {
                textList[j].color = new Color(textList[j].color.r, textList[j].color.g, textList[j].color.b, Mathf.Lerp(textList[j].color.a, 1, fadeSpeed * Time.deltaTime));
            }
            yield return null;
        }

        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].color = new Color(imageList[i].color.r, imageList[i].color.g, imageList[i].color.b, 1);
        }
        for (int j = 0; j < textList.Count; j++)
        {
            textList[j].color = new Color(textList[j].color.r, textList[j].color.g, textList[j].color.b,1);
        }


        yield break;
    }
}


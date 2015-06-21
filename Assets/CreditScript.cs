using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreditScript : MonoBehaviour {

    public GameObject menuCanvas, creditCanvas;

    public float fadeSpeed;
    public List<Image> imageList;


	// Use this for initialization
	void Start () {
        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].color = new Color(imageList[i].color.r, imageList[i].color.g, imageList[i].color.b, 0);
        }  
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void resumeMenu()
    {
        LoadingScreen.instance.loadLevel("Menu");
    }

    public void resume()
    {
        menuCanvas.gameObject.SetActive(true);
        menuCanvas.GetComponent<MenuPanel>()._fadeObject.appear();
        creditCanvas.SetActive(false);
    }
    public void appear()
    {
        StartCoroutine(fade());
    }

    IEnumerator fade()
    {
        for (int j= 0; j < imageList.Count;j++)
        {
            imageList[j].color = new Color(imageList[j].color.r, imageList[j].color.g, imageList[j].color.b, 0);
        }  
        int i = 0;
        while (i < imageList.Count)
        {
            while (imageList[i].color.a < 0.95f)
            {
               imageList[i].color = new Color(imageList[i].color.r, imageList[i].color.g, imageList[i].color.b, Mathf.Lerp(imageList[i].color.a, 1, fadeSpeed * Time.deltaTime));
               yield return null;
            }

             imageList[i].color = new Color(imageList[i].color.r, imageList[i].color.g, imageList[i].color.b, 1);
             i++;
             yield return null;
        }
        yield break;
    }
}

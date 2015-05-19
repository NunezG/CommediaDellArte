using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    Canvas canvas;
    GUIManager gui;
    bool pause;

    string[] goalList = new string[] {
		"Objectif en cours: distraire le public",
		"Objectif en cours: effrayer le Capitan",
		"Objectif en cours: séduire Colombine" 
};

    void Start()
    {
        gui = GameObject.Find("GUIManager").GetComponent<GUIManager>();
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        pause = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }
    }
    public void OnMouseOver()
    {
        transform.GetComponent<Highlight>().overlap = true;
    }

    public void Pause()
    {
        canvas.enabled = !canvas.enabled;

        if (canvas.enabled)
        {
            gui.active = false;
            transform.GetChild(2).GetComponent<Text>().text = goalList[0];
        }
        else
        {
            gui.active = true;
            transform.GetChild(2).GetComponent<Text>().text = "";
        }
    
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        Camera.main.GetComponent<AudioListener>().enabled = !Camera.main.GetComponent<AudioListener>().enabled;

        pause = !pause;
    }

}
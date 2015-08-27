using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    Canvas canvas;
    GUIManager gui;
    bool pause;

    public string[] goalList = new string[] {
		"Objectif en cours: Apprendre le métier de comédien",
		"Objectif en cours: Effrayer le Capitan",
		"Objectif en cours: Parler du Capitan à Colombina",
		"Objectif en cours: Faire sortir le Capitan de chez Pantalone",
		"Objectif en cours: Aider Colombina à se déguiser pour échapper au Capitan",
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
      /*  if (Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }*/
    }

	public void setGoal(string goal)
	{
		transform.GetChild(2).GetComponent<Text>().text = goal;
	}

	public string getGoal()
	{
		return transform.GetChild(2).GetComponent<Text>().text;
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
            //gui.active = false;
            //setGoal(goalList[0]);
        }
        else
        {
            //gui.active = true;
        }
    
       // Time.timeScale = Time.timeScale == 0 ? 1 : 0;
       // Camera.main.GetComponent<AudioListener>().enabled = !Camera.main.GetComponent<AudioListener>().enabled;

        pause = !pause;
    }

}
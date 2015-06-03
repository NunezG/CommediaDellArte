using UnityEngine;
using System.Collections;

public class MenuPanel : MonoBehaviour {
    Canvas canvas;
    GUIManager gui;
    public Animator anim;
   // public Texture2D curseur;

	// Use this for initialization
	void Start () {

       // Cursor.SetCursor(curseur, Vector2.zero, CursorMode.Auto);

        gui = GameObject.Find("GUIManager").GetComponent<GUIManager>();
        canvas = GetComponent<Canvas>();
        anim.Play("Fermeture");

       // canvas.enabled = !canvas.enabled;
           //  setGoal(goalList[0]);
          //  gui.active = true;
       // Time.timeScale = 0;
	}



    public void startGame()
    {
        anim.Play("Ouverture");

        //GameObject.Find("GameManager").GetComponent<GameManager>().startEvent("Tutorial_1");
        // animator.StopPlayback();
        gameObject.SetActive(false);
    }

    public void restoreGame()
    {
        anim.Play("Ouverture");

        //GameObject.Find("GameManager").GetComponent<GameManager>().startEvent("Tutorial_3");
        gameObject.SetActive(false);

    }

    public void credits()
    {
        anim.Play("Ouverture");

    }

	// Update is called once per frame
	void Update () {
	
	}
}

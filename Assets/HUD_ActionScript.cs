using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD_ActionScript : MonoBehaviour {

	public Text actionName;// text qui correspond au nom de l'action 
	public Image background;
	public float fadeSpeed = 1.0f, timeBeforeFade = 0.0f;

	private float timer = 0;

	private bool show = false;

	// Use this for initialization
	void Start () {
		actionName.color = new Color(actionName.color.r,actionName.color.g,actionName.color.b,0);
		background.color = new Color (background.color.b, background.color.g, background.color.b, 0);
		actionName.text = "None";
	}
			
	// Update is called once per frame
	void Update () {
	
		if (show) {
			actionName.color = new Color(actionName.color.r,actionName.color.g,actionName.color.b,Mathf.Lerp (actionName.color.a, 1, fadeSpeed * Time.deltaTime));
			background.color = new Color(background.color.b,background.color.g,background.color.b,Mathf.Lerp (background.color.a, 1, fadeSpeed * Time.deltaTime));
		}
		else{
			timer += Time.deltaTime;
			if(timer > timeBeforeFade){
				actionName.color = new Color(actionName.color.r,actionName.color.g,actionName.color.b,Mathf.Lerp (actionName.color.a, 0, fadeSpeed * Time.deltaTime));
				background.color = new Color(background.color.b,background.color.g,background.color.b,Mathf.Lerp (background.color.a, 0, fadeSpeed * Time.deltaTime));
			}
		}
	}


	public void displayAction (string text){
		actionName.text = text;
		show = true;
	}

	public void stopDisplay (){
		if (show) {
			show = false;
			timer = 0;
		}
	}

	public void setText(string text){
		actionName.text = text;
	}


	
}

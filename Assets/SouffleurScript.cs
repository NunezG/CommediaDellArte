using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SouffleurScript : MonoBehaviour {
	
	public Image UIimage;
	public Text UIText;	
	public float fadeSpeed = 1, maxAlpha = 1,minAlpha = 0;

	private bool show = true;
	private int index = 0;

	public List<string> textList = new List<string>{

		//1
		"Bienvenue cher comédien ! Comme tu es nouveau dans la troupe, je vais te guider.",
		//2
		"Ton rôle est celui d’Arlecchino, un personnage bien célèbre venu d’Italie. " +
		"C’est un meneur d’intrigue, rusé, spirituel et railleur, qui brouille toujours les pistes.",
		//3
		"C’est également et avant tout un valet qui représente le peuple, qui l’adore. ",
		//4
		"C’est un lourd rôle pour un débutant, j’en ai conscience," +
		"mais notre ancien Arlecchino s’est brisé la jambe lors d’un tour et nous n’avons trouvé personne d’autre." ,
		//5
		"Cependant j’ai bonne foi que tu t’en sortes, avec mes conseils !"};



	void Start () {
		//saySomething ("ayyy");
	}
	
	void Update () {

		if (show) {
			UIimage.color = new Color(1,1,1,Mathf.Lerp(UIimage.color.a, maxAlpha, fadeSpeed * Time.deltaTime));
			UIText.color = new Color(0,0,0,Mathf.Lerp(UIimage.color.a, maxAlpha, fadeSpeed * Time.deltaTime));
		} 
		else {
			UIimage.color = new Color(1,1,1,Mathf.Lerp(UIimage.color.a, minAlpha, fadeSpeed * Time.deltaTime));
			UIText.color = new Color(0,0,0,Mathf.Lerp(UIimage.color.a, minAlpha, fadeSpeed * Time.deltaTime));
		}
	
		if(Input.GetButtonDown("Fire1") && show == true){
			updateText();
		}	
	}

	public void saySomething(string text){
		UIText.text = text;
	}

	public void disappear(){
		show = false;
	}
	public void appear(){
		show = true;
	}


	private void updateText(){

		if(index == textList.Count){
			disappear();
			index = 0;
			return;
		}
		
		saySomething(textList[index]);
		index++;

	}
}

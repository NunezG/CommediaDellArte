using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class SouffleurScript : MonoBehaviour {
	
	public Image UIimage,UICursor;
	public Text UIText;	
	public float fadeSpeed = 4, maxAlpha = 1,minAlpha = 0, textSpeed = 1;
	public GUIManager guimanager;
	public bool talking = false;

	private bool end = false;
	private int index = 0;
	private Animator animator;
	private IEnumerator coroutineParagraph, coroutineText;
	private CoroutineParameters param;
	private List<string> textList;
	

	public List<string> textList1 = new List<string>{
		"Bienvenue cher comedien ! Comme tu es nouveau dans la troupe, je vais te guider.", 
		"Ton rôle est celui d’Arlecchino, un personnage bien celebre venu d’Italie. C’est un" +
		"meneur d’intrigue, ruse, spirituel et railleur, qui brouille toujours les pistes. C’est" +
		"egalement et avant tout un valet qui represente le peuple, qui l’adore." ,
		"C’est un lourd rôle pour un debutant, j’en ai conscience, mais notre ancien Arlecchino" +
		"s’est brise la jambe lors d’un tour et nous n’avons trouve personne d’autre. Cependant j’ai" +
		"bonne foi que tu t’en sortes, avec mes conseils !"
	};

	public List<string> textList2 = new List<string>{
		"Allez, maintenant il faut que tu distraies ces braves gens si tu veux qu’ils s'amassent devant" +
		"notre scène. Nous, les comediens itinerants, nous jouons chaque soir un peu notre survie.  Si l’on se " +
		"debrouille bien, il n’est pas exclu qu’on nous donne à manger, à boire, voire parfois un logis pour le soir." +
		"C’est toujours mieux que s’endormir affame dans une roulotte, non ?"
	};

	public List<string> textList3 = new List<string>{
		"Tu vois ce coffre, essaie de le fouiller pour voir si un accessoire ne peut pas t’aider…"
	};

	public List<string> textList4 = new List<string>{
		"Mais que fais-tu ? Ce coffre n’est pas un comedien !"
	};

	

	void Start () {
		animator = this.GetComponent<Animator> ();
		UIimage.color = new Color(1,1,1,0);
		UIText.color = new Color(0,0,0,0);
		UICursor.color = new Color(1,1,1,0);
		param = new CoroutineParameters (textSpeed);
		coroutineParagraph = updateParagraph (param);
		coroutineText = updateText ();
	}
	
	void Update () {

		if (talking) {
			UIimage.color = new Color(1,1,1,Mathf.Lerp(UIimage.color.a, maxAlpha, fadeSpeed * Time.deltaTime));
			UIText.color = new Color(0,0,0,Mathf.Lerp(UIimage.color.a, maxAlpha, fadeSpeed * Time.deltaTime));
		} 
		else {
			UIimage.color = new Color(1,1,1,Mathf.Lerp(UIimage.color.a, minAlpha, fadeSpeed * Time.deltaTime));
			UIText.color = new Color(0,0,0,Mathf.Lerp(UIimage.color.a, minAlpha, fadeSpeed * Time.deltaTime));
		}
	
		if (Input.GetButtonDown ("Fire1") && talking){
				param.speedUp(4);
		}
	}



	
	public void saySomething(string s){
		List<string> temp = new List<string>{s};
		saySomething (temp);
	}


	public void saySomething(List<string> text){
		appear ();
		textList = text;
		end = false;
		talking = true;
		coroutineParagraph = updateParagraph (param);
		coroutineText = updateText ();
		StartCoroutine (coroutineText);
		StartCoroutine (coroutineParagraph);
	}

	public void disappear(){
		talking = false;
		guimanager.active = true;
		animator.SetBool ("show", false);
	}
	public void appear(){
		talking = true;
		guimanager.active = false;
		animator.SetBool ("show", true);
	}

	private void nextParagrah(){

		if(index == textList.Count){
			disappear();		
			animator.SetBool ("show", false);
			index = 0;
			guimanager.active = true;
			return;
		}
		index++;
	}

	IEnumerator updateParagraph(CoroutineParameters param){

		float timer = 0;
		string paragraph = "";
		char[] text = textList [index].ToCharArray();
		int charIndex = 0;

		end = false;
		UICursor.color = new Color (1, 1, 1, 0);

		while (charIndex < text.Length) {

            timer += Time.deltaTime;
			if(timer > 1/param.speed){
				while(timer > 1/param.speed ){
					timer -= 1/param.speed;
					string s =""+text[charIndex];
					paragraph = paragraph.Insert(paragraph.Length ,s);
					UIText.text = paragraph;
					charIndex++;

					if(charIndex >= text.Length)
						break; 
				}
				yield return null;
			}
			else
				yield return null;
		}
		end = true;
		UICursor.color = new Color (1, 1, 1, 1);
		yield break;
	}

	IEnumerator updateText(){

		while (true) {
			if (Input.GetButtonDown ("Fire1") && end == true) {
				param.reset();
				index++;
				if (index == textList.Count) {
					disappear ();		
					index = 0;
					UICursor.color = new Color (1, 1, 1, 0);
					yield break;
				}
				coroutineText = updateParagraph(param);
				StartCoroutine (coroutineText);
			}
			yield return null;
		}
	}

	public class CoroutineParameters{

		public float speed;
		private float baseSpeed;

		public CoroutineParameters(float s){
			this.baseSpeed = s;
			this.speed = s;
		}
		public void reset(){
			speed = baseSpeed;
		}
		public void speedUp(float s){
			speed *= s;
		}
	}

}

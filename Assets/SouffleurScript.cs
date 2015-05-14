using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class SouffleurScript : MonoBehaviour {
	
	public Image UIimage;
	public Text UIText;	

	public float fadeSpeed = 4, maxAlpha = 1,minAlpha = 0, textSpeed = 1;
	public GUIManager guimanager;

	private bool show = false, end = false;
	private int index = 0;
	private Animator animator;
	private IEnumerator coroutineParagraph, coroutineText;
	private CoroutineParameters param;

	private bool saidOnce = false;

	public List<string> textList = new List<string>{

		//1
		"Bienvenue cher comedien ! Comme tu es nouveau dans la troupe, je vais te guider.",
		//2
		"Ton role est celui d’Arlecchino, un personnage bien celebre venu d’Italie. " +
		"C’est un meneur d’intrigue, ruse, spirituel et railleur, qui brouille toujours les pistes.",
		//3
		"C’est egalement et avant tout un valet qui represente le peuple, qui l’adore. ",
		//4
		"C’est un lourd role pour un debutant, j’en ai conscience," +
		"mais notre ancien Arlecchino s’est brise la jambe lors d’un tour et nous n’avons trouve personne d’autre." ,
		//5
		"Cependant j’ai bonne foi que tu t’en sortes, avec mes conseils !"};
	

	void Start () {
		animator = this.GetComponent<Animator> ();
		UIimage.color = new Color(1,1,1,0);
		UIText.color = new Color(0,0,0,0);
		param = new CoroutineParameters (textSpeed);
		coroutineParagraph = updateParagraph (param);
		coroutineText = updateText ();
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
	
		if (Input.GetButtonDown ("Fire1")){
		    if( !saidOnce) {
				saidOnce = true;
				saySomething (textList);
			} 
			else {
				param.speedUp(4);
			}
		}
	}

	public void saySomething(List<string> text){
		appear ();
		StartCoroutine (coroutineText);
		StartCoroutine (coroutineParagraph);
	}

	public void disappear(){
		show = false;
		guimanager.active = false;
		animator.SetBool ("show", false);
	}
	public void appear(){
		show = true;
		guimanager.active = true;
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
		Debug.Log ("fin de la coroutine paragrh");
		end = true;
		yield break;
	}

	IEnumerator updateText(){

		while (true) {
			if (Input.GetButtonDown ("Fire1") && end == true) {
				index++;
				if (index == textList.Count) {
					disappear ();		
					index = 0;
					yield break;
				}
				param.reset();
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

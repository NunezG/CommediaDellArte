using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class SouffleurScript : MonoBehaviour {
	
	public Image UIimage,UICursor,UIPanneau;
	public Text UIText;	
	public float fadeSpeed = 4, maxAlpha = 1,minAlpha = 0, textSpeed = 1;
	public GUIManager guimanager;
	public Sprite avecPanneau, sansPanneau;
	public Sprite good, bad, bide;
	[HideInInspector]
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

	public List<string> textList5 = new List<string>{
	"Ce personnage, c’est le Capitan. C’est un soldat. Il peut faire peur a première vue, " +
	"mais en vérité c’est un poltron qui a peur de son ombre. Il n’est bon qu’a fanfaronner," +
	"tu vas sûrement pouvoir aisement trouver un moyen de t’en debarrasser, pour le bonheur de " +
	"notre public… enfin si on peut appeler cela ainsi ! (zoom sur l’avant scène, seules trois personnes" +
	"assistent au spectacle, bruit de vent) ",
	"Mais j’ai confiance en toi, petit ! Si tu accomplis plusieurs actions pour l’effrayer, il finira par s’en aller pleurer dans les jupons de sa mere ! Essaie donc !"
	};
	
    public List<string> textList6 = new List<string>{
   "Bon. Maintenant que nous avons un public, il faut le garder ! Pour ce faire, il va falloir l’ecouter." +
   "Sois attentif à leurs demandes et fais avancer l’intrigue. ",
    "Dans cette situation, nous avons Pantalone (zoom sur lui), il possede toutes les tares du vieux privilegie :" +
    "avarice, credulite, libertinage… Et nous avons aussi Colombina (zoom sur elle), servante hardie et insolente, a" +
    "l’esprit vif. Je ne t’en dis pas plus, il est temps de te debrouiller. Vas-y je te regarde !"
    };

	void Start () {
		animator = this.GetComponent<Animator> ();
		UIimage.color = new Color(1,1,1,0);
		UIText.color = new Color(0,0,0,0);
		UICursor.color = new Color(1,1,1,0);
		UIPanneau.color = new Color (1, 1, 1, 0);
		param = new CoroutineParameters (textSpeed);
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


	public void setPanneau(int imageIndex){
		switch (imageIndex) {
			case (0):
				UIPanneau.sprite = good;
				break;
			case (1):
				UIPanneau.sprite = bad;
				break;
			case (2):
				UIPanneau.sprite = bide;
				break;
        }
	}

	public void giveFeedback(float time, int imageIndex){
		this.GetComponent<Image>().sprite = avecPanneau;
		this.GetComponent<Image> ().color = new Color (1, 1, 1, 1);
		setPanneau (imageIndex);
		StartCoroutine (feedbackCoroutine (time));
	}

	
	public void saySomething(string s, bool reactiveGUI = true){
		List<string> temp = new List<string>{s};
		saySomething (temp, reactiveGUI);
	}


	public void saySomething(List<string> text, bool reactiveGUI = true){
		this.GetComponent<Image>().sprite = sansPanneau;
		appear ();
		textList = text;
		end = false;
		talking = true;
		coroutineParagraph = updateParagraph (param);
		coroutineText = updateText (reactiveGUI);
		StartCoroutine (coroutineText);
		StartCoroutine (coroutineParagraph);
	}

	public void disappear(bool reactiveGUI ){
		guimanager.active = reactiveGUI;
		animator.SetBool ("show", false);
	}
	public void disappear(){
		animator.SetBool ("show", false);
	}
	public void appear(){
		guimanager.active = false;
		animator.SetBool ("show", true);
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

	IEnumerator updateText(bool reactiveGUI){

		while (true) {
			if (Input.GetButtonDown ("Fire1") && end == true) {
				param.reset();
				index++;
				if (index == textList.Count) {
					disappear (reactiveGUI);
					talking = false;
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

	IEnumerator feedbackCoroutine(float time){
		appear ();
		yield return new WaitForSeconds(time);
		disappear ();
		yield return new WaitForSeconds(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
		this.GetComponent<Image> ().color = new Color (1, 1, 1, 0);
		yield break;
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

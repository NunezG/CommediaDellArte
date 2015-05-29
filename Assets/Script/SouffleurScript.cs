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
	public AudioClip sound;
	[HideInInspector]
	public bool talking = false;

	private bool end = false;
	private int index = 0, charIndex = 0;
	private Animator animator;
	private IEnumerator coroutineParagraph, coroutineText;
	private CoroutineParameters param;
	private List<string> textList;
	
	
	public List<string> textList1 = new List<string>{
		"Bienvenue cher comédien ! Comme tu es nouveau dans la troupe, je vais te guider. Alors écoute-moi bien !",
		"Ton rôle est celui d’Arlecchino, un personnage bien célèbre venu d’Italie. C’est un meneur d’intrigue, rusé, " +
		"spirituel et railleur, qui brouille toujours les pistes. C’est également — et avant tout — un valet qui représente" +
		"le peuple, qui l’adore. ",
		"C’est un lourd rôle pour un débutant, j’en ai conscience, mais notre ancien Arlecchino s’est brisé la jambe lors" +
		"d’un tour et... nous n’avons trouvé personne d’autre. Cependant, j’ai bonne foi que tu t’en sortes, avec mes conseils bien sûr !"
	};
	
	public List<string> textList2 = new List<string>{
		"Allez, maintenant il faut que tu distraies ces braves gens si tu veux qu’ils " +
		"s'amassent devant notre scène. Nous, comédiens itinérants, nous jouons chaque soir un peu notre survie. ",
		"Si l’on se débrouille bien, il n’est pas exclu qu’on nous donne à manger, à boire, voire parfois un logis" +
		"pour le soir ! C’est toujours mieux que s’endormir affamé dans une roulotte, non ?"
	};
	
	public List<string> textList3 = new List<string>{
		"Tu vois ce coffre, là-bas, essaie de le fouiller pour voir si un accessoire ne peut pas t’aider…"
	};
	
	public List<string> textList4 = new List<string>{
		"Mais que fais-tu ? Ce coffre n’est pas un comedien !"
	};
	
	public List<string> textList5 = new List<string>{
		"Ce personnage, c’est le Capitan. C’est un soldat. Il peut faire peur à première vue, mais " +
		"en vérité c’est un poltron qui a peur de son ombre. Il n’est bon qu’à fanfaronner, tu vas sûrement" +
		"pouvoir trouver un moyen de t’en débarrasser, pour la joie de notre public… enfin si on peut appeler " +
		"cela ainsi !",
		"Mais j’ai confiance en toi, petit ! Si tu accomplis plusieurs actions pour l’effrayer, il finira par s’en" +
		"aller pleurer dans les jupons de sa mère ! Essaie donc !"
	};
	
	public List<string> textList6 = new List<string>{
		"Bon. Maintenant que nous avons du public, il faut le garder ! Pour ce faire, il va falloir l'écouter." +
		"Sois attentif à leurs demandes et fais avancer l’intrigue en essayant de les satisfaire. ",
		"Dans cette situation, nous avons Pantalone, qui possède toutes les tares du vieux privilégié" +
		": avarice, crédulité, libertinage…",
		"Et nous avons aussi Colombina, servante hardie et insolente, à l’esprit vif. ",
		"Je ne t’en dis pas plus, il est temps de te débrouiller. Moi, je te donnerai juste des indications sur les effets" +
		"de tes actions sur le public. Vas-y je te regarde !"
	};

	public List<string> textList7 = new List<string>{

	"Oh non, on dirait que la taverne vient d’ouvrir ! On va perdre notre public, bon sang de bonsoir ! ",
	"Vite, il nous faut utiliser notre botte secrète, un lazzi, autrement dit un numéro acrobatique " +
	"exécuté par un comédien appelé en renfort. Le public en raffole. Utilise-le si tu es en difficulté," +
	"mais rapelle-toi, tu ne peux en user qu’une fois et cela aura un impact sur le partage de nos gains éventuels." +
	"Tire vite sur la clochette maintenant pour appeler un lazzi !"
	
	};

	void Start () {
		animator = this.GetComponent<Animator> ();
        UIText.supportRichText = true;

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

	public int getIndex(){
		return index;
	}

	public bool getEnd(){
		return end;
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
		UIPanneau.color = new Color (1, 1, 1, 1);
		UIText.color = new Color (1, 1, 1, 0);
		setPanneau (imageIndex);
		StartCoroutine (feedbackCoroutine (time));
	}

	public void giveFeedback(float time, string text){
		this.GetComponent<Image>().sprite = avecPanneau;
		UIPanneau.color = new Color (1, 1, 1, 0);
		UIText.color = new Color (1, 1, 1, 0);
		UIText.text = text;
		StartCoroutine (feedbackCoroutine (time));
	}
	
	public void saySomething(string s, bool reactiveGUI = true){
		List<string> temp = new List<string>{s};
		saySomething (temp, reactiveGUI);
	}

	public void saySomething(List<string> text, bool reactiveGUI = true){
		this.GetComponent<Image>().sprite = sansPanneau;
		appear ();
		this.GetComponent<AudioSource> ().PlayOneShot (sound);
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
        UIText.text = "";
	}
	public void disappear(){
        disappear(true);
	}
	public void appear(){
		guimanager.active = false;
		animator.SetBool ("show", true);
	}


    IEnumerator updateWord(CoroutineParameters param, string openingTag , string closingTag, string text ) {

        float timer = 0;
        UIText.text = UIText.text.Insert( charIndex,openingTag+closingTag);
        int wordIndex = 0;
        charIndex += openingTag.Length;
        string paragraph = UIText.text;

        while (wordIndex < text.Length)
        {
            timer += Time.deltaTime;
            if (timer > 1 / param.speed)
            {
                while (timer > 1 / param.speed)
                {
                    timer -= 1 / param.speed;
                    if (text[wordIndex] == '<' && text[wordIndex + 1] != '/')
                    {
                        string openingTag2 = "", textInside = "", closingTag2 = "";
                        getTag(wordIndex, text.ToCharArray(), ref openingTag2, ref closingTag2, ref textInside);
                        CoroutineParameters co = new CoroutineParameters(param.speed);
                        StartCoroutine(updateWord(co, openingTag2, closingTag2, textInside));
                        while (!co.finished)
                        {
                            yield return null;
                        }
                        wordIndex += openingTag2.Length + closingTag2.Length + textInside.Length;
                        if (wordIndex >= text.Length)
                            break;
                    }
                    else
                    {
                        string s = "" + text[wordIndex];
                        paragraph = paragraph.Insert(charIndex, s);
                        UIText.text = paragraph;
                        charIndex++;
                        wordIndex++;

                        if (wordIndex >= text.Length)
                            break;
                    }
                }
                yield return null;
            }
            else
                yield return null;
        }
        charIndex += closingTag.Length;
        param.finished = true;
        yield break;
    }


	IEnumerator updateParagraph(CoroutineParameters param){

		float timer = 0;
		string paragraph = "";
		char[] text = textList [index].ToCharArray();

		end = false;
		UICursor.color = new Color (1, 1, 1, 0);
        charIndex = 0;

		while (charIndex < text.Length) {

            timer += Time.deltaTime;
			if(timer > 1/param.speed){
				while(timer > 1/param.speed ){
					timer -= 1/param.speed;

                    if (text[charIndex] == '<' && text[charIndex+1] != '/')
                    {
                        string openingTag = "", textInside = "", closingTag = "";
                        getTag(charIndex, text, ref openingTag, ref closingTag, ref textInside);   
                        CoroutineParameters co = new CoroutineParameters(param.speed);
                        StartCoroutine(updateWord(co, openingTag, closingTag, textInside));
                        while (!co.finished)
                        {
                            yield return null;
                        }
                        paragraph = UIText.text;
                    }

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

    private void getTag(int charIndex, char[] text, ref string openingTag, ref string closingTag, ref string textInside)
    {
        int i = charIndex;
        openingTag = ""; textInside = ""; closingTag = "";
        string tagName = "";

        //recuperation de la balise ouvrante
        bool done = false;
        while (text[i] != '>')
        {
            string temp = "" + text[i];
            openingTag = openingTag.Insert(openingTag.Length, temp);
            if (!done)
            {
                if (text[i] == '>' || text[i] == '=')
                    done = true;
                else if (text[i] !='<')
                    tagName = tagName.Insert(tagName.Length, temp);
            }
            i++;
        }
        openingTag = openingTag.Insert(openingTag.Length, ">");
        i++;

        //recuperation du texte a l"interieur de la balise               

        done = false;
        while (!done)
        {
            if (text[i] != '<')
            {
                string temp = "" + text[i];
                textInside = textInside.Insert(textInside.Length, temp);
                i++;
            }
            else
            {
                char[] name = tagName.ToCharArray();
                done = true;
                for (int j = 0; j < tagName.Length; j++)
                {
                    if (text[i + j + 2] != name[j])
                    {
                        string temp = "" + text[i];
                        textInside = textInside.Insert(textInside.Length, temp);
                        i++;
                        done = false; 
                        break;
                    }
                }       
            }
        }
        //recuperation de la balise fermante
        while (text[i] != '>')
        {
            string temp = "" + text[i];
            closingTag = closingTag.Insert(closingTag.Length, temp);
            i++;
        }
        closingTag = closingTag.Insert(closingTag.Length, ">");
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
                    charIndex = 0;
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
		UIPanneau.color = new Color (1, 1, 1, 0);
		yield break;
	}

	private class CoroutineParameters{

		public float speed;
        public bool finished = false;
		private float baseSpeed;

        public CoroutineParameters()
        {
            this.baseSpeed = 1;
            this.speed = 1;
        }
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

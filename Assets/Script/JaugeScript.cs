using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class JaugeScript : MonoBehaviour {

	public List<Image> objectList;

	public Image bar; // image de la barre de remplissage
	public Text textValue; // text qui affiche la valeur de la barre
	public List<Image> otherImage; // tout autre image associées a la barre

	public float minvValue = 0, maxValue = 100, fillSpeed = 10, currentValue;
	public bool activeFade = true; //vrai si la barre disparait si il ,n'y a aucune interaction
	public float fadeSpeed = 10, fadeCountdown = 1; // vitesse de disparition/apparition et temps avant le debut de la disparition

	//public UnityEvent OnFull;

	public List<jaugeEvent> eventsList;

	[HideInInspector]
	public bool show = false;

	private IEnumerator fillCoroutine;
	private bool coroutineOP = false;
	private float timer;

	// Use this for initialization
	void Start () {
		bar.fillAmount = (currentValue / (minvValue + maxValue));
		textValue.text = currentValue.ToString();
	}
	
	// Update is called once per frame
	void LateUpdate () {


		if (eventsList.Count > 0 ) {
			if(currentValue >= eventsList[0].value){
				eventsList[0].events.Invoke();
				eventsList.RemoveAt(0);

			}
		}


		if (activeFade) {

			timer += Time.deltaTime;

			if (show) {
				bar.color = new Color (1, 1, 1, Mathf.Lerp (bar.color.a, 1, Time.deltaTime * fadeSpeed));
				textValue.color = new Color (1, 1, 1, Mathf.Lerp (textValue.color.a, 1, Time.deltaTime * fadeSpeed));
				for(int i = 0; i < otherImage.Count; i++){
					otherImage[i].color = new Color (1, 1, 1, Mathf.Lerp (otherImage[i].color.a, 1, Time.deltaTime * fadeSpeed));
				}
				timer = 0;
			} else if (!coroutineOP && timer > fadeCountdown) {
				bar.color = new Color (1, 1, 1, Mathf.Lerp (bar.color.a, 0, Time.deltaTime * fadeSpeed));
				textValue.color = new Color (1, 1, 1, Mathf.Lerp (textValue.color.a, 0, Time.deltaTime * fadeSpeed));
				for(int i = 0; i < otherImage.Count; i++){
					otherImage[i].color = new Color (1, 1, 1, Mathf.Lerp (otherImage[i].color.a, 0, Time.deltaTime * fadeSpeed));
				}
			}
			show = false;
		}
	}

	public void add(float value){
		setValue (currentValue + value);
	}
	public void sub(float value){
		setValue (currentValue - value);
	}

	public void setValue(float value){

		if(fillCoroutine != null)
			StopCoroutine (fillCoroutine);

		fillCoroutine = (fillBar (value));
		StartCoroutine (fillCoroutine);
	}


	IEnumerator fillBar(float value){

		float fill = ( value  / (minvValue+maxValue) ), tempFillSpeed ;

		coroutineOP = true;

		if (fill < bar.fillAmount)
			tempFillSpeed = -fillSpeed;
		else
			tempFillSpeed = +fillSpeed;


		while (bar.fillAmount != fill) {

			if (activeFade) {
				bar.color = new Color (1, 1, 1, Mathf.Lerp (bar.color.a, 1,Time.deltaTime * fadeSpeed));
				textValue.color = new Color (1, 1, 1, Mathf.Lerp (textValue.color.a, 1,Time.deltaTime * fadeSpeed));
				for(int i = 0; i < otherImage.Count; i++){
					otherImage[i].color = new Color (1, 1, 1, Mathf.Lerp (otherImage[i].color.a, 1, Time.deltaTime * fadeSpeed));
				}
			}
			if( Mathf.Abs( bar.fillAmount - fill) <  (fillSpeed/100)*Time.deltaTime)
				bar.fillAmount = fill;
			else{
				bar.fillAmount += Time.deltaTime * (tempFillSpeed/100);
			}
			currentValue = bar.fillAmount*(maxValue+minvValue);
			textValue.text = ((int)currentValue).ToString();
			yield return null;
		}
		coroutineOP = false;
		timer = 0;
		yield break;
	}

	[System.Serializable]
	public class jaugeEvent{
		 public int value;
		 public UnityEvent events;
	}
}


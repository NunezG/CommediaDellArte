using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof (AudioSource))]


public class PublicScript : MonoBehaviour {
	
	public List<AudioClip> laughtClips = new List<AudioClip>();//liste des son de rires
	public List<AudioClip> clapClips = new List<AudioClip>();//liste des sons d'applaudissements
	public List<publicEvent> EventList;
			
	private float time, value = 100;
	private int index = 0,publicSize = 0;
	private AudioSource audioSource;


	private List<Animator> AnimatorList, presentSpectator, goneSpectator;


	void Start () {

		AnimatorList = new List<Animator> ();
		presentSpectator = new List<Animator> ();
		goneSpectator = new List<Animator> ();
		audioSource = this.GetComponent<AudioSource>();

		index = EventList.Count-1;

		for (int i = 0; i < this.transform.childCount; i++) {
			AnimatorList.Insert(AnimatorList.Count , this.transform.GetChild (i).transform.GetChild (0).GetComponent<Animator>());	
		}
		for (int i = 0; i < AnimatorList.Count; i++) {
			if( AnimatorList[i].GetBool("walkAway") == false){
				presentSpectator.Add(AnimatorList[i]);
			}
			else{
				goneSpectator.Add(AnimatorList[i]);
			}
		}
		publicSize = AnimatorList.Count;
	}

	void Update () {

		if (Input.GetButtonDown ("Fire1")) {
			for (int i = 0; i < this.transform.childCount; i++) {
				//walkAway(i);
			}
			//happy (1);
			subValue (25);

			GameObject.Find ("mario panneau").GetComponent<Animator> ().SetTrigger ("panneau");
		}
	}


	public void addValue(float val){
		value += val;
		while(value >= EventList[index].value ){
			while(publicSize <  EventList[index].publicSize && goneSpectator.Count > 0){
				int rand = Random.Range(0, goneSpectator.Count-1);
				presentSpectator.Insert(0,goneSpectator[rand]);
				goneSpectator[rand].SetBool("walkAway", false);
				goneSpectator.RemoveAt(rand);
			}
			if( (index < EventList.Count))
				index++;
			else
				break;
		}
	}
	public void subValue(float val){
		value -= val;
		while(value <= EventList[index].value ){
			while(publicSize >  EventList[index].publicSize && presentSpectator.Count > 0){
				int rand = Random.Range(0, presentSpectator.Count-1);
				goneSpectator.Insert(0,presentSpectator[rand]);
				presentSpectator[rand].SetBool("walkAway", true);
				presentSpectator.RemoveAt(rand);
				publicSize--;
			}
			if(index > 0)
				index--;
			else
				break;
		}

	}
	public void setValue(float val){
		if (val == value)
			return;
		else if (val > value) {
			addValue (val - value);
		} 
		else {
			subValue(value - val);
		}
	}


	public void happy(float time, float speed){
		StartCoroutine(happyTimer(time, speed));
	}
	
	public void happy(float time){
		StartCoroutine(happyTimer(time));
	}

	public void walkAway(int index){
		this.transform.GetChild (index).transform.GetChild (0).GetComponent<Animator> ().SetTrigger ("walkAway");
	}
	public void walkBack(int index){
		this.transform.GetChild (index).transform.GetChild (0).GetComponent<Animator> ().SetTrigger ("walkAway");
	}

	//coroutine qui gere la durée de l'animation du public 
	IEnumerator happyTimer(float time, float speed = 1 ){

		audioSource.clip = getRandomLaughtSound();
		audioSource.Play ();

		for (int i = 0; i < AnimatorList.Count; i++) {
			AnimatorList[i].SetBool("happy", true);
			AnimatorList[i].speed = speed;
		}

		while (true) {
			if (time > 0) {
				time -= Time.deltaTime;
			}
			else{
				for (int i = 0; i < AnimatorList.Count; i++) {
					AnimatorList[i].SetBool("happy", false);
					audioSource.Stop ();
				}
				yield break;
			}
			yield return null;
		}
	}

	private AudioClip getRandomClapSound(){
		return clapClips [Random.Range (0, clapClips.Count)];
	}
	private AudioClip getRandomLaughtSound(){
		return laughtClips [Random.Range (0, laughtClips.Count)];
	}

	[System.Serializable]
	public class publicEvent{
		public int value, publicSize;
	}


}

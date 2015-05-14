using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class PantaloneScript : MonoBehaviour {

	public JaugeScript jauge;
	
	private Vector3 newPosition;	
	private IEnumerator moveCoroutine;
	private AudioSource audioSource;

	CoroutineParameters cParams;
	
	
	// Use this for initialization
	void Start () {
		
		cParams = new CoroutineParameters ();
		cParams.active = false;
		moveCoroutine = moveCam (cParams);
		audioSource = this.GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void doAction(){
		
		//this.GetComponent<Animator> ().SetTrigger("roll");
		cParams.active = false;
		moveCoroutine = moveCam (cParams);
		StartCoroutine (moveCoroutine);

		GameObject.Find ("Character").GetComponent<CharacterController> ().goTo (this.transform.position);

	}
	
	IEnumerator moveCam(CoroutineParameters param){
		
		while (true) {
			if (GameObject.Find ("Character").transform.position.x == this.transform.position.x) {
				param.active = true;
			}
			
			if (param.active == true) {
				this.GetComponent<Animator> ().SetTrigger ("punch");
				audioSource.Play();
				GameObject.Find("Public").GetComponent<PublicScript>().happy(1.5f, 1.5f);
				yield break;
			}
			yield return null;
		}
		
	}
	
	public class CoroutineParameters{
		public bool active = true;
	}
}

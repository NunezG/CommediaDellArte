using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class CharacterController : MonoBehaviour {


	public float moveSpeed = 1.0f;	
	public Animator sprite;

	private Vector3 goal;//vector ou le personnage doit se rendre
	public AudioSource audioSource;


	// Use this for initialization
	void Start () {
		goal = this.transform.position;
		audioSource = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

	
		if (this.transform.position != goal) {
			if (Vector3.Distance (this.transform.position, goal) < Time.deltaTime * moveSpeed) {
				this.transform.position = goal;
				audioSource.Stop();
			}
			else {
				this.transform.position += Time.deltaTime * ((goal - this.transform.position).normalized) * moveSpeed;
			}
			sprite.SetBool ("move", true);
		} else {
			sprite.SetBool ("move", false);
		}
	}

	public void dothething(){
		//goTo(goal);
	}

	public void goTo(Vector3 vec){		
		goal = vec;

		if (this.transform.position != goal) {
			audioSource.Play ();
		}
	}
	public void setPositionAndGoal(Vector3 pos){
		this.transform.position = pos;
		goal = pos;
	}



}

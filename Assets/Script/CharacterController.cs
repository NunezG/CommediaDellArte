using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class CharacterController : MonoBehaviour {


	public float moveSpeed = 1.0f;	
    public bulleInfoScript bubble;

	private Vector3 goal;//vector ou le personnage doit se rendre
    private Animator _animator;
    [HideInInspector]
	public AudioSource audioSource;


	// Use this for initialization
	void Start () {
		goal = this.transform.position;
		audioSource = this.GetComponent<AudioSource>();
        _animator = this.GetComponentInChildren<Animator>();
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
            _animator.SetBool("move", true);
		} else {
            _animator.SetBool("move", false);
            _animator.speed = 1;
            audioSource.pitch =1 ;
		}
	}

	public void dothething(){
		//goTo(goal);
	}

	public void goTo(Vector3 vec){
        _animator.speed = moveSpeed / 10;
        audioSource.pitch = moveSpeed / 10 ;
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

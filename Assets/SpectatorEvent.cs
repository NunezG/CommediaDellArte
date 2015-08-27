using UnityEngine;
using System.Collections;

public class SpectatorEvent : MonoBehaviour {
	
	private Animator _animator;

	// Use this for initialization
	void Start () {
		_animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void randomAnimationStart(string name){
			_animator.CrossFade (name, 0.5f, 0, ((float)Random.Range (0, 100)) / 100);
	}
}

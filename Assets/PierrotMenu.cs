using UnityEngine;
using System.Collections;

public class PierrotMenu : MonoBehaviour {

    public float _time;
    private float _timer = 0;
    private Animator _animator;



	// Use this for initialization
	void Start () {
       _animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        _timer += Time.deltaTime;

        if (_timer > _time)
        {
            _timer = -3;
            _animator.SetTrigger(Random.Range(0,3).ToString());
        }
	}

        
}

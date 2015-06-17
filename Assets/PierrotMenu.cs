using UnityEngine;
using System.Collections;

public class PierrotMenu : MonoBehaviour {

    public float _time;
    public AudioClip _sound;

    private float _timer = 0;
    private Animator _animator;
    private AudioSource _audioSource;
    private int oldRand = -1;



	// Use this for initialization
	void Start () {
       _animator = this.GetComponent<Animator>();
       _audioSource = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        _timer += Time.deltaTime;

        if (_timer > _time)
        {
            _timer = -3;

            int rand = Random.Range(0, 3);

            while (rand == oldRand) { 
                rand = Random.Range(0, 3);
            }

            oldRand = rand;
            
            _animator.SetTrigger(rand.ToString());
        }
	}


    public void playSound()
    {
        _audioSource.PlayOneShot(_sound);
    }


        
}

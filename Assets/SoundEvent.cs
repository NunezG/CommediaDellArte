using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundEvent : MonoBehaviour {


    public List<AudioClip> _clipList;
    public AudioSource _audioSource;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void playSound(string name)
    {
        for (int i = 0; i < _clipList.Count; i++)
        {
            if (_clipList[i].name == name)
            {
                _audioSource.PlayOneShot(_clipList[i]);
                return;
            }
        }
    }
}


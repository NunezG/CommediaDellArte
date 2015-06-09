using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(AudioSource))]
public class SoundController : MonoBehaviour {


    public List<AudioClip> _clipList;

    private AudioSource _audioSource;
    private float initialVolumeValue;

	// Use this for initialization
	void Start () {
        _audioSource = this.GetComponent<AudioSource>();
        initialVolumeValue = _audioSource.volume;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float playSound(string name, float volume = -1)
    {
        AudioClip temp = null;

        for (int i = 0; i < _clipList.Count; i++)
        {
            Debug.Log(_clipList[i].name);
            if (name == _clipList[i].name)
            {
                temp = _clipList[i];
                break;
            }
        }

        if (temp == null){
            Debug.Log("Sound "+name+" was not found.");
            return -1;
        }
        else
        {
            if(volume != -1)
                 _audioSource.volume = volume;
            _audioSource.PlayOneShot(temp);
        }

        StartCoroutine(resetVolume(temp.length));
        return temp.length;       
    }

    private IEnumerator resetVolume(float time)
    {
        yield return new WaitForSeconds(time);
        _audioSource.volume = initialVolumeValue;
        yield break;
    }


}

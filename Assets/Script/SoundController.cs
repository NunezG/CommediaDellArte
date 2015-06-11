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

    public float playRandomSoundPart(string name, float duration, float volume = -1)
    {
        AudioClip temp = null;

        for (int i = 0; i < _clipList.Count; i++)
        {
            if (name == _clipList[i].name)
            {
                Debug.Log("Sound " + name + " was found.");
                temp = _clipList[i];
                break;
            }
        }

        if (temp == null)
        {
            Debug.Log("Sound " + name + " was not found.");
            return -1;
        }
        else
        {
            if (volume != -1)
                _audioSource.volume = volume;
           
        }

        float startTime = Random.Range(0, temp.length - duration);
        StartCoroutine(playPart(temp, startTime, duration, volume));

        StartCoroutine(resetVolume(duration));
        return temp.length;
    }


    private IEnumerator playPart(AudioClip clip, float startTime, float duration, float volume)
    {
        AudioClip oldClip = _audioSource.clip;
        _audioSource.clip = clip;
        _audioSource.time = startTime;
        _audioSource.Play();


        if (volume != -1)
            volume = _audioSource.volume;
        _audioSource.volume = 0;
        while (_audioSource.volume < volume)
        {
            _audioSource.volume += Time.deltaTime * (volume / 0.5f);
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= Time.deltaTime *( volume / 0.5f );
            yield return null;
        }

        _audioSource.Stop();
        _audioSource.volume = volume;
        _audioSource.clip = oldClip;
        yield break;
    }

    private IEnumerator resetVolume(float time)
    {
        yield return new WaitForSeconds(time);
        _audioSource.volume = initialVolumeValue;
        yield break;
    }


}

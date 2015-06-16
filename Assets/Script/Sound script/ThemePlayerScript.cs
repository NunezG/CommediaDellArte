using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(AudioSource))]
public class ThemePlayerScript : MonoBehaviour {


    public List<AudioClip> themeList;
    private AudioSource _audioSource;
    private float _initialVolumeValue;


    private static ThemePlayerScript _instance;

    public static ThemePlayerScript instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ThemePlayerScript>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {   
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        _audioSource = this.GetComponent<AudioSource>();
        _initialVolumeValue = _audioSource.volume;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void playTheme(string name)
    {
        AudioClip temp = null;

        for (int i = 0; i < themeList.Count; i++)
        {
            if (name == themeList[i].name)
            {
                temp = themeList[i];
                break;
            }
        }

        if (temp == null)
        {
            Debug.Log("Theme " + name + " was not found.");
            return ;
        }
        else
        {
            _audioSource.clip = temp;
            _audioSource.Play();
        }
        return;    
    }

    public void smoothThemeChange(string name, float disappearTime = 1, float waitTime = 0, float appearTime = 1)
    {
        StartCoroutine( smoothThemeChangeCoroutine( name,  disappearTime ,  waitTime ,  appearTime));
    }

    private IEnumerator smoothThemeChangeCoroutine(string name, float disappearTime , float waitTime , float appearTime )
    {
        AudioClip temp = null;

        for (int i = 0; i < themeList.Count; i++)
        {
            if (name == themeList[i].name)
            {
                temp = themeList[i];
                break;
            }
        }
        if (disappearTime > 0)
        {
            while (_audioSource.volume > 0)
            {
                _audioSource.volume -= Time.deltaTime * _initialVolumeValue / disappearTime;
                yield return null;
            }
        }
        _audioSource.volume = 0;

        yield return new WaitForSeconds(waitTime);

        _audioSource.clip = temp;
        _audioSource.Play();

        if (appearTime > 0)
        {
            while (_audioSource.volume < _initialVolumeValue)
            {
                _audioSource.volume += Time.deltaTime * _initialVolumeValue / appearTime;
                yield return null;
            }
        }

        _audioSource.volume = _initialVolumeValue;

        yield break;
    }


}

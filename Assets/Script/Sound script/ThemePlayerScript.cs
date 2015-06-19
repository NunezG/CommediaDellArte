using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(AudioSource))]
public class ThemePlayerScript : MonoBehaviour {


    public List<AudioClip> themeList;
    private AudioSource _audioSource;
    private float _initialVolumeValue;

	private List<Music> _playlist;
    private float timer = 0;

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
        _playlist = new List<Music>();
        _audioSource = this.GetComponent<AudioSource>();
        _initialVolumeValue = _audioSource.volume;
	}
	
	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;

        if (timer <= 0 && _playlist.Count > 0)
        {
           smoothThemeChange(_playlist[0]._audioClip.name, _playlist[0].disappearTime, _playlist[0].waitTime, _playlist[0].appearTime);
           float temp = 0;
           if (_playlist.Count > 1)
               temp = _playlist[1].disappearTime;
           timer = _playlist[0]._audioClip.length - temp;
            if (_playlist[0]._repeatCount != -1)
            {
                _playlist[0]._repeatCount--;
                if (_playlist[0]._repeatCount <= 0)
                {
                    _playlist.RemoveAt(0);
                }
            }
        }
	}


	
	public void resetList(){
		_playlist.Clear ();
        timer = 0;
	}

	public void addMusic(string name, int repeatCount){
		AudioClip temp = null;
		
		for (int i = 0; i < themeList.Count; i++){
			if (name == themeList[i].name){
				temp = themeList[i];
				break;
			}
		}
		
		if (temp == null){
			Debug.Log("Theme " + name + " was not found.");
			return ;
		}
		else{
			_playlist.Add(new Music(temp, repeatCount));
		}
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
            _audioSource.Stop();
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


public class Music{
	public int _repeatCount = 1;
    public float disappearTime = 1, waitTime = 0, appearTime = 1;
	public AudioClip _audioClip;
	public Music(AudioClip audioClip, int repeatCount){
		_repeatCount = repeatCount;
		_audioClip = audioClip;
	}

}

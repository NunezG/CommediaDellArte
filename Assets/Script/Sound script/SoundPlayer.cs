using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SoundPlayer : MonoBehaviour {


    public string _soundFolderName;
    private static AudioClip[] _audioClipArray;
	// Use this for initialization

	void Start () {
        loadRessource();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public static AudioClip getSound(string soundName)
    {
        for (int i = 0; i < _audioClipArray.Length ; i++)
        {
            if(_audioClipArray[i].name ==soundName )
                return (_audioClipArray[i]);
        }
        return null;
    }

    void loadRessource(){
        _audioClipArray = Resources.LoadAll<AudioClip>(_soundFolderName);
    }



}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.IO;

public class XmlManager : MonoBehaviour {


    public List<XmlAsset> _assetList;
    public  GameManager _gameManager;


	public string nomDuDocumentXml;
	public string nomDeLEvenement;

    public static List<XmlAsset> _assetListStatic;
    public static GameManager _gameManagerStatic;
    private static Object coroutineBase;


    void Start(){
        _assetListStatic = _assetList;
        _gameManagerStatic = _gameManager;
        coroutineBase = new Object();
        //_assetList.Clear();
        //_gameManager = null;

        if (nomDuDocumentXml != null && nomDeLEvenement != null)
        {
			TextAsset temp = null;
			for (int i = 0; i < _assetList.Count; i++) {
				if (_assetList [i]._xmlAsset.name == nomDuDocumentXml)
					temp = _assetList [i]._xmlAsset;
			}
			if (temp == null)
				Debug.Log ("Le document xml est introuvable");
			else
				XmlManager.launchEvent (nomDeLEvenement, temp);
		}

    }

	public static void launchEvent(string eventName, TextAsset DocumentXml)
	{
        Evenement event_temp = _gameManagerStatic.loadEvent(DocumentXml, eventName);
		Debug.Log(event_temp._id +" " + event_temp._event.Count);
        _gameManagerStatic.StartCoroutine(_gameManagerStatic.launchEvent(event_temp, DocumentXml));
	}

    public static void launchEvent(string eventName, string xmlAssetName)
    {
        TextAsset xmltemp = null;
        for (int i = 0; i < _assetListStatic.Count; i++)
        {
            if (xmlAssetName == _assetListStatic[i]._name)
            {
                Debug.Log("xml doc found");
                xmltemp = _assetListStatic[i]._xmlAsset;
                break;
            }
        }

        if (xmltemp == null)
        {
            Debug.Log("event not found");
            return;
        }

        Evenement event_temp = _gameManagerStatic.loadEvent(xmltemp, eventName);
        Debug.Log(event_temp._id +" " + event_temp._event.Count);
        _gameManagerStatic.StartCoroutine(_gameManagerStatic.launchEvent(event_temp, xmltemp));
    }


    public static IEnumerator launchEventCoroutine(string eventName, string xmlAssetName)
    {

        TextAsset xmltemp = null;
        for (int i = 0; i < _assetListStatic.Count; i++)
        {
            if (xmlAssetName == _assetListStatic[i]._name)
            {
                Debug.Log("xml doc found");
                xmltemp = _assetListStatic[i]._xmlAsset;
                break;
            }
        }
        if (xmltemp == null)
        {
            Debug.Log("xml doc not found");
            yield break ;
        }

        Evenement event_temp = _gameManagerStatic.loadEvent(xmltemp, eventName);
        if (event_temp == null)
        {
            Debug.Log("event not found");
            yield break ;
        }
        else
        {
            Debug.Log("event found");
            yield return _gameManagerStatic.StartCoroutine(_gameManagerStatic.launchEvent(event_temp, xmltemp));
        }
    }
}

[System.Serializable]
public class XmlAsset{
    public TextAsset _xmlAsset;
    public string _name;
}
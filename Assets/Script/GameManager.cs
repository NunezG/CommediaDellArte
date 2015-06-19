using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.IO;

public class GameManager : MonoBehaviour {


	public PublicScript publicOnScene;
	public CameraScript camera;
	public GUIManager guiManager;
	public FadeBlackScript fadeBlack;


    public List<Character> characterList;
    public List<InteractiveObject> objectList;
    public TextAsset GameAsset;


    public SouffleurManager souffleur;
    private List<Evenement> eventList;
    private CoroutineParameter param;


	// Use this for initialization
	void Start () {
        param = new CoroutineParameter();

        //recherche du public
        publicOnScene = GameObject.FindObjectOfType<PublicScript>();

        eventList = loadEvent(GameAsset);
        StartCoroutine(startEventCoroutine("Introduction", eventList, GameAsset));
        //XmlManager.launchEvent("Introduction","scene_1" );
        //ThemePlayerScript.instance.playTheme("Commedia Theme Redux");
    }

	// Update is called once per frame
	void Update () {
       /* if (Input.GetButtonDown("Fire2")) { 
             Debug.Log("yolo");
             StartCoroutine( startEventCoroutine("Tutorial_1", eventList, GameAsset));
        }*/

	}

    public void launchEndEvent()
    {
        StartCoroutine(endEventCoroutine());
    }

	private IEnumerator endEventCoroutine(){

        yield return StartCoroutine( startEventCoroutine("Tutorial_5", eventList, GameAsset));

        XmlManager.launchEvent("Introduction", "scene1");

        yield break;
	}

    //Lance un evenement dans le jeu
    public void startEvent(string id, List<Evenement> eventList)
    {

        bool found = false;
        for (int i = 0; i < eventList.Count; i++ )
        {
            if (id == eventList[i]._id)
            {
                StartCoroutine(launchEvent(eventList[i], GameAsset)); 
                found = true;
                Debug.Log("Event found");
                break;
            }
        }
        if (!found)
            Debug.Log("Event not found");
    }

    public IEnumerator startEventCoroutine(string id, List<Evenement> eventList, TextAsset xmlAsset)
    {
        Debug.Log("lancement de l'event :" + id);

        int j = 0;
        while (id != eventList[j]._id)
        {
            j++;
        }
        param._count += 1;
        int i = 0;
        while (i < eventList[j]._event.Count)
        {
            yield return StartCoroutine(eventList[j]._event[i]);
            i++;
            //yield return null;
        }
        Debug.Log("Fin de l'event");
        //on recharge l'event en memoire
        eventList[j]._event.Clear();
        eventList[j] = loadEvent(xmlAsset, eventList[j]._id);
        yield break;        
    }


    public IEnumerator launchEvent(Evenement evenement, TextAsset xmlAsset)
    {
        Debug.Log("lancement de l'event :" + evenement._id);
        param._count += 1;
        int i = 0; 
        while (i < evenement._event.Count)
        {  
            yield return StartCoroutine( evenement._event[i]);           
            i++;
           // yield return null;
        }
        Debug.Log("Fin de l'event");
        //on recharge l'event en memoire
        evenement._event.Clear();
        evenement = loadEvent(xmlAsset, evenement._id);
        yield break;
    }


    public List<Evenement> getEventList()
    {
        return eventList;
    }

    //permet de recuperer le gameobject d'un personnage a partir de son nom
    public GameObject getCharacterGameobject(string name){
        
        for (int i = 0; i < characterList.Count; i++){
           // Debug.Log("research : " + name + " , actually this is :" + characterList[i]._characterName);
            if (name == characterList[i]._characterName){
               // Debug.Log("Found");
                return characterList[i]._characterGameobject;
            }
        }
        //Debug.Log("Not Found");
        return null;
    }

    //permet de recuperer un objet a partir de son nom
    private GameObject getInteractiveObjectGameobject(string name)
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            if (name == objectList[i]._name)
            {
                return objectList[i]._gameObject;
            }
        }
        return null;
    }



    //Chargment d'un event a partir d'un fichier xml
    public Evenement loadEvent(TextAsset GameAsset, string id)
    {
        Debug.Log("recherche de l'event : " + id + ".");
        XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
        xmlDoc.LoadXml(GameAsset.text); // load the file.

        XmlNodeList eventList = xmlDoc.GetElementsByTagName("event"); // liste des evenements en xml
        List<IEnumerator> coroutineList = new List<IEnumerator>();
        Evenement result = new Evenement(coroutineList, id);

        for (int temp = 0; temp < eventList.Count; temp++)
        {

            if (eventList[temp].Attributes["id"].Value == id)
            {
                Debug.Log("event found");
                XmlNodeList actionsList = eventList[temp].ChildNodes;

                Debug.Log(actionsList.Count + " actions a charger.");
                for (int i = 0; i < actionsList.Count; i++)
                {
                    //Debug.Log(" nom de la node n°" + i + " :" + actionsList.Item(i).Name);
                    //analyse de la node
                    if (checkNode(actionsList[i], coroutineList, param)) { }
                    else if (actionsList.Item(i).Name == "multiple")
                    {
                        //Debug.Log("Node multiple detecté ");
                        CoroutineParameter paramMultiple = new CoroutineParameter();
                        List<IEnumerator> multipleCoroutineList = new List<IEnumerator>();
                        IEnumerator action = multipleCoroutine(multipleCoroutineList, paramMultiple, param);
                        coroutineList.Insert(coroutineList.Count, action);

                        //remplir la liste des coroutines multiples
                        XmlNodeList multipleActionsList = actionsList.Item(i).ChildNodes;
                        for (int j = 0; j < multipleActionsList.Count; j++)
                        {
                            checkNode(multipleActionsList[j], multipleCoroutineList, paramMultiple);
                        }
                    }
                }
                break;
            }
        }
        return result;
    }
    //Chargment d'une liste event a partir d'un fichier xml
    public List<Evenement> loadEvent(TextAsset GameAsset)
    {
        param = new CoroutineParameter();
        XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
        xmlDoc.LoadXml(GameAsset.text); // load the file.

        XmlNodeList eventList = xmlDoc.GetElementsByTagName("event"); // liste des evenements en xml
        List<Evenement> evenementList = new List<Evenement>(); // liste des evenements
        
         for (int temp = 0; temp < eventList.Count; temp++)
         {
             XmlNodeList actionsList = eventList[temp].ChildNodes;
             List<IEnumerator> coroutineList = new List<IEnumerator>();
             Evenement event_1 = new Evenement(coroutineList, eventList[temp].Attributes["id"].Value);
             evenementList.Add(event_1);

             Debug.Log(actionsList.Count + " actions a charger.");
             for (int i = 0; i < actionsList.Count; i++)
             {
                 //Debug.Log(" nom de la node n°" + i + " :" + actionsList.Item(i).Name);
                 //analyse de la node
                 if (checkNode(actionsList[i], coroutineList, param)) { }
                 else if (actionsList.Item(i).Name == "multiple")
                 {
                     //Debug.Log("Node multiple detecté ");
                     CoroutineParameter paramMultiple = new CoroutineParameter();
                     List<IEnumerator> multipleCoroutineList = new List<IEnumerator>();
                     IEnumerator action = multipleCoroutine(multipleCoroutineList, paramMultiple, param);
                     coroutineList.Insert(coroutineList.Count, action);

                     //remplir la liste des coroutines multiples
                     XmlNodeList multipleActionsList = actionsList.Item(i).ChildNodes;
                     for (int j = 0; j < multipleActionsList.Count; j++)
                     {
                         checkNode(multipleActionsList[j], multipleCoroutineList, paramMultiple);
                     }
                 }
             }

         }
        return evenementList;
    }

    bool checkWaitAttribute(XmlNode node)
    {
        bool wait = true;
        if (node != null)
        {
            if (node.Value == "false" || node.Value == "False")
            {
                wait = false;
            }
        }
        return wait;
    }

    bool checkNode(XmlNode node, List<IEnumerator> coroutineList, CoroutineParameter param)
    {
        if (checkCharacterNode(node, coroutineList, param)) { return true; }
        //activation/desactivation du GUImanager
        else if (checkGUIManagerNode(node, coroutineList, param)) { return true; }
        //action du souffleur
        else if (checkSouffleurNode(node, coroutineList, param)) { return true; }
        //action du public
        else if (checkPublicNode(node, coroutineList, param)) { return true; }
        //action de la camera
        else if (checkCameraNode(node, coroutineList, param)) { return true; }
        //wait coroutine
        else if (checkWaitNode(node, coroutineList, param)) { return true; }
        //fadetoblack coroutine
        else if (checkFadeNode(node, coroutineList, param)) { return true; }
        //actions sur les objets 
        else if (checkObjectNode(node, coroutineList, param)) { return true; }
        //choix des musiques
        else if (checkMusiqueNode(node, coroutineList, param)) { return true; }
        else
            return false;
    }

    bool checkWaitNode(XmlNode node, List<IEnumerator> coroutineList, CoroutineParameter param)
    {
        if (node.Name == "wait")
        {
            //Debug.Log("Attente de " +node.Attributes["time"].Value +"s.");
            IEnumerator action = waitCoroutine(float.Parse(node.Attributes["time"].Value), param);
            coroutineList.Insert(coroutineList.Count, action);
            return true;
        }
        return false;
    }

    bool checkFadeNode(XmlNode node, List<IEnumerator> coroutineList, CoroutineParameter param)
    {
        if (node.Name == "fadetoblack")
        {
            //Debug.Log("Fade de " + node.Attributes["time"].Value + "s.");
            bool wait = checkWaitAttribute(node.Attributes["wait"]);
            IEnumerator action = fadeCoroutine(float.Parse(node.Attributes["time"].Value),wait, param);
            coroutineList.Insert(coroutineList.Count, action);
            return true;
        }
        return false;
    }

    bool checkPublicNode(XmlNode node, List<IEnumerator> coroutineList, CoroutineParameter param)
    {
        if (node.Name == "public")
        {
           // Debug.Log("On cible le public");
            XmlNodeList publicActionsList = node.ChildNodes;
            for (int j = 0; j < publicActionsList.Count; j++)
            {
                if (publicActionsList.Item(j).Name == "laugh")
                {
                    //Debug.Log("Le public rigole pendant " + publicActionsList.Item(j).Attributes["time"].Value + "s.");
                    IEnumerator action = laughCoroutine(float.Parse(publicActionsList.Item(j).Attributes["time"].Value), param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
                if (publicActionsList.Item(j).Name == "addvalue")
                {
                    IEnumerator action = publicValueCoroutine(float.Parse(publicActionsList.Item(j).Attributes["value"].Value), 0, param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
                if (publicActionsList.Item(j).Name == "subvalue")
                {
                    IEnumerator action = publicValueCoroutine(float.Parse(publicActionsList.Item(j).Attributes["value"].Value), 1, param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
                if (publicActionsList.Item(j).Name == "setvalue")
                {
                    IEnumerator action = publicValueCoroutine(float.Parse(publicActionsList.Item(j).Attributes["value"].Value), 2, param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
                if (publicActionsList.Item(j).Name == "sound")
                {
                     bool wait;
                     if (publicActionsList[j].Attributes["wait"] != null)
                         wait = checkWaitAttribute(publicActionsList[j].Attributes["wait"]);
                     else
                         wait = false;

                    float volume = -1;
                    XmlNode nodeTemp = publicActionsList.Item(j).Attributes["volume"];
                    if(nodeTemp != null)
                        volume =  float.Parse(nodeTemp.Value);
                    
                    IEnumerator action = publicPlaySoundCoroutine(publicActionsList.Item(j).Attributes["name"].Value, volume, float.Parse( publicActionsList.Item(j).Attributes["duration"].Value ), wait, param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
            }
            return true;
        }
        return false;
    }

    bool checkCameraNode(XmlNode node, List<IEnumerator> coroutineList, CoroutineParameter param)
    {
        if (node.Name == "camera")
        {
            //Debug.Log("On cible la camera");

            XmlNodeList cameraActionsList = node.ChildNodes;
            for (int j = 0; j < cameraActionsList.Count; j++)
            {
                if (cameraActionsList.Item(j).Name == "deplacement")
                {
                    /*Debug.Log("Deplacement de la camera en : " + cameraActionsList[j].Attributes["x"].Value + ", " +
                              cameraActionsList[j].Attributes["y"].Value + ", " +
                              cameraActionsList[j].Attributes["z"].Value + " .");*/

              
                    bool wait = checkWaitAttribute( cameraActionsList[j].Attributes["wait"]);

                    IEnumerator action = deplacementCameraCoroutine(new Vector3(
                     float.Parse(cameraActionsList[j].Attributes["x"].Value),
                     float.Parse(cameraActionsList[j].Attributes["y"].Value),
                     float.Parse(cameraActionsList[j].Attributes["z"].Value)), wait, param);

                    coroutineList.Insert(coroutineList.Count, action);
                }
                if (cameraActionsList.Item(j).Name == "reset")
                {
                    //Debug.Log("Reset de la position de la camera.");
                    bool wait = checkWaitAttribute(cameraActionsList[j].Attributes["wait"]);
                    IEnumerator action = resetCameraCoroutine(wait, param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
            }
            return true;
        }
        return false;
    }

    bool checkSouffleurNode(XmlNode node, List<IEnumerator> coroutineList, CoroutineParameter param)
    {
        if (node.Name == "souffleur")
        {
            int position = 0;
            if(node.Attributes["position"].Value == "milieu") position = 0;
            else if(node.Attributes["position"].Value == "haut") position = 1;
            else if(node.Attributes["position"].Value == "droite") position = 2;
            else if (node.Attributes["position"].Value == "gauche") position = 3;

           // Debug.Log("On cible le souffleur ");
            XmlNodeList characterActionsList = node.ChildNodes;
            for (int j = 0; j < characterActionsList.Count; j++)
            {
                if (characterActionsList.Item(j).Name == "talk")
                {
                   // Debug.Log("Le souffleur veut dire un truc ");
                    XmlNodeList souffleurText = characterActionsList.Item(j).ChildNodes;
                    List<string> text = new List<string>();
                    for (int k = 0; k < souffleurText.Count; k++)
                    {
                        text.Add(souffleurText[k].InnerText);
                    }
                    IEnumerator action = talkCoroutine(text, position, param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
                else if (characterActionsList.Item(j).Name == "feedback")
                {
                    /*Debug.Log("Le souffleur envoie un feedback de type : " + characterActionsList[j].Attributes["type"].Value +
                              " d'une durée de " + characterActionsList[j].Attributes["time"].Value + "s.");*/
                    IEnumerator action = feedbackCoroutine(characterActionsList[j].Attributes["type"].Value, float.Parse(characterActionsList[j].Attributes["time"].Value), position, param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
                if (characterActionsList.Item(j).Name == "sound")
                {
                    bool wait;
                    if (characterActionsList[j].Attributes["wait"] != null)
                        wait = checkWaitAttribute(characterActionsList[j].Attributes["wait"]);
                    else
                        wait = false;
                    float volume = -1;
                    XmlNode nodeTemp = characterActionsList.Item(j).Attributes["volume"];
                    if (nodeTemp != null)
                        volume = float.Parse(nodeTemp.Value);

                    IEnumerator action = souffleurPlaySoundCoroutine( characterActionsList.Item(j).Attributes["name"].Value, volume, wait, param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
            }
            return true;
        }
        return false;
    }

    bool checkGUIManagerNode(XmlNode node, List<IEnumerator> coroutineList, CoroutineParameter param)
    {
        if (node.Name == "guimanager" || node.Name == "guiManager")
        {
            //Debug.Log("Modification du GUIManager en " + node.Attributes["active"].Value + ".");
            bool temp = false;
            if (node.Attributes["active"].Value == "true" ||
                node.Attributes["active"].Value == "True" ||
                node.Attributes["active"].Value == "Vrai" ||
                node.Attributes["active"].Value == "vrai")
            {
                temp = true;
            }
            else if (node.Attributes["active"].Value == "false" ||
                node.Attributes["active"].Value == "False" ||
                node.Attributes["active"].Value == "Faux" ||
                node.Attributes["active"].Value == "faux")
            {
                temp = false;
            }
            IEnumerator action = guiCoroutine(temp, param);
            coroutineList.Insert(coroutineList.Count, action);
            return true;
        }
        return false;
    }

    bool checkCharacterNode(XmlNode node, List<IEnumerator> coroutineList, CoroutineParameter param)
    {
        if (node.Name == "character")
        {
            XmlNodeList characterActionsList = node.ChildNodes;
            for (int j = 0; j < characterActionsList.Count; j++)
            {
                if (characterActionsList.Item(j).Name == "deplacement")
                {
                   
                    bool wait = checkWaitAttribute(characterActionsList[j].Attributes["wait"]);

                    XmlNode instantNode = characterActionsList[j].Attributes["instant"];
                    bool instant = false;
                    if (instantNode != null)
                    {
                        instant = false;
                        if (instantNode.Value == "true" || instantNode.Value == "True")
                        {
                            instant = true;
                        }
                    }
                    IEnumerator action = deplacementCoroutine(node.Attributes["name"].Value,
                     new Vector3(
                     float.Parse(characterActionsList[j].Attributes["x"].Value),
                     float.Parse(characterActionsList[j].Attributes["y"].Value),
                     float.Parse(characterActionsList[j].Attributes["z"].Value)), instant,  wait, param);

                    coroutineList.Insert(coroutineList.Count, action);
                }
                if (characterActionsList.Item(j).Name == "sound")
                {
                    bool wait;
                    if (characterActionsList[j].Attributes["wait"] != null)
                        wait = checkWaitAttribute(characterActionsList[j].Attributes["wait"]);
                    else
                        wait = false;
                    float volume = -1;
                    XmlNode nodeTemp = characterActionsList.Item(j).Attributes["volume"];
                    if(nodeTemp != null)
                        volume =  float.Parse(nodeTemp.Value);
                    
                    IEnumerator action = playSoundCoroutine(node.Attributes["name"].Value,characterActionsList.Item(j).Attributes["name"].Value, volume, wait, param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
                else if (characterActionsList.Item(j).Name == "animation")
                {
                    bool wait = checkWaitAttribute(characterActionsList[j].Attributes["wait"]);
                    IEnumerator action = animationCoroutine(node.Attributes["name"].Value, characterActionsList[j].Attributes["name"].Value, wait, param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
                else if (characterActionsList.Item(j).Name == "rotation")
                {              
                    bool wait = checkWaitAttribute(characterActionsList[j].Attributes["wait"]);
                    IEnumerator action = rotationCoroutine(node.Attributes["name"].Value,
                    new Vector3(
                        float.Parse(characterActionsList[j].Attributes["x"].Value),
                        float.Parse(characterActionsList[j].Attributes["y"].Value),
                        float.Parse(characterActionsList[j].Attributes["z"].Value)) , param);

                    coroutineList.Insert(coroutineList.Count, action);
                }
                else if (characterActionsList.Item(j).Name == "bubble")
                {
                    IEnumerator action = bubbleCoroutine(node.Attributes["name"].Value, characterActionsList[j].Attributes["name"].Value, float.Parse(characterActionsList[j].Attributes["time"].Value), param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
                else if (characterActionsList.Item(j).Name == "interaction")
                {
                    bool temp = false;
                    if (characterActionsList.Item(j).Attributes["value"].Value == "true" ||
                        characterActionsList.Item(j).Attributes["value"].Value == "True" ||
                        characterActionsList.Item(j).Attributes["value"].Value == "Vrai" ||
                        characterActionsList.Item(j).Attributes["value"].Value == "vrai")
                    {
                        temp = true;
                    }
                    else if (characterActionsList.Item(j).Attributes["value"].Value == "false" ||
                        characterActionsList.Item(j).Attributes["value"].Value == "False" ||
                        characterActionsList.Item(j).Attributes["value"].Value == "Faux" ||
                        characterActionsList.Item(j).Attributes["value"].Value == "faux")
                    {
                        temp = false;
                    }
                    IEnumerator action = characterInteractionCoroutine(node.Attributes["name"].Value, temp, param);
                    coroutineList.Insert(coroutineList.Count, action);
                }
            }
            return true;
        }
        return false;
    }

    bool checkObjectNode(XmlNode node, List<IEnumerator> coroutineList, CoroutineParameter param)
    {
        if (node.Name == "object")
        {
              XmlNodeList objectActionsList = node.ChildNodes;
              for (int j = 0; j < objectActionsList.Count; j++)
              {
                  if (objectActionsList.Item(j).Name == "interaction")
                  {
                      bool temp = false;
                      if (objectActionsList.Item(j).Attributes["value"].Value == "true" ||
                          objectActionsList.Item(j).Attributes["value"].Value == "True" ||
                          objectActionsList.Item(j).Attributes["value"].Value == "Vrai" ||
                          objectActionsList.Item(j).Attributes["value"].Value == "vrai")
                      {
                          temp = true;
                      }
                      else if (objectActionsList.Item(j).Attributes["value"].Value == "false" ||
                          objectActionsList.Item(j).Attributes["value"].Value == "False" ||
                          objectActionsList.Item(j).Attributes["value"].Value == "Faux" ||
                          objectActionsList.Item(j).Attributes["value"].Value == "faux")
                      {
                          temp = false;
                      }
                      IEnumerator action = objectInteractionCoroutine(node.Attributes["name"].Value, temp, param);
                      coroutineList.Insert(coroutineList.Count, action);                  
                  }
                  else if (objectActionsList.Item(j).Name == "animation")
                  {
                      bool wait = checkWaitAttribute(objectActionsList[j].Attributes["wait"]);

                      IEnumerator action = objectAnimationCoroutine(node.Attributes["name"].Value, objectActionsList.Item(j).Attributes["name"].Value, wait, param);
                      coroutineList.Insert(coroutineList.Count, action);
                  }
                  else if (objectActionsList.Item(j).Name == "deplacement")
                  {
                      IEnumerator action = objectDeplacementCoroutine(node.Attributes["name"].Value, new Vector3(
                     float.Parse(objectActionsList[j].Attributes["x"].Value),
                     float.Parse(objectActionsList[j].Attributes["y"].Value),
                     float.Parse(objectActionsList[j].Attributes["z"].Value)), param);
                      coroutineList.Insert(coroutineList.Count, action);
                  }
				 else if (objectActionsList.Item(j).Name == "sound")
				{
					bool wait;
					if (objectActionsList[j].Attributes["wait"] != null)
						wait = checkWaitAttribute(objectActionsList[j].Attributes["wait"]);
					else
						wait = false;
					float volume = -1;
					XmlNode nodeTemp = objectActionsList.Item(j).Attributes["volume"];
					if(nodeTemp != null)
						volume =  float.Parse(nodeTemp.Value);
					
					IEnumerator action = objectPlaySoundCoroutine(node.Attributes["name"].Value,objectActionsList.Item(j).Attributes["name"].Value, volume, wait, param);
					coroutineList.Insert(coroutineList.Count, action);
				}

             }
             return true;
      }
      return false;
    }

    bool checkMusiqueNode(XmlNode node, List<IEnumerator> coroutineList, CoroutineParameter param)
    {

        if (node.Name == "musique")
        {  
            int repeat = 1;
            if(node.Attributes["repeat"] != null){
                repeat = int.Parse(node.Attributes["repeat"].Value);
            }
            bool reset = false;
            if (node.Attributes["reset"] != null)
            {
                if (node.Attributes["reset"].Value == "true" ||
                        node.Attributes["reset"].Value == "True" ||
                        node.Attributes["reset"].Value == "Vrai" ||
                        node.Attributes["reset"].Value == "vrai")
                {
                    reset = true;
                }
            }

            IEnumerator action = musiqueCoroutine(node.Attributes["name"].Value, 
                    repeat,
                    float.Parse(node.Attributes["disappearTime"].Value),
                    float.Parse(node.Attributes["waitTime"].Value),
                    float.Parse(node.Attributes["appearTime"].Value), reset,  param);
            coroutineList.Insert(coroutineList.Count, action);
        }
        return false;
    }

    IEnumerator musiqueCoroutine(string musiqueName, int repeat, float dissapearTime, float waitTime, float appearTime, bool reset , CoroutineParameter param)
    {
        Debug.Log("Changement du theme : " + musiqueName);
        if(reset)
             ThemePlayerScript.instance.resetList();
        ThemePlayerScript.instance.addMusic(musiqueName, repeat);
        //ThemePlayerScript.instance.smoothThemeChange(musiqueName, dissapearTime, waitTime, appearTime);
        param._count++;
        yield break;
    }

    IEnumerator objectDeplacementCoroutine(string objectName, Vector3 position, CoroutineParameter param)
    {
        getInteractiveObjectGameobject(objectName).transform.position = position;
        param._count++;
        yield break;
    }

    IEnumerator objectAnimationCoroutine(string objectName, string animationName, bool wait, CoroutineParameter param)
    {
        Animator tempAnimator;
        tempAnimator = getInteractiveObjectGameobject(objectName).GetComponent<Animator>();
        if (tempAnimator == null)
            tempAnimator = getInteractiveObjectGameobject(objectName).GetComponentInChildren<Animator>();

        tempAnimator.SetTrigger(animationName);

        if (wait)
        {
            while (tempAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash != Animator.StringToHash(animationName))
            {
                Debug.Log("recherche du state");
                yield return null;
            }
            Debug.Log("trouvé");
            yield return new WaitForSeconds(tempAnimator.GetCurrentAnimatorStateInfo(0).length);
            Debug.Log("attente finie");
        }

        param._count++;
        yield break;
    }

    IEnumerator objectInteractionCoroutine(string objectName, bool active, CoroutineParameter param)
    {
        if (active)
            Debug.Log("Activation de l'objet : " + objectName +".");
        else
            Debug.Log("Desactivation de l'objet : " + objectName + ".");

        getInteractiveObjectGameobject(objectName).GetComponent<Collider2D>().enabled = active;
        param._count++;
        yield break;
    }

    IEnumerator characterInteractionCoroutine(string characterName, bool active, CoroutineParameter param)
    {
        if (active)
            Debug.Log("Activation du personnage : " + characterName + ".");
        else
            Debug.Log("Desactivation du personnage : " + characterName + ".");

        getCharacterGameobject(characterName).GetComponent<Collider2D>().enabled = active;
        param._count++;
        yield break;
    }

    IEnumerator guiCoroutine(bool active, CoroutineParameter param )
    {
        if (active)
            Debug.Log("Activation du GUImanager.");
        else 
            Debug.Log("Desactivation du GUImanager.");
        guiManager.active = active;
        param._count++;
        yield break;
    }

    IEnumerator deplacementCoroutine(string characterName, Vector3 position, bool instant, bool wait, CoroutineParameter param)
    {
        Debug.Log("Execution d'un deplacement de " + characterName + " en " + position + ".");
        CharacterController character = getCharacterGameobject(characterName).GetComponent<CharacterController>();
        if (instant)
        {
            character.setPositionAndGoal(position);
        }
        else
        {
            character.goTo(position);
        }

        if (wait)
        {
            while (character.transform.position != position)
            {
                yield return null;
            }
        }
        param._count++;
        yield break;
    }

    IEnumerator rotationCoroutine(string characterName, Vector3 rotation, CoroutineParameter param ) 
    {
        Debug.Log("Execution d'une rotation de " + characterName + " en " + rotation + ".");
        GameObject character = getCharacterGameobject(characterName);

        character.transform.Rotate(rotation);
        param._count++;
        yield break;
    }

    IEnumerator animationCoroutine(string characterName, string animationName, bool wait, CoroutineParameter param )
    {
        Debug.Log("Execution d'une animation de " + characterName + " , qui est \"" + animationName + "\".");
        Animator characterAnimator = getCharacterGameobject(characterName).GetComponentInChildren<Animator>();

        if( characterAnimator.GetBool(animationName) !=null )
		    characterAnimator.SetBool (animationName, true);
        else
         characterAnimator.SetTrigger(animationName);


        if (wait) {
		    while(characterAnimator.GetCurrentAnimatorStateInfo (0).shortNameHash !=  Animator.StringToHash(animationName) )
            {
                 Debug.Log("recherche du state");
				  yield return null;
		    }
            Debug.Log("trouvé");
		    yield return new WaitForSeconds(characterAnimator.GetCurrentAnimatorStateInfo(0).length);
            Debug.Log("attente finie");
         }
        param._count++;
        yield break;
    }

    IEnumerator feedbackCoroutine(string type, float time, int position, CoroutineParameter param)
    {
        Debug.Log("Envoie d'un feedback de type :" + type + " , de " + time + "s.");
        if(type == "good")
        {
            souffleur.giveFeedback(time, 0, position);
        }
        else if(type == "bad")
        {
            souffleur.giveFeedback(time, 1, position);
        }
        param._count++;
        yield break;
    }

    IEnumerator talkCoroutine(List<string> text, int position, CoroutineParameter param)
    {
        Debug.Log("Le souffleur parle.");
        souffleur.saySomething(text, position, false);
        while (souffleur.souffleurArray[position].talking == true)
        {
            yield return null;
        }
        param._count++;
        yield break;
    }

    IEnumerator laughCoroutine(float time, CoroutineParameter param)
    {
        Debug.Log("Le public rigole.");
        publicOnScene.happy(time);
        param._count++;
        yield break;
    }

    IEnumerator deplacementCameraCoroutine(Vector3 position, bool wait,  CoroutineParameter param )
    {
        Debug.Log("La camera se deplace en :" + position + ".");
        camera.moveTo(position);
        if (wait)
        {
            while ((position - camera.transform.position).magnitude > 1)
            {
                yield return null;
            }
        }
        param._count++;
        yield break;
    }

    IEnumerator resetCameraCoroutine(bool wait, CoroutineParameter param )
    {
        Debug.Log("On reset la position de la camera");
        camera.resetPosition();
        if (wait)
        {
            while ((camera.getOriginalPosition() - camera.transform.position).magnitude > 1)
            {
                yield return null;
            }
        }
        param._count++;
        yield break;
    }

    IEnumerator waitCoroutine(float time, CoroutineParameter param ) 
    {
        yield return new WaitForSeconds(time);
        param._count++;
        yield break;
    }

    IEnumerator fadeCoroutine(float time, bool wait, CoroutineParameter param )
    {
        Debug.Log("Fadetoblack de " + time + "s.");
        fadeBlack.fadeToBlack(time);
        if(wait)
         yield return new WaitForSeconds( time + 2 / fadeBlack.fadeSpeed);
        param._count++;
        yield break;
    }

    IEnumerator publicValueCoroutine(float value, int type, CoroutineParameter param)
    {
        if (type == 0)
        {
            publicOnScene.addValue(value);
        }
        else if (type == 1)
        {
            publicOnScene.subValue(value);
        }
        else if (type == 2)
        {
            publicOnScene.setValue(value);
        }
        param._count++;
        yield break;
    }

    IEnumerator multipleCoroutine(List<IEnumerator> list, CoroutineParameter multipleParam, CoroutineParameter param)
    {
        //On demarre toute les coroutines
        for (int i = 0; i < list.Count; i++)
        {
            StartCoroutine(list[i]);
        }
        //Ojn attend la fin de chaque coroutine
        while (multipleParam._count != list.Count)
        {
            yield return null;
        }

        param._count++;
        yield break;
    }

    IEnumerator bubbleCoroutine(string characterName, string bubbleName, float time, CoroutineParameter param)
    {
        Debug.Log("Affichage de la bubble :"+bubbleName+ "  pendant :  "+time+".");
        CharacterController character = getCharacterGameobject(characterName).GetComponent<CharacterController>();
        character.bubble.showBubble(time, bubbleName);
        param._count++;
        yield break;
    }

    IEnumerator playSoundCoroutine(string characterName, string soundName, float volume, bool wait, CoroutineParameter param)
    {
        if (volume == -1)
        {
            volume = getCharacterGameobject(characterName).GetComponent<AudioSource>().volume;
        }
        Debug.Log("Emission d'un son sur "+characterName+" :" + soundName + "  avec un volume de :  " + volume + ".");

        SoundController soundC = getCharacterGameobject(characterName).GetComponent<SoundController>();
        float duration = soundC.playSound(soundName, volume);

        if (wait)
            yield return new WaitForSeconds(duration);

        param._count++;
        yield break;
    }

    IEnumerator souffleurPlaySoundCoroutine( string soundName, float volume, bool wait, CoroutineParameter param)
    {
        if (volume == -1)
        {
            volume = souffleur.GetComponent<AudioSource>().volume;
        }
        Debug.Log("Emission d'un son sur le souffleur :" + soundName + "  avec un volume de :  " + volume + ".");

        SoundController soundC = souffleur.GetComponent<SoundController>();
        float duration = soundC.playSound(soundName, volume);

        if (wait)
            yield return new WaitForSeconds(duration);

        param._count++;
        yield break;
    }

    IEnumerator objectPlaySoundCoroutine(string objectName, string soundName, float volume, bool wait, CoroutineParameter param)
    {
        if (volume == -1)
        {
            volume = getInteractiveObjectGameobject(objectName).GetComponent<AudioSource>().volume;
        }
        Debug.Log("Emission d'un son :" + soundName + "  avec un volume de :  " + volume + ".");

        SoundController soundC = getInteractiveObjectGameobject(objectName).GetComponent<SoundController>();
        float duration = soundC.playSound(soundName, volume);

        if (wait)
            yield return new WaitForSeconds(duration);

        param._count++;
        yield break;
    }

    IEnumerator publicPlaySoundCoroutine( string soundName, float volume, float duration, bool wait, CoroutineParameter param)
    {
        if (volume == -1)
        {
            volume = publicOnScene.GetComponent<AudioSource>().volume;
        }
        Debug.Log("Emission d'un son :" + soundName + "  avec un volume de :  " + volume + ".");

        SoundController soundC = publicOnScene.GetComponent<SoundController>();

        soundC.playRandomSoundPart(soundName, duration, volume);

        if (wait)
            yield return new WaitForSeconds(duration);

        param._count++;
        yield break;
    }


    

	//Intro avec le souffleur
	/*public IEnumerator event1(){

		guiManager.active = false;

		Vector3 moveEvent1 =  new Vector3(-16,7,30);
		character.goTo (moveEvent1);

		while (character.transform.position != moveEvent1) {
			yield return null;
		}

		souffleur.saySomething (souffleur.textList1, false);

		while (souffleur.talking == true) {
			yield return null;
		}

		Vector3 zoomPublicEvent = new Vector3(0,6,0);

		publicOnScene.addValue (20);
		souffleur.saySomething (souffleur.textList2, false);
		camera.moveTo (zoomPublicEvent);

		while (souffleur.talking == true) {
			yield return null;
		}
		camera.resetPosition ();

		guiManager.active = false;
		yield return new WaitForSeconds (1.0f);

		Vector3 zoomCoffreEvent = new Vector3(17,10,16);

		souffleur.saySomething (souffleur.textList3, false);
		camera.moveTo (zoomCoffreEvent);

		while (souffleur.talking == true) {
			yield return null;
		}
		camera.resetPosition ();

		guiManager.active = true;

		yield break;

	}*/

	//Arrivé du capitaine
	/*public IEnumerator event2(){

		guiManager.active = false;
		
		Vector3 moveEvent1 =  new Vector3(-16,7,30);
		capitaine.GetComponent<CharacterController>().goTo (moveEvent1);

		while (capitaine.transform.position !=  moveEvent1) {
			yield return null;
		}

		capitaine.GetComponent<CharacterController>().sprite.SetTrigger ("moquerie");

		capitaine.GetComponent<AudioSource> ().PlayOneShot (capitaine.moquerie,1);

		souffleur.saySomething (souffleur.textList5, false);
		while (souffleur.talking == true) {
			yield return null;
		}

		//activation du capitaine
		capitaine.GetComponent<Collider2D> ().enabled = true;

		guiManager.active = true;

		yield break;
		
	}*/

    //Arrivé de pantalone et colombine
   /* public IEnumerator event3(){
        
        guiManager.active = false;

		//fondu noir 
		fadeBlack.fadeToBlack (1.0f);
		yield return new WaitForSeconds (1/fadeBlack.fadeSpeed);
		yield return new WaitForSeconds (1);

		coffre.gameObject.SetActive (false);
        pantalone.GetComponent<CharacterController>().setPositionAndGoal (new Vector3(-16, 7, 30));
		character.setPositionAndGoal (new Vector3 (-8, 7, 30));
        colombine.GetComponent<CharacterController>().setPositionAndGoal (new Vector3(16, 7, 30));

		yield return new WaitForSeconds (1/fadeBlack.fadeSpeed);
		yield return new WaitForSeconds (0.5f);

		Vector3 zoomPantaloneEvent = new Vector3(-15,12,11);
		Vector3 zoomColombineEvent = new Vector3(17,10,16);
		bool eventPantaloneDone = false, eventColombineDone = false;

        souffleur.saySomething (souffleur.textList6, false);

        while (souffleur.talking == true) {

			if(!eventPantaloneDone && souffleur.getIndex() == 1){
				camera.moveTo (zoomPantaloneEvent);
			}
			if(!eventColombineDone && souffleur.getIndex() == 2){
				camera.moveTo (zoomColombineEvent);				
            }
            yield return null;
		}

        camera.resetPosition ();
		yield return new WaitForSeconds (2.0f);

		pantalone.GetComponentInChildren<Animator> ().SetTrigger("asking");
        pantalone.GetComponentInChildren<bulleInfoScript>().showBubble(1.5f, "Pantalone_love_Colombine");
		yield return new WaitForSeconds(pantalone.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);

        guiManager.active = true;
        
        yield break;
        
    }*/

	//Arrivé du lazzi
	/*public IEnumerator lazziEvent(){

		guiManager.active = false;

		publicOnScene.subValue (100);

		souffleur.saySomething (souffleur.textList7, false);

		Vector3 zoomClocheEvent = new Vector3 (20, 23, 27);
		
		while (souffleur.talking == true) {
			if(souffleur.getIndex() == 1){
				camera.moveTo(zoomClocheEvent);
				break;
			}
			yield return null;
		}
		while (souffleur.talking == true) {
			yield return null;			
		}
		camera.resetPosition();
        cloche.enabled = true;
		
		guiManager.active = true;

		yield break;
	}*/
    
    //Event avec pierrot
	/*IEnumerator eventFinTuto(){
		
		guiManager.active = false;

		Vector3 goToCenterEvent = new Vector3 (-1.5f, 10, 30);
		Vector3 zoomLazziEvent = new Vector3 (0, 13, 5);
		
		pierrot.GetComponent<CharacterController>().goTo (goToCenterEvent);
		while (pierrot.transform.position !=  goToCenterEvent) {
			yield return null;
		}
		
		//zoom
		camera.moveTo (zoomLazziEvent);
		
		pierrot.GetComponentInChildren<Animator> ().SetTrigger ("juggling");
		while(pierrot.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).shortNameHash !=  Animator.StringToHash("pierrot_jongle") ){
			yield return null;
		}
		yield return new WaitForSeconds(pierrot.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length );
		
		publicOnScene.setValue (75);
				
		Vector3 goTalkToTpantalone = new Vector3 (-8, 7, 30);
		character.goTo (goTalkToTpantalone);
		while (character.transform.position !=  goTalkToTpantalone) {
			yield return null;
		}	

		pantalone.GetComponentInChildren<Animator> ().SetTrigger ("dispute");
		character.GetComponentInChildren<Animator> ().SetTrigger ("angryTalking");
		yield return new WaitForSeconds(pantalone.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);

		Vector3 moveToColombine = new Vector3 (11, 10, 30);
		pierrot.GetComponent<CharacterController>().goTo (moveToColombine);
		while (pierrot.transform.position !=  moveToColombine) {
			yield return null;
		}
		pierrot.GetComponentInChildren<Animator> ().SetTrigger ("range");
		colombine.gameObject.SetActive (false);

		while(pierrot.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo (0).shortNameHash != Animator.StringToHash("pierrot_range_colombine") ){
			yield return null;
		}
		yield return new WaitForSeconds(pierrot.GetComponentInChildren<Animator> ().GetCurrentAnimatorStateInfo(0).length);
		

		Vector3 goAwayEvent = new Vector3 (50f, 10, 30);
		pierrot.GetComponent<CharacterController>().goTo (goAwayEvent);
		while (pierrot.transform.position !=  goAwayEvent) {
			yield return null;
		}

		// compte rendu 
		//fadeBlack.fadeToBlack (Mathf.Infinity);
		GameObject.Find ("Gazette").GetComponent<Image>().color = new Color (1, 1, 1, 1);

		while (true) {
			if(Input.GetButtonDown("Fire1")){
				Application.LoadLevel("saynete_1");
				break;
			}
			yield return null;
		}

		yield break;
	}*/

}

[System.Serializable]
public class Character
{
    public GameObject _characterGameobject;
    public string _characterName;
    public Character(GameObject g, string s )
    {
        _characterGameobject = g;
        _characterName = s;
    }
}

public class Evenement
{
    public List<IEnumerator> _event;
    public string _id;
    
    public Evenement(List<IEnumerator> i, string s)
    {
        _event = i;
        _id = s;
    }

}

class CoroutineParameter
{
    public int _count;
    public CoroutineParameter()
    {
        _count = 0;
    }
}

[System.Serializable]
public class InteractiveObject
{
    public GameObject _gameObject;
    public string _name;
}
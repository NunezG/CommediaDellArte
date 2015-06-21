using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

	public Canvas loadingScreen;
	public Text loadingPercentage;
	private int percentage = 0;

    private static LoadingScreen _instance;

    public static LoadingScreen instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LoadingScreen>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            Debug.Log("instance is null");
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
            {
                Debug.Log("instance is already defined");
                StartCoroutine(disappear(_instance.gameObject));
            }
            _instance = this;
        }
    }

	// Use this for initialization
	void Start () {
		loadingScreen.gameObject.SetActive (false);
	}

	
	// Update is called once per frame
	void Update () {
	}

	public void loadLevel(string levelName){
        loadingScreen.enabled = true;
		StartCoroutine (displayLoadingScreen (levelName));
	}

    IEnumerator disappear(GameObject obj) {

        float fadeSpeed = 1.0f;
        Image temp = obj.GetComponent<LoadingScreen>().loadingScreen.GetComponentInChildren<Image>();
        obj.GetComponent<LoadingScreen>().loadingScreen.GetComponentInChildren<Text>().text = " ";
        obj.GetComponent<LoadingScreen>().loadingScreen.GetComponentsInChildren<Text>()[1].text = " ";

        while (temp.color.a > 0.1)
        {
            Debug.Log("disappearing");
            temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, Mathf.Lerp(temp.color.a, 0, fadeSpeed * Time.deltaTime));
            yield return null;
        }
        
        temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, 0);
        Destroy(obj.GetComponent<LoadingScreen>().loadingScreen.gameObject);
        Destroy(obj);
    }


	 IEnumerator displayLoadingScreen(string levelName){

		//affichage de l'ecran de chargement
        loadingScreen.gameObject.SetActive(true);
		//Initialisation du poucentage a 0%
		loadingPercentage.GetComponent<UnityEngine.UI.Text> ().text = percentage.ToString() + "%";

		AsyncOperation async = Application.LoadLevelAsync (levelName);

		//tant que le chargement de la scene n'est pas finis
		while (!async.isDone) {
			//on recupere la progression du la tache
			percentage =  (int) ( async.progress * 100 );
			loadingPercentage.GetComponent<UnityEngine.UI.Text> ().text = percentage.ToString() + "%";
			yield return null;
		}

	}

	 
}

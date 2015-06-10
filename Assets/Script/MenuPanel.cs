using UnityEngine;
using System.Collections;

public class MenuPanel : MonoBehaviour {

    public Animator anim;
    public string scene;
    public AudioClip _transitionSound;
    private AsyncOperation op;

    public GameObject loadingPercentage;
    private int percentage = 0;

	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(transform.parent.gameObject);

      //  anim.Play("Fermeture");

        //StartCoroutine(displayLoadingScreen(scene));

        ThemePlayerScript.instance.playTheme("Commedia dell'Arte Theme Principal");
	}

    public void startGame()
    {  
       // StartCoroutine("startScene");
        StartCoroutine(displayLoadingScreen(scene));
    }

    IEnumerator displayLoadingScreen(string levelName)
    {
        //affichage de l'ecran de chargement
       
        //Initialisation du poucentage a 0%
        loadingPercentage.GetComponent<UnityEngine.UI.Text>().text = percentage.ToString() + "%";

        op = Application.LoadLevelAsync(levelName);
        //op.allowSceneActivation = false;

        //tant que le chargement de la scene n'est pas finis
        while (!op.isDone)
        {
            //on recupere la progression du la tache
            percentage = (int)(op.progress * 100);
            loadingPercentage.GetComponent<UnityEngine.UI.Text>().text = percentage.ToString() + "%";

            yield return null;
        }
        transform.FindChild("MenuButtons").gameObject.SetActive(false);
        Destroy(transform.parent.gameObject);
        GetComponent<Canvas>().worldCamera = Camera.main;
        loadingPercentage.SetActive(false);
    }


    IEnumerator startScene()
    {
        this.GetComponent<AudioSource>().PlayOneShot(_transitionSound);
        ThemePlayerScript.instance.smoothThemeChange("Commedia Theme Redux", 0.5f, 2f , 3.5f);

        transform.FindChild("MenuButtons").gameObject.SetActive(false);
        //op.allowSceneActivation = true;

        anim.Play("Ouverture");

        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Wait"))
        {
            yield return null;
        }

       Destroy(transform.parent.gameObject);
    }

    public void restoreGame()
    {
        StartCoroutine("startScene");
    }

    public void credits()
    {
        StartCoroutine("startScene");
    }
}

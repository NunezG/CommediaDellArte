using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour {

    public GameObject menuCanvas, creditCanvas;

    public fadeAtStart _fadeObject;
    public Text loadingText;
    public Image cursor;
    public Animator anim;
    public string scene;
    public AudioClip _transitionSound;
    private AsyncOperation op;

    public GameObject loadingPercentage;
    private int percentage = 0;
    private bool done = false;
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
        if (!done)
        {
            StartCoroutine(displayLoadingScreen(scene));
            done = true;
        }
    }

    IEnumerator displayLoadingScreen(string levelName)
    {
        //affichage de l'ecran de chargement
       
        //Initialisation du poucentage a 0%
        loadingPercentage.GetComponent<UnityEngine.UI.Text>().text = percentage.ToString() + "%";

        op = Application.LoadLevelAsync(levelName);
        //op.allowSceneActivation = false;

        //tant que le chargement de la scene n'est pas finis
        //transform.FindChild("MenuButtons").gameObject.SetActive(false);

        while (!op.isDone)
        {
            //on recupere la progression du la tache
            percentage = (int)(op.progress * 100);
            loadingPercentage.GetComponent<UnityEngine.UI.Text>().text = percentage.ToString() + "%";

            yield return null;
        }
        Destroy(loadingText.gameObject);
        Destroy(cursor.gameObject);
        //transform.FindChild("MenuButtons").gameObject.SetActive(false);

        this.GetComponent<Canvas>().worldCamera = Camera.main;
        anim.SetTrigger("open");

        while (anim.GetCurrentAnimatorStateInfo(0).shortNameHash != Animator.StringToHash("open"))
        {
            Debug.Log("wait");
            yield return null;
        }

        this.GetComponent<AudioSource>().PlayOneShot(_transitionSound);

       //disparition progressive de l'UI

       StartCoroutine( fade(_fadeObject, 0, 0, 1f, 2));
       StartCoroutine(  fade(_fadeObject, 0, 1, 2));
       StartCoroutine(  fade(_fadeObject, 0, 2, 2));

       StartCoroutine(  fade(_fadeObject, 1, 0, 2));
       StartCoroutine( fade(_fadeObject, 1, 1, 2));

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length+5);

        Destroy(this.gameObject.GetComponent<Canvas>());
        Destroy(this.gameObject);
        Destroy(transform.parent.gameObject);

        GetComponent<Canvas>().worldCamera = Camera.main;
        loadingPercentage.SetActive(false);
    }


    public IEnumerator fade(fadeAtStart obj, int type, int index, float fadeSpeed, float delay = 0){


        yield return new WaitForSeconds(delay);

        if (type == 0)
        {

            while (obj.imageList[index].color.a > 0.1)
            {
                Debug.Log("doing");
                obj.imageList[index].color = new Color(obj.imageList[index].color.r, obj.imageList[index].color.g, obj.imageList[index].color.b, Mathf.Lerp(obj.imageList[index].color.a, 0, fadeSpeed * Time.deltaTime));
                yield return null;
            }

            obj.imageList[index].color = new Color(obj.imageList[index].color.r, obj.imageList[index].color.g, obj.imageList[index].color.b, 0);

            yield break;
        }
        else if (type == 1)
        {
            while (obj.textList[index].color.a > 0.1)
            {
                obj.textList[index].color = new Color(obj.textList[index].color.r, obj.textList[index].color.g, obj.textList[index].color.b, Mathf.Lerp(obj.textList[index].color.a, 0, fadeSpeed * Time.deltaTime));
                yield return null;
            }

            obj.textList[index].color = new Color(obj.textList[index].color.r, obj.textList[index].color.g, obj.textList[index].color.b, 0);

            yield break;
        }
        yield break;
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

    public void credits()
    {
        menuCanvas.gameObject.SetActive(false);
        creditCanvas.SetActive(true);
        creditCanvas.GetComponent<CreditScript>().appear();
    }
}

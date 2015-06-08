using UnityEngine;
using System.Collections;

public class MenuPanel : MonoBehaviour {

    public Animator anim;
    public string scene;
    public AudioClip _transitionSound;
    private AsyncOperation op;


	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(transform.parent.gameObject);

      //  anim.Play("Fermeture");

        op = Application.LoadLevelAsync(scene);
        op.allowSceneActivation = false;

        ThemePlayerScript.instance.playTheme("Commedia dell'Arte Theme Principal");
	}

    public void startGame()
    {  
        StartCoroutine("startScene");
    }

    IEnumerator startScene()
    {
        this.GetComponent<AudioSource>().PlayOneShot(_transitionSound);
        ThemePlayerScript.instance.smoothThemeChange("Commedia Theme Redux", 0.5f, 2f , 3.5f);

        transform.FindChild("MenuButtons").gameObject.SetActive(false);
        op.allowSceneActivation = true;

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

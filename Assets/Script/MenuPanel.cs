using UnityEngine;
using System.Collections;

public class MenuPanel : MonoBehaviour {

    public Animator anim;
    public string scene;
    private AsyncOperation op;

	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(transform.parent.gameObject);

      //  anim.Play("Fermeture");

        op = Application.LoadLevelAsync(scene);
        op.allowSceneActivation = false;
	}

    public void startGame()
    {  
        StartCoroutine("startScene");
    }

    IEnumerator startScene()
    {
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

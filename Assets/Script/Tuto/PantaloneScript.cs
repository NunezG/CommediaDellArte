using UnityEngine;
using System.Collections;

public class PantaloneScript : MonoBehaviour {

    public GameManager gameManager;

	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void questionEvent()
    {
        StartCoroutine(questionCoroutine());
    }

    IEnumerator questionCoroutine()
    {
        gameManager.guiManager.active = false;

        Vector3 moveEvent = new Vector3(-8, 7, 30);
        gameManager.getCharacterGameobject("Arlequin").GetComponent<CharacterController>().goTo(moveEvent);

        while (gameManager.getCharacterGameobject("Arlequin").transform.position != moveEvent)
        {
            yield return null;
        }
        gameManager.getCharacterGameobject("Arlequin").GetComponentInChildren<Animator>().SetTrigger("niceTalking");
        yield return new WaitForSeconds(0.5f);


        gameManager.getCharacterGameobject("Pantalone").GetComponentInChildren<Animator>().SetTrigger("asking");
        this.GetComponent<CharacterController>().bubble.showBubble(2, "Capitaine_love_Colombine");
        yield return new WaitForSeconds(2);

        gameManager.guiManager.active = true;
        yield break;
    }
}

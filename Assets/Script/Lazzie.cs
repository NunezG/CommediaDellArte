using UnityEngine;
using System.Collections;
using UnityEngine.UI; // required when using UI elements in scripts



public class Lazzie : MonoBehaviour {

    public GameManager gameManager;

    public CharacterController character;
    Button guiButton;

	// Use this for initialization
	void Start () {
       // gui = GameObject.Find("GUIManager").GetComponent<GUIManager>();
       // character = GetComponent<CharacterController>();
        guiButton = GetComponentInChildren<Button>();
	}
	
	// Update is called once per frame
	void Update () {

        if ((gameManager.guiManager.active && !guiButton.interactable) || (!gameManager.guiManager.active && guiButton.interactable))
        {
            guiButton.interactable = !guiButton.interactable;

        }
	}


    public void jugggle()
    {
        StartCoroutine(juggleCoroutine());
        guiButton.gameObject.SetActive(false);
    }
   

    IEnumerator juggleCoroutine()
    {

        gameManager.guiManager.active = false;

        Vector3 oldPosition = character.transform.position;

        Vector3 goToCenterEvent = new Vector3(-1.5f, 7, 30);

        character.goTo(goToCenterEvent);

        while (character.transform.position != goToCenterEvent)
        {
            Vector3 zoomLazzieEvent = new Vector3(character.transform.position.x+5, character.transform.position.y+5 , character.transform.position.z-25);
            gameManager.camera.moveTo(zoomLazzieEvent);

            yield return null;
        }

        character.GetComponentInChildren<Animator>().SetTrigger("juggling");
        yield return new WaitForSeconds(character.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.2f);

        gameManager.publicOnScene.happy(2, 2);
        gameManager.publicOnScene.setValue(75);

        gameManager.camera.resetPosition();

        character.goTo(oldPosition);
        while (character.transform.position != oldPosition)
        {

            yield return null;
        }

        gameManager.guiManager.active = true;


        yield break;
    }

   

}

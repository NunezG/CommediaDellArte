using UnityEngine;
using System.Collections;

public class bulleInfoScript : MonoBehaviour {


	public float fadeSpeed;
	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	
	}


	public void showBubble(float time){
		StartCoroutine (showBubbleCoroutine (time));
	}

	IEnumerator showBubbleCoroutine(float time){

		while (GetComponent<SpriteRenderer> ().color.a != 1) {
			if(GetComponent<SpriteRenderer> ().color.a>=0.9)
				GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
			else
				GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, Mathf.Lerp (GetComponent<SpriteRenderer> ().color.a, 1, fadeSpeed * Time.deltaTime));
			yield return null;
		}
		yield return new WaitForSeconds(time);
		while (GetComponent<SpriteRenderer> ().color.a != 0) {
			if(GetComponent<SpriteRenderer> ().color.a<=0.1)
				GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
			else
				GetComponent<SpriteRenderer> ().color = new Color(1,1,1,Mathf.Lerp (GetComponent<SpriteRenderer> ().color.a, 0, fadeSpeed * Time.deltaTime));
			yield return null;
		}
				
		yield break;
	}


}

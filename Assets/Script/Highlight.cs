using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpriteRenderer))]


public class Highlight : MonoBehaviour {


	//transparence minimum et maximum
	public float baseAlpha = 0, maxAlpha = 1;
	//vitesse d'apparition/disparition
	public float fadeSpeed;
	//collider qui active le highlight
	public Collider2D targetCollider;
	//vrai quand le surseur de la souris est dessus
	public bool overlap = false;
	//permet d'afficher ou non le sprite
	private bool show = true;

	void LateUpdate () {

		if (!show || targetCollider.enabled == false) {
			this.GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255, 0);
			return;
		} 
		else {
			//si le curseur de la souris est dans le Collider
			if(overlap)
				this.GetComponent<SpriteRenderer> ().color = new Color(255,255,255,Mathf.Lerp (this.GetComponent<SpriteRenderer> ().color.a, maxAlpha,
				                                                                               fadeSpeed));
			else
				this.GetComponent<SpriteRenderer> ().color = new Color(255,255,255 ,Mathf.Lerp (this.GetComponent<SpriteRenderer> ().color.a,baseAlpha, 
				                                                                                fadeSpeed));
		}
		overlap = false;
	}


	public void enable(){
		show = true;
	}
	public void disable(){
		show = false;
	}


}

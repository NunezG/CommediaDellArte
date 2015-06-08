using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (Collider2D))]

public class RadialButtonScript :  MonoBehaviour {

	//objet qui possede le script RadialMenu avec cet objet en action
	public RadialMenuScript parentObject;
	//vitesse d'apparition
	public float fadeSpeed = 0.2f, moveSpeed = 0.2f;
	//vrai si l'action est débloquée
	public bool active = true;

	//permet de recuperer la position dans le script RadialMenu
	[HideInInspector] public int index;
	//permet de gerer l'affichage du button
	[HideInInspector] public bool show = true;
	//permet de gerer la position de l'objet
	[HideInInspector] public Vector3 positionOffset, parentOldPos;

	private Collider2D _collider;
	private SpriteRenderer _spriteRenderer;

	void Start () {
		GetComponent<SpriteRenderer> ().color = new Color(1,1,1,0);
		_collider = this.GetComponent<Collider2D> ();
		_spriteRenderer = this.GetComponent<SpriteRenderer> ();
	}

	
	// Update is called once per frame
	void Update () {

		this.transform.position += parentObject.transform.position - parentOldPos;

		//Si le button est affiché
		if (show) {
			//si l'action est disponible
			if(active){
				_spriteRenderer.color = new Color(1,1,1,Mathf.Lerp (_spriteRenderer.color.a, 1, fadeSpeed * Time.deltaTime));
			}
			else{
				_spriteRenderer.color = new Color(0.5f,0.5f,0.5f,Mathf.Lerp (_spriteRenderer.color.a, 1, fadeSpeed * Time.deltaTime));
			}

			//gestion du mouvement lors de l'apparition
			this.transform.position = Vector3.Lerp( this.transform.position, parentObject.transform.position + positionOffset, moveSpeed * Time.deltaTime);

		}
		else{	
			if(active){
				_spriteRenderer.color = new Color(1,1,1,Mathf.Lerp (_spriteRenderer.color.a, 0, fadeSpeed * Time.deltaTime));
			}
			else{
                _spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, Mathf.Lerp(_spriteRenderer.color.a, 0, fadeSpeed * Time.deltaTime));
			}

			//gestion du mouvement lors de la disparition
			this.transform.position = Vector3.Lerp( this.transform.position, parentObject.transform.position ,  moveSpeed *Time.deltaTime);
		}

		parentOldPos = parentObject.transform.position;
	}

	public void setDisplay(bool display){

		if (display) {
			this.GetComponent<Collider2D> ().enabled = true;
			show = true;
		}
		else {
			this.GetComponent<Collider2D> ().enabled = false;
			show = false;
		}
	}

	public void setActive(bool b){
		active = b;
	}
	


}

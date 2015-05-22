using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Camera))]

public class CameraScript : MonoBehaviour {

	public float moveSmoothTime = 1, speed;
	public Vector3 next = Vector3.zero, velocity;

	private IEnumerator moveCoroutine;	
	//Variables pour la gestion du deplacement et le zoom
	private Vector3 nextPosition, newPosition, originalPosition;
	private float xVelocity = 0.0f, yVelocity = 0.0f, zVelocity = 0.0f;
	
	CoroutineParameters cParams;
	
	void Start () {
		
		//sauvegarde de la postion orignal
		originalPosition = this.transform.position;
		
		//initialisation des variables pour les coroutines
		nextPosition = this.transform.position;

		next = nextPosition;

		cParams = new CoroutineParameters ();
		cParams.position = nextPosition;
		cParams.speed = moveSmoothTime;
		
		//initialisation des coroutines
		//moveCoroutine = updatePosition (cParams);
		//StartCoroutine (moveCoroutine);
		
	}
	
	void Update () {
		
		//Debug.Log (nextPosition);
		
		//Example
		/*if(Input.GetButtonDown("Fire1") ){
			
			Vector3 mousePosition = new Vector3();
			mousePosition = Input.mousePosition;
			mousePosition.z = 10;
			
			nextPosition = Camera.main.ScreenToWorldPoint(mousePosition);
			nextPosition.z = this.transform.position.z;

			moveTo(nextPosition);
			
		}*/


		transform.position = Vector3.SmoothDamp(transform.position, next,ref velocity, speed * Time.deltaTime);

	}
	
	//deplacement
	public void moveTo(Vector3 nextPosition, float speed  ){

	/*	Vector3 newpos = new Vector3 ();
		newpos.x = nextPosition.x;
		newpos.y = nextPosition.y;
		newpos.z = nextPosition.z;
		cParams.position = newpos;
		cParams.speed = speed;*/
		next = nextPosition;


	}	
	public void moveTo(Vector3 nextPosition ){
		moveTo(nextPosition, moveSmoothTime);
	}
	
	//zoom
	public void zoom(float zoomValue){
		cParams.position.z = nextPosition.z / zoomValue;
	}
	
	
	//retourne a la position original
	public void resetPosition(float speed){
		cParams.position = originalPosition;
		cParams.speed = speed;
		next = originalPosition;
	}
	public void resetPosition(){
		resetPosition(moveSmoothTime);
	}
	
	//coroutine pour gerer le deplacement
	IEnumerator updatePosition(CoroutineParameters param ){
		while (true) {
			if (param.active && this.transform.position != param.position) {
				newPosition.x = Mathf.SmoothDamp (this.transform.position.x, param.position.x, ref xVelocity, param.speed);
				newPosition.y = Mathf.SmoothDamp (this.transform.position.y, param.position.y, ref yVelocity, param.speed);
				newPosition.z = Mathf.SmoothDamp (this.transform.position.z, param.position.z, ref zVelocity, param.speed);
				this.transform.position = newPosition;
				yield return null;
			}
			yield return null;
		}
	}
	
}

public class CoroutineParameters{
	public Vector3 position;
	public float speed = 0.5f;
	public bool active = true;
}


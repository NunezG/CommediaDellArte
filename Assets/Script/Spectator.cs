using UnityEngine;
using System.Collections;

public class Spectator : MonoBehaviour {


	public enum Direction{left,right};

	public int publicType;
	public Direction walkAwayDirection;

	// Use this for initialization
	void Start () {	

		this.transform.GetChild (0).GetComponent<Animator> ().SetInteger ("type", publicType);

		if (walkAwayDirection == Direction.left)
			this.transform.GetChild (0).GetComponent<Animator> ().SetInteger ("walkDirection", 0);
		else if(walkAwayDirection == Direction.right)
			this.transform.GetChild (0).GetComponent<Animator> ().SetInteger ("walkDirection", 1);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

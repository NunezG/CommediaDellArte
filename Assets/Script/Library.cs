using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Library{

	//return the x and y of a vector3
	public static Vector2 toVector2(this Vector3 vector3){
		return new Vector2(vector3.x, vector3.y);
	}
	public static Vector3 toVector3(this Vector2 vector2, float newZ = 0){
		return new Vector3(vector2.x, vector2.y, newZ);
	}

	//return true is the mouse in on the collider2D od the object
	public static bool overlapMouse(this Collider2D collider){
		Vector3 position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, collider.transform.position.z - Camera.main.transform.position.z);
		return collider.OverlapPoint ( (Camera.main.ScreenToWorldPoint (position)).toVector2 ());
	}


	//return the position of the mouse on a Z plane
	public static Vector2 getMousePosition(float z){
		Vector3 position = new Vector3 ();
		position = Input.mousePosition;
		position.z = z;
		return Camera.main.ScreenToWorldPoint (position);
	}




}


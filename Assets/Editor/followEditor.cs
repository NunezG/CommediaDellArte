using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;


[CustomEditor(typeof(follow))]
public class followEditor : Editor 
{

		public override void OnInspectorGUI()
	{

		follow myTarget = (follow)target;

		myTarget.target = (GameObject) EditorGUILayout.ObjectField ("Target :" ,myTarget.target, typeof(GameObject), true);

		myTarget.offsetType =(follow.Direction) EditorGUILayout.EnumPopup("Offset Type :",myTarget.offsetType);
		if (myTarget.offsetType != follow.Direction.None) {
			myTarget.offset = EditorGUILayout.Vector3Field ("Offset :", myTarget.offset);
		}



		myTarget.followPosition = EditorGUILayout.Toggle("Follow Position", myTarget.followPosition);
		if (myTarget.followPosition) {

			GUILayout.BeginHorizontal (GUILayout.Width(10));

			GUILayout.Space(10);
			GUILayout.Label("X :");
			myTarget.positionX = GUILayout.Toggle( myTarget.positionX, GUIContent.none);
			GUILayout.Label("Y :");
			myTarget.positionY = GUILayout.Toggle( myTarget.positionY, GUIContent.none);
			GUILayout.Label("Z :");
			myTarget.positionZ = GUILayout.Toggle( myTarget.positionZ, GUIContent.none);

			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal (GUILayout.Width(5));
			GUILayout.Space(10);
			GUILayout.Label("Smooth Damp :");
			myTarget.smoothDamp = GUILayout.Toggle( myTarget.smoothDamp,  GUIContent.none);
			if (myTarget.smoothDamp) {
				myTarget.smoothTime = EditorGUILayout.FloatField ("Smooth Time :", myTarget.smoothTime);
			}
			GUILayout.EndHorizontal ();

		}
		myTarget.followRotation = EditorGUILayout.Toggle("Follow Rotation", myTarget.followRotation);
		if (myTarget.followRotation) {

			GUILayout.BeginHorizontal (GUILayout.Width(10));
			GUILayout.Space(10);
			GUILayout.Label("X :");
			myTarget.rotationX = GUILayout.Toggle( myTarget.rotationX, GUIContent.none);
			GUILayout.Label("Y :");
			myTarget.rotationY = GUILayout.Toggle( myTarget.rotationY, GUIContent.none);
			GUILayout.Label("Z :");
			myTarget.rotationZ = GUILayout.Toggle( myTarget.rotationZ, GUIContent.none);
			
			GUILayout.EndHorizontal ();
		}

	}




}


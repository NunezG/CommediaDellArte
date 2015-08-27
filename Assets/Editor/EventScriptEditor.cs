using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Reflection;
using System;


[CustomEditor(typeof(EventScript))]
public class EventScriptEditor: Editor {

	public enum Combinaison{And, Or};//type enuméré pour le test logique
	public enum MultipleComparison{Sup, Inf, Equal, NotEqual, EqualInf, EqualSup};//type enuméré pour le test logique
	public enum SimpleComparison{Equal, NotEqual};//type enuméré pour le test logique
	
	SimpleComparison simpleComparisonType;

	SerializedObject eventManager ;
	SerializedProperty componentIndexList,propertyIndexList,objectList,logicalTestList, sastisfiedEvent, test;
	
	void OnEnable(){
		eventManager = new SerializedObject (target);
		componentIndexList = eventManager.FindProperty ("componentIndexList");
		propertyIndexList = eventManager.FindProperty ("propertyIndexList");
		objectList = eventManager.FindProperty ("objectList");
		logicalTestList = eventManager.FindProperty ("logicalTestList");
		sastisfiedEvent = eventManager.FindProperty ("sastisfiedEvent");
		test = eventManager.FindProperty ("test");
	}
	
	public override void OnInspectorGUI(){

		EventScript myTarget = (EventScript)target;

		serializedObject.Update();

		for (int num = 0; num < objectList.arraySize; num++) {
		
			EditorGUILayout.BeginVertical (GUILayout.Width(50));

			EditorGUILayout.BeginHorizontal (GUILayout.Width(50));
			GUILayout.Label("Target :",GUILayout.Width(80));
			objectList.GetArrayElementAtIndex(num).objectReferenceValue = EditorGUILayout.ObjectField (objectList.GetArrayElementAtIndex(num).objectReferenceValue , typeof(GameObject), true, GUILayout.MinWidth (150));	//objet ciblé 
			EditorGUILayout.EndHorizontal();

			if (objectList.GetArrayElementAtIndex(num).objectReferenceValue  != null) {

				myTarget.componentList = ((GameObject)objectList.GetArrayElementAtIndex(num).objectReferenceValue ).GetComponents<Component> ();//liste des composant de cet objet
								
				if(myTarget.componentList.Length > 0 ){

					string[] options = new string[myTarget.componentList.Length];//liste des noms ( string ) des composants de cet objet
					
					for (int i = 0; i < myTarget.componentList.Length; i++) {
						options[i] = myTarget.componentList[i].GetType().Name;
					}

					EditorGUILayout.BeginHorizontal (GUILayout.Width(100));
					GUILayout.Label("Component :",GUILayout.Width(80) );
					componentIndexList.GetArrayElementAtIndex(num).intValue = EditorGUILayout.Popup(componentIndexList.GetArrayElementAtIndex(num).intValue, options,GUILayout.MinWidth (150)); // index pour la selection du composant
					EditorGUILayout.EndHorizontal();

					myTarget.fieldList  =  myTarget.componentList[	componentIndexList.GetArrayElementAtIndex(num).intValue].GetType().GetFields(BindingFlags.Instance | 
					                                                                        BindingFlags.Static |
					                                                                       BindingFlags.NonPublic |
					                                                                         BindingFlags.Public); //liste des attributs du composant					
					if(myTarget.fieldList.Length >0){
						
						string[] propertiesOptions = new string[myTarget.fieldList.Length];//liste des noms (string ) des attributs du composant
						
						for (int i = 0; i < propertiesOptions.Length; i++) {
							propertiesOptions[i] = myTarget.fieldList[i].Name;
						}
						EditorGUILayout.BeginHorizontal (GUILayout.Width(50));
						GUILayout.Label("Atribute :",GUILayout.Width(80));
						propertyIndexList.GetArrayElementAtIndex(num).intValue = EditorGUILayout.Popup(propertyIndexList.GetArrayElementAtIndex(num).intValue, propertiesOptions,GUILayout.MinWidth (150));// index pour la selection de l'attribut
						EditorGUILayout.EndHorizontal();
					}
				}
			}
			EditorGUILayout.EndVertical();

			//field pour le test logique
			if (objectList.GetArrayElementAtIndex(num).objectReferenceValue  != null && myTarget.fieldList.Length > 0) {
			
				EditorGUILayout.BeginHorizontal (GUILayout.Width(50),GUILayout.MaxHeight(50) );


				switch(myTarget.fieldList[propertyIndexList.GetArrayElementAtIndex(num).intValue].FieldType.Name){
					case("Object"):
						EditorGUILayout.ObjectField(new UnityEngine.Object() ,typeof(GameObject), true );
						break;
					case("String"):
						EditorGUILayout.TextField(" ");
						break;
					case("Boolean"):
						drawBoolTestField(myTarget, num);
						break;
					case("int32"):
						EditorGUILayout.IntField(0);
						break;
					case("Single"):
						EditorGUILayout.FloatField(0);
						break;
					case("Double"):
						EditorGUILayout.DoubleField(0);
						break;				
				}
				EditorGUILayout.EndHorizontal();
			}
	}

		//button pour la gestion de la taille de la liste des objets
		EditorGUILayout.BeginHorizontal (GUILayout.MaxWidth(200));
		if (GUILayout.Button ("+")) {
			objectList.InsertArrayElementAtIndex(objectList.arraySize);
			componentIndexList.InsertArrayElementAtIndex(componentIndexList.arraySize);
			propertyIndexList.InsertArrayElementAtIndex(propertyIndexList.arraySize);
			logicalTestList.InsertArrayElementAtIndex(logicalTestList.arraySize);
		}
		if (GUILayout.Button ("-") && objectList.arraySize > 0 ) {
			objectList.DeleteArrayElementAtIndex(objectList.arraySize-1);
			componentIndexList.DeleteArrayElementAtIndex(componentIndexList.arraySize-1);
			propertyIndexList.DeleteArrayElementAtIndex(propertyIndexList.arraySize-1);
			logicalTestList.DeleteArrayElementAtIndex(logicalTestList.arraySize-1);

		}


		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.PropertyField (test);

		EditorGUILayout.PropertyField (sastisfiedEvent);

		//Debug.Log ("list size = " +componentIndexList.arraySize);
		//Debug.Log (" test list size = " + logicalTestList.arraySize);

		eventManager.ApplyModifiedProperties();

	}

	
	public void drawBoolTestField(EventScript myTarget, int num ){

		SerializedProperty comp = eventManager.FindProperty ("comparisonTestList");

		//EditorGUILayout.EnumPopup (comp.GetArraElementAtIndex(num).stringValue);

		logicalTestList.GetArrayElementAtIndex(num).objectReferenceValue  =  CreateInstance("LogicalBooleanTest");

		//Debug.Log(((LogicalBooleanTest)logicalTestList.GetArrayElementAtIndex(num).objectReferenceValue).target[1]);
		//((LogicalBooleanTest)logicalTestList.GetArrayElementAtIndex(num).objectReferenceValue).target[1] =  EditorGUILayout.Toggle (((LogicalBooleanTest)logicalTestList.GetArrayElementAtIndex(num).objectReferenceValue).target[1] );
		//((LogicalBooleanTest)logicalTestList.GetArrayElementAtIndex(num).objectReferenceValue).comparisonTest = EditorGUILayout.EnumPopup(((LogicalBooleanTest)logicalTestList.GetArrayElementAtIndex(num).objectReferenceValue).comparisonTest );

		((LogicalBooleanTest)logicalTestList.GetArrayElementAtIndex(num).objectReferenceValue).init(ref myTarget.componentList[componentIndexList.GetArrayElementAtIndex(num).intValue],
		                                                                                            ref myTarget.fieldList[propertyIndexList.GetArrayElementAtIndex(num).intValue] ,
		                                                                                            true,SimpleComparison.Equal );
	

	}

	
}




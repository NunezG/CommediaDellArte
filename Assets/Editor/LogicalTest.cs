using UnityEngine;
using System.Collections;

public class LogicalTest: ScriptableObject 
{

	public virtual bool checkTest(){
		return false;
	}
	public virtual string getTestType(){
		return "none";
	}
}


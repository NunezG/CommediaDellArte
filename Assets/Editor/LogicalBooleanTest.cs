using UnityEngine;
using System.Collections;
using System.Reflection;

public class LogicalBooleanTest : LogicalTest
{
	
	public bool[] target;
	public EventScriptEditor.SimpleComparison comparisonTest;

	public LogicalBooleanTest(){
		target = new bool[2];
	}

	public void init(ref Component comp, ref FieldInfo field, bool value,EventScriptEditor.SimpleComparison comparisonTest  ){	

		//Debug.Log ( (bool)field.GetValue(comp) );
		this.target = new bool[2];
		this.target [0] = (bool)field.GetValue(comp);
		this.target [1] = value;
		this.comparisonTest = comparisonTest;
	}

	public override bool checkTest(){

		Debug.Log ("target value : " + target [0] + "  value to compare : " + target [1]);

		switch (comparisonTest) {
			case(EventScriptEditor.SimpleComparison.Equal):
				if(target[0] == target [1])
					return true;
				else
					return false;
				break;
			case(EventScriptEditor.SimpleComparison.NotEqual):
				if(target[0] != target [1])
					return true;
				else
					return false;
				break;
		}

		return false;

	}

	public override string getTestType(){
		return "boolean";
	}
}


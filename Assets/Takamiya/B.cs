using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B : A {

	public static List<int> stList = new List<int>();

	public B(){
		for(int i = 0; i < 10000; i++){
			stList.Add (i);
		}
	}

	public override void MethodA(){
		Debug.Log ("B");
	}

	public void MethodB(){
		Debug.Log ("B");
	}
}

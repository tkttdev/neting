using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C : MonoBehaviour {

	B b = new B ();

	void Start(){
		for (int i = 0; i < B.stList.Count; i++) {
			Debug.Log (B.stList [i]);
		}
	}
}

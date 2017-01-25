using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerComponent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.MoveAdd (gameObject, iTween.Hash ("x", 0.3f, "y", 0.3f, "loopType", iTween.LoopType.pingPong,"easeType",iTween.EaseType.linear));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

	[SerializeField] private GameObject warpPurposeObject;
	[HideInInspector] public Vector3 warpPos;

	void Start(){
		warpPos = warpPurposeObject.transform.position;
		iTween.RotateAdd(this.gameObject, iTween.Hash("loopType", iTween.LoopType.loop, "easeType",iTween.EaseType.linear, "z", -360, "time", 3));
	}
}

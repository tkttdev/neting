using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

	[SerializeField] private GameObject warpPurposeObject;
	[SerializeField] private GameObject[] cornerOfBothSides = new GameObject[2];
	[HideInInspector] public Vector3 warpPos;
	[HideInInspector] public string afterWarpLineId;


	void Start(){
		warpPos = warpPurposeObject.transform.position;
		afterWarpLineId = (cornerOfBothSides [0].GetInstanceID () > cornerOfBothSides [1].GetInstanceID ()) ? 
			cornerOfBothSides [0].GetInstanceID ().ToString () + cornerOfBothSides [1].GetInstanceID ().ToString () : 
			cornerOfBothSides [1].GetInstanceID ().ToString () + cornerOfBothSides [0].GetInstanceID ().ToString ();
		iTween.RotateAdd(this.gameObject, iTween.Hash("loopType", iTween.LoopType.loop, "easeType",iTween.EaseType.linear, "z", -360, "time", 3));
	}
}

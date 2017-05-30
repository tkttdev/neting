﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(order = 120)]
public class CasheCornerData : ScriptableObject {
	public Dictionary<string, Vector2> straightPurposeData = new Dictionary<string, Vector2>();
	public Dictionary<string, Transform[]> curveData = new Dictionary<string, Transform[]>();
	public Dictionary<string, float> lengthData = new Dictionary<string, float>();
	public Dictionary<string, float[]> curveSectionLengthData = new Dictionary<string, float[]>(); 
	public Dictionary<string, string> lineIdData = new Dictionary<string, string>();
	public Dictionary<string, MoveDir> moveDirData = new Dictionary<string, MoveDir>();
}

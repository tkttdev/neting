using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(order = 120)]
public class CasheCornerData : ScriptableObject {
	public Dictionary<int, Vector2> slopeData = new Dictionary<int, Vector2>();	
	public Dictionary<int, int> lineIdData = new Dictionary<int, int>();
	//public Dictionary<int, MoveObjectBase.MoveDir> moveDirData = new Dictionary<int, MoveObjectBase.MoveDir>();
}

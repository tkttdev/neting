using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner : MonoBehaviour {
	[NamedArrayAttribute(new string[] { "UP", "RIGHT", "DOWN", "LEFT" })]
	public Transform[] purposeTransform = new Transform[4];
	public List<Transform> upBezerTransform = new List<Transform> ();
	public List<Transform> rightBezerTransform = new List<Transform> ();
	public List<Transform> downBezerTransform = new List<Transform> ();
	public List<Transform> leftBezerTransform = new List<Transform> ();
	public bool[] isCurve = new bool[4];
	//public List<Transform>[] bezerControlTransform = new List<Transform>[4] ();
	//public List<Transform> bezerControlTransform = new List<Transform> ();
	private Vector2[] slope = new Vector2[4];
	private string[] lineId = new string[5];
	[SerializeField] private bool onlyEnemy;
	[SerializeField] private bool onlyBullet;
	[SerializeField] private bool onlyForward;
	//public bool[] isCurve = new bool[5];

	private void Awake(){
		int id = gameObject.GetInstanceID ();
		for (int i = 0; i < 4; i++) {
			if (purposeTransform [i] == null) {
				slope [i] = Vector2.zero;
				lineId [i] = "";
				continue;
			}
			int pid = purposeTransform [i].gameObject.GetInstanceID ();
			slope [i] = (purposeTransform [i].position - transform.position).normalized;
			lineId [i] = (id > pid) ? id.ToString () + pid.ToString () : pid.ToString () + id.ToString ();
		}
	}

	// corner tag : RightCorner, LeftCorner, PassCorner, CurveCorner
	// TODO : より良いコードで実装し直し(Vector2の参照渡しがなぜできない？)
	public Vector2 ChangePurpose(ref MoveDir _moveDir, int _moveDesMode, ref string _lineId){
		if ((_moveDir == MoveDir.RIGHT || _moveDir == MoveDir.LEFT) && tag != "PassCorner") {
			if (_moveDesMode == 1) {
				_moveDir = MoveDir.UP;
				_lineId = lineId [0];
				return slope [0];
			} else {
				_moveDir = MoveDir.DOWN;
				_lineId = lineId [2];
				return slope [2];
			}
		} 

		if (onlyForward) {
			if (_moveDesMode == 1) {
				_moveDir = MoveDir.UP;
				_lineId = lineId [0];
				return slope [0];
			} else {
				_moveDir = MoveDir.DOWN;
				_lineId = lineId [2];
				return slope [2];
			}
		} else if ((onlyEnemy && _moveDir == MoveDir.DOWN) || (onlyBullet && _moveDir == MoveDir.UP)) {
			if (transform.tag == "RightCorner") {
				_moveDir = MoveDir.RIGHT;
				_lineId = lineId [2];
				return slope [2];
			} else {
				_moveDir = MoveDir.LEFT;
				_lineId = lineId [3];
				return slope [3];
			}
		} else if(onlyEnemy || onlyBullet) {
			_lineId = lineId [(int)_moveDir];
			return slope [(int)_moveDir];
		}

		if (transform.tag == "RightCorner") {
			if (_moveDir == MoveDir.RIGHT) {
			}
			_moveDir = MoveDir.RIGHT;
			_lineId = lineId [1];
			return slope [1];
		} else if (transform.tag == "LeftCorner") {
			_moveDir = MoveDir.LEFT;
			_lineId = lineId [3];
			return slope [3];
		} else if (transform.tag == "PassCorner") {
			_lineId = lineId [(int)_moveDir];
			return slope [(int)_moveDir];
		}

		Debug.Log ("Error Case Corner " + transform.name);
		return Vector2.zero;
	}

	#if UNITY_EDITOR
	private void OnDrawGizmos(){
		UnityEditor.Handles.Label(transform.position, name);
		bool isConnected = false;
		for (int i = 0; i < 4; i++) {
			if (isCurve [i]) {
				List<Transform> drawTransform = new List<Transform> ();
				switch (i) {
				case 0:
					for (int j = 0; i < upBezerTransform.Count; i++) {
						if (upBezerTransform [j] == null) {
							break;
						}
					}
					drawTransform = new List<Transform> (upBezerTransform);
					break;
				case 1:
					for (int j = 0; i < rightBezerTransform.Count; i++) {
						if (rightBezerTransform [j] == null) {
							break;
						}
					}
					drawTransform = new List<Transform> (rightBezerTransform);
					break;
				case 2:
					for(int j = 0; i < downBezerTransform.Count; i++){
						if(downBezerTransform[j] == null){
							break;
						}
					}
					drawTransform = new List<Transform> (downBezerTransform);
					break;
				case 3:
					for (int j = 0; i < leftBezerTransform.Count; i++) {
						if (leftBezerTransform [j] == null) {
							break;
						}
					}
					drawTransform = new List<Transform> (leftBezerTransform);
					break;
				}
				if (drawTransform.Count > 0) {
					Gizmos.color = Color.red;
					Gizmos.DrawLine (transform.position, upBezerTransform [0].position);
					for (int j = 1; j < drawTransform.Count; j++) {
						Gizmos.DrawLine (drawTransform [j-1].position, drawTransform [j].position);
					}
				}
			} else {
				if (purposeTransform [i] == null) {
					continue;
				}
				var corner = purposeTransform [i].GetComponent<Corner> ();
				if (corner == null) {
					continue;
				}
				isConnected = corner.purposeTransform [(i + 2) % 4] == gameObject.transform;
				if (isConnected) {
					Gizmos.color = Color.red;
					Gizmos.DrawLine (transform.position, purposeTransform [i].transform.position);
				} else {
					Gizmos.color = Color.blue;
					Gizmos.DrawLine (transform.position, purposeTransform [i].transform.position);
				}
			}
		}
	}
	#endif
}

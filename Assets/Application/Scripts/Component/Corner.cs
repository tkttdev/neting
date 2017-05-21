using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Corner : MonoBehaviour {
	[NamedArrayAttribute(new string[] { "UP", "RIGHT", "DOWN", "LEFT" })]
	public Transform[] purposeTransform = new Transform[4];
	[NamedArrayAttribute(new string[] { "START", "POS1", "POS2", "END", "START", "POS1", "POS2", "END", "START", "POS1", "POS2", "END", "START", "POS1", "POS2", "END"})]
	public Transform[] bezerPoints = new Transform[16];
	[NamedArrayAttribute(new string[] { "UP CURVE", "RIGHT CURVE", "DOWN CURVE", "LEFT CURVE" })]
	public bool[] isCurve = new bool[4];

	private int bezerFineness = 20;
	private Vector2[] slope = new Vector2[4];
	private string[] lineId = new string[5];
	[SerializeField] private bool onlyEnemy;
	[SerializeField] private bool onlyBullet;
	[SerializeField] private bool onlyForward;

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

	private Vector3 Bezer3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		var oneMinusT = 1f - t;
		return oneMinusT * oneMinusT * oneMinusT * p0 +
			3f * oneMinusT * oneMinusT * t * p1 +
			3f * oneMinusT * t * t * p2 +
			t * t * t * p3;
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

	public bool CheckCurve(MoveDir _moveDir, int _moveDesMode){
		if (tag == "PassCorner") {
			return isCurve [(int)_moveDir];
		}

		return false;
	}

	#if UNITY_EDITOR
	private void OnDrawGizmos(){
		UnityEditor.Handles.Label(transform.position, name);
		bool isConnected = false;
		for (int i = 0; i < 4; i++) {
			if (isCurve [i]) {
				for (int j = 0; j < 4; j++) {
					if (bezerPoints [i * 4 + j] == null) {
						return;
					}
				}
				var purposeCorner = bezerPoints [i * 4 + 3].gameObject.GetComponent<Corner> ();
				if (purposeCorner != null) {
					isConnected = transform == purposeCorner.bezerPoints [(i + 2) % 4 * 4 + 3];
				}
				Gizmos.color = isConnected ? Color.red : Color.blue;
				float t = 0.0f;
				for (int k = 0; k < bezerFineness; k++) {
					Vector3 tmp1 = Bezer3 (bezerPoints [i * 4].position, bezerPoints [i * 4 + 1].position, bezerPoints [i * 4 + 2].position, bezerPoints [i * 4 + 3].position, t);
					Vector3 tmp2 = Bezer3 (bezerPoints [i * 4].position, bezerPoints [i * 4 + 1].position, bezerPoints [i * 4 + 2].position, bezerPoints [i * 4 + 3].position, Mathf.Clamp (t + 1f/bezerFineness, 0.0f, 1.0f));
					t += 1f / bezerFineness;
					Gizmos.DrawLine (tmp1, tmp2);
				}
			} else {
				if (purposeTransform [i] == null) {
					continue;
				}
				var purposeCorner = purposeTransform [i].GetComponent<Corner> ();
				if (purposeCorner == null) {
					continue;
				}
				isConnected = purposeCorner.purposeTransform [(i + 2) % 4] == gameObject.transform;
				isConnected = !purposeCorner.isCurve [(i + 2) % 4];
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

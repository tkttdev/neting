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
	public float[] curveLength = new float[4];

	private int bezerFineness = 100;
	private Vector2[] slope = new Vector2[4];
	private string[] lineId = new string[5];
	[SerializeField] private bool onlyEnemy;
	[SerializeField] private bool onlyBullet;
	[SerializeField] private bool onlyForward;

	private void Awake(){
		int id = gameObject.GetInstanceID ();
		for (int i = 0; i < 4; i++) {
			if (isCurve [i]) {
				bool isConnectCurve = true;
				for (int j = 0; j < 4; j++) {
					if (bezerPoints [i * 4 + j] == null) {
						isConnectCurve = false;
					}
				}
				if (!isConnectCurve) {
					continue;
				}
				curveLength [i] = GetCurveLength (i);
				Debug.Log (curveLength [i]);
			} else { 
				if (purposeTransform [i] == null) {
					slope [i] = Vector2.zero;
					lineId [i] = "";
					continue;
				}
				slope [i] = (purposeTransform [i].position - transform.position).normalized;
			}
			int pid = purposeTransform [i].gameObject.GetInstanceID ();
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
	public Vector2 ChangePurposeStraight(ref MoveDir _moveDir, int _moveDesMode, ref string _lineId){
		_moveDir = GetNextMoveDir (_moveDir, _moveDesMode);
		_lineId = lineId [(int)_moveDir];
		return slope [(int)_moveDir];
	}

	public Transform[] ChangePurposeCurve(ref MoveDir _moveDir, int _moveDesMode, ref string _lineId, ref float _curveLength) {
		_moveDir = GetNextMoveDir (_moveDir, _moveDesMode);
		_lineId = lineId [(int)_moveDir];
		_curveLength = curveLength [(int)_moveDir];
		Transform[] points = new Transform[4];
		Array.Copy (bezerPoints, (int)_moveDir * 4, points, 0, 4);
		return points;
	}

	private MoveDir GetNextMoveDir(MoveDir _moveDir, int _moveDesMode){
		if (onlyForward) {
			if (_moveDesMode == 1) {
				return MoveDir.UP;
			} else {
				return MoveDir.DOWN;
			}
		} else if ((onlyEnemy && _moveDir == MoveDir.DOWN) || (onlyBullet && _moveDir == MoveDir.UP)) {
			if (transform.tag == "RightCorner") {
				return MoveDir.RIGHT;
			} else {
				return MoveDir.DOWN;
			}
		} else if ((onlyEnemy && _moveDesMode == -1) || (onlyBullet && _moveDesMode == 1)) {
			if (_moveDesMode == 1) {
				return MoveDir.UP;
			} else {
				return MoveDir.DOWN;
			}
		} else if(onlyEnemy || onlyBullet) {
			if (_moveDesMode == 1) {
				return MoveDir.UP;
			} else {
				return MoveDir.DOWN;
			}
		}

		if ((_moveDir == MoveDir.RIGHT || _moveDir == MoveDir.LEFT) && tag != "PassCorner") {
			if (_moveDesMode == 1) {
				return MoveDir.UP;
			} else {
				return MoveDir.DOWN;
			}
		} 

		if (transform.tag == "RightCorner") {
			return MoveDir.RIGHT;
		} else if (transform.tag == "LeftCorner") {
			return MoveDir.LEFT;
		} else if (transform.tag == "PassCorner") {
			return _moveDir;
		}

		Debug.Log ("Error Case Corner (GetNextMoveDir)");
		Debug.Log (tag);
		Debug.Log (_moveDir);
		Debug.Log (_moveDesMode);
		return MoveDir.UP;
	}

	public bool CheckCurve(MoveDir _moveDir, int _moveDesMode){
		return isCurve [(int)GetNextMoveDir (_moveDir, _moveDesMode)];
	}

	public float GetCurveLength(int _index){
		float length = 0.0f;
		float t = 0.0f;
		for (int k = 0; k < bezerFineness; k++) {
			Vector3 tmp1 = Bezer3 (bezerPoints [_index * 4].position, bezerPoints [_index * 4 + 1].position, bezerPoints [_index * 4 + 2].position, bezerPoints [_index * 4 + 3].position, t);
			Vector3 tmp2 = Bezer3 (bezerPoints [_index * 4].position, bezerPoints [_index * 4 + 1].position, bezerPoints [_index * 4 + 2].position, bezerPoints [_index * 4 + 3].position, Mathf.Clamp (t + 1f/bezerFineness, 0.0f, 1.0f));
			t += 1f / bezerFineness;
			length += (tmp2 - tmp1).magnitude;
		}
		return length;
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

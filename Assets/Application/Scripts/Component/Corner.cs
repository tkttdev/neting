using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Corner : MonoBehaviour {
	[NamedArrayAttribute(new string[] { "UP", "RIGHT", "DOWN", "LEFT" })]
	public Transform[] purposeTransform = new Transform[4];
	public Transform[] bezerPoints = new Transform[16];
	public bool[] isCurve = new bool[4];
	#if UNITY_EDITOR
	public Vector3[] bezerStartPos = new Vector3[4];
	public Vector3[] bezerEndPos = new Vector3[4];
	#endif

	private int bezerFineness = 20;
	private Vector2[] slope = new Vector2[4];
	private string[] lineId = new string[5];
	[SerializeField] private bool onlyEnemy;
	[SerializeField] private bool onlyBullet;
	[SerializeField] private bool onlyForward;

	private int vertexCountDiff = 0;
	private Func<Transform[], int, bool> CheckBoundary = null;
	private Action<Transform[] , int> SetupPoints = null;
	private Func<float, Vector3> ParametricTransformWithGeometryMatrix = null;
	private int numberOfPoints = 20;

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

	private Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
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
				Gizmos.color = Color.red;
				float t = 0.0f;
				for (int k = 0; k < bezerFineness; k++) {
					Vector3 tmp1 = GetPoint (bezerPoints [i * 4].position, bezerPoints [i * 4 + 1].position, bezerPoints [i * 4 + 2].position, bezerPoints [i * 4 + 3].position, t);
					Vector3 tmp2 = GetPoint (bezerPoints [i * 4].position, bezerPoints [i * 4 + 1].position, bezerPoints [i * 4 + 2].position, bezerPoints [i * 4 + 3].position, Mathf.Clamp (t + 1f/bezerFineness, 0.0f, 1.0f));
					t += 1f / bezerFineness;
					Gizmos.DrawLine (tmp1, tmp2);
				}
				bezerStartPos[i] = GetPoint (bezerPoints [i * 4].position, bezerPoints [i * 4 + 1].position, bezerPoints [i * 4 + 2].position, bezerPoints [i * 4 + 3].position, 0);
				bezerEndPos [i] = GetPoint (bezerPoints [i * 4].position, bezerPoints [i * 4 + 1].position, bezerPoints [i * 4 + 2].position, bezerPoints [i * 4 + 3].position, 1.0f);
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

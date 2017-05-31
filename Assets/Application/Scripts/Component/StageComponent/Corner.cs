using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Corner : MonoBehaviour {
	#region public_field
	[NamedArrayAttribute(new string[] { "UP", "RIGHT", "DOWN", "LEFT" })]
	public Transform[] purposeTransform = new Transform[4];
	[NamedArrayAttribute(new string[] { "START", "POS1", "POS2", "END", "START", "POS1", "POS2", "END", "START", "POS1", "POS2", "END", "START", "POS1", "POS2", "END"})]
	public Transform[] bezerPoints = new Transform[4 * 4];
	[NamedArrayAttribute(new string[] { "UP CURVE", "RIGHT CURVE", "DOWN CURVE", "LEFT CURVE" })]
	public bool[] isCurve = new bool[4];
	public float[] lineLength = new float[4];
	public float[] lengthOfBezerSection = new float[4 * (bezerFineness + 1)];
	public const int bezerFineness = 50;
	#endregion

	#region protected_field
	protected string[] lineId = new string[5];
	[SerializeField] protected bool onlyEnemy;
	[SerializeField] protected bool onlyBullet;
	[SerializeField] protected bool onlyForward;
	#endregion

	protected virtual void Awake(){
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
				CulcBezerLength (i);
				int pid = bezerPoints [i * 4 + 3].gameObject.GetInstanceID ();
				lineId [i] = (id > pid) ? id.ToString () + pid.ToString () : pid.ToString () + id.ToString ();
			} else { 
				if (purposeTransform [i] == null) {
					lineId [i] = "";
					continue;
				}
				lineLength [i] = Vector3.Distance (transform.position, purposeTransform [i].position);
				int pid = purposeTransform [i].gameObject.GetInstanceID ();
				lineId [i] = (id > pid) ? id.ToString () + pid.ToString () : pid.ToString () + id.ToString ();
			}
		}
	}

	public void CulcBezerLength(int _index){
		float length = 0.0f;
		float t = 0.0f;
		lengthOfBezerSection [0] = 0.0f;
		for (int k = 0; k < bezerFineness; k++) {
			Vector3 tmp1 = Bezer3 (bezerPoints [_index * 4].position, bezerPoints [_index * 4 + 1].position, bezerPoints [_index * 4 + 2].position, bezerPoints [_index * 4 + 3].position, t);
			Vector3 tmp2 = Bezer3 (bezerPoints [_index * 4].position, bezerPoints [_index * 4 + 1].position, bezerPoints [_index * 4 + 2].position, bezerPoints [_index * 4 + 3].position, Mathf.Clamp (t + 1f/bezerFineness, 0.0f, 1.0f));
			t += 1f / bezerFineness;
			Vector3 tmp3 = tmp2 - tmp1;
			length += tmp3.magnitude;
			lengthOfBezerSection [_index * (bezerFineness + 1) + k + 1] = length;
		}
		for (int k = 0; k < bezerFineness + 1; k++) {
			lengthOfBezerSection [_index * 51 + k + 1] /= length;
		}
		lineLength [_index] = length;
	}

	protected Vector3 Bezer3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		var oneMinusT = 1f - t;
		return oneMinusT * oneMinusT * oneMinusT * p0 +
			3f * oneMinusT * oneMinusT * t * p1 +
			3f * oneMinusT * t * t * p2 +
			t * t * t * p3;
	}

	// corner tag : RightCorner, LeftCorner, PassCorner, CurveCorner
	// TODO : より良いコードで実装し直し(Vector3の参照渡しがなぜできない？)
	public virtual Vector3 ChangePurposeStraight(MoveMode _moveMode ,int _moveDesMode, ref MoveDir _moveDir, ref string _lineId, ref float _lineLength){
		_moveDir = GetNextMoveDir (_moveDir, _moveDesMode, _moveMode);
		_lineId = lineId [(int)_moveDir];
		_lineLength = lineLength [(int)_moveDir];
		return purposeTransform [(int)_moveDir].position;
	}

	public virtual Transform[] ChangePurposeCurve(MoveMode _moveMode, int _moveDesMode, ref MoveDir _moveDir, ref string _lineId, ref float _lineLength, ref float[] _lengthOfBezerSection) {
		_moveDir = GetNextMoveDir (_moveDir, _moveDesMode, _moveMode);
		_lineId = lineId [(int)_moveDir];
		_lineLength = lineLength [(int)_moveDir];
		Transform[] points = new Transform[4];
		Array.Copy (bezerPoints, (int)_moveDir * 4, points, 0, 4);
		Array.Copy (lengthOfBezerSection, (int)_moveDir * 51, _lengthOfBezerSection, 0, 51);
		return points;
	}

	protected MoveDir GetNextMoveDir(MoveDir _moveDir, int _moveDesMode, MoveMode _moveMode){
		if (onlyForward || _moveMode == MoveMode.IGNORE) {
			if (_moveDesMode == 1) {
				return MoveDir.UP;
			} else {
				return MoveDir.DOWN;
			}
		} else if ((onlyEnemy && _moveDir == MoveDir.DOWN) || (onlyBullet && _moveDir == MoveDir.UP)) {
			if (transform.tag == "RightCorner") {
				return MoveDir.RIGHT;
			} else {
				return MoveDir.LEFT;
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

		if ((_moveDir == MoveDir.RIGHT || _moveDir == MoveDir.LEFT) && (tag != "PassCorner" && tag != "Warp")) {
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
		} else if (transform.tag == "PassCorner" || transform.tag == "Warp") {
			if (transform.tag == "Warp")
				Debug.Log (_moveDir);
			bool isChangeDir = false;
			for (int i = 0; i < 4; i++) {
				isChangeDir = (isCurve [i] && isCurve [(i + 1) % 4]) || (purposeTransform [i] != null && purposeTransform [(i + 1) % 4] != null);
				if (isChangeDir) {
					break;
				}
			}
			if (isChangeDir) {
				Debug.Log ("CHANGE");
				int nextMoveDir = ((int)_moveDir + 2) % 4;
				for (int i = 1; i < 4; i++) {
					nextMoveDir = (nextMoveDir + i) % 4;
					if (purposeTransform [nextMoveDir] != null) {
						return (MoveDir)nextMoveDir;
					}
				}
			}

			return _moveDir;
		}

		Debug.Log ("Error Case Corner (GetNextMoveDir)");
		Debug.Log (tag);
		Debug.Log (_moveDir);
		Debug.Log (_moveDesMode);
		return MoveDir.UP;
	}

	public bool CheckCurve(MoveDir _moveDir, int _moveDesMode, MoveMode _moveMode){
		return isCurve [(int)GetNextMoveDir (_moveDir, _moveDesMode, _moveMode)];
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
					Gizmos.color = Color.blue;
					Gizmos.DrawLine (transform.position, purposeTransform [i].transform.position);
					continue;
				}
				isConnected = purposeCorner.purposeTransform [(i + 2) % 4] == gameObject.transform;
				//isConnected = !purposeCorner.isCurve [(i + 2) % 4];
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

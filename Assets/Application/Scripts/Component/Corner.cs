using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner : MonoBehaviour {
	public enum Direction : int {
		UP = 0,
		RIGHT = 1,
		DOWN = 2,
		LEFT = 3,
	}
	[NamedArrayAttribute(new string[] { "UP", "RIGHT", "DOWN", "LEFT" })]
	public Transform[] purposeTransform = new Transform[4];
	private Vector2[] slope = new Vector2[4];
	private int[] lineId = new int[5];
	[SerializeField] private bool onlyEnemy;
	[SerializeField] private bool onlyBullet;

	private void Awake(){
		int id = gameObject.GetInstanceID ();
		for (int i = 0; i < 4; i++) {
			if (purposeTransform [i] == null) {
				slope [i] = Vector2.zero;
				lineId [i] = 0;
				continue;
			}
			slope [i] = (purposeTransform [i].position - transform.position).normalized;
			lineId [i] = id + purposeTransform [i].GetInstanceID ();
		}
	}

	// corner tag : RightCorner, LeftCorner, PassCorner, CurveCorner
	// TODO : より良いコードで実装し直し(Vector2の参照渡しがなぜできない？)
	public Vector2 ChangePurpose(ref MoveObjectBase.MoveDir _moveDir, int _moveDesMode, ref int _lineId){
		if ((_moveDir == MoveObjectBase.MoveDir.RIGHT || _moveDir == MoveObjectBase.MoveDir.LEFT) && tag != "PassCorner") {
			if (_moveDesMode == 1) {
				_moveDir = MoveObjectBase.MoveDir.UP;
				_lineId = lineId [0];
				return slope [0];
			} else {
				_moveDir = MoveObjectBase.MoveDir.DOWN;
				_lineId = lineId [2];
				return slope [2];
			}
		} 

		if ((onlyEnemy && _moveDir == MoveObjectBase.MoveDir.DOWN) || (onlyBullet && _moveDir == MoveObjectBase.MoveDir.UP)) {
			if (transform.tag == "RightCorner") {
				_moveDir = MoveObjectBase.MoveDir.RIGHT;
				_lineId = lineId [2];
				return slope [2];
			} else {
				_moveDir = MoveObjectBase.MoveDir.LEFT;
				_lineId = lineId [1];
				return slope [1];
			}
		} else if(onlyEnemy || onlyBullet) {
			return slope [(int)_moveDir];
		}

		if (transform.tag == "RightCorner") {
			if (_moveDir == MoveObjectBase.MoveDir.RIGHT) {
			}
			_moveDir = MoveObjectBase.MoveDir.RIGHT;
			_lineId = lineId [1];
			return slope [1];
		} else if (transform.tag == "LeftCorner") {
			_moveDir = MoveObjectBase.MoveDir.LEFT;
			_lineId = lineId [3];
			return slope [3];
		} else if (transform.tag == "PassCorner") {
			_lineId = lineId [(int)_moveDir];
			return slope [(int)_moveDir];
		}

		Debug.Log ("Error Case Corner " + transform.name);
		return Vector2.zero;
	}


	public int[] GetLineId(){
		return (int[])lineId.Clone ();
	}

	#if UNITY_EDITOR
	public void SetCorner(){
		Debug.Log ("set");
		for (int i = 0; i < 4; i++) {
			var corner = purposeTransform [i].GetComponent<Corner> ();
			if (corner == null) {
				continue;
			}
			switch (i) {
			case (int)Direction.UP:
				corner.purposeTransform [(int)Direction.DOWN] = gameObject.transform;
				break;
			case (int)Direction.RIGHT:
				corner.purposeTransform [(int)Direction.LEFT] = gameObject.transform;
				break;
			case (int)Direction.DOWN:
				corner.purposeTransform [(int)Direction.UP] = gameObject.transform;
				break;
			case (int)Direction.LEFT:
				corner.purposeTransform [(int)Direction.RIGHT] = gameObject.transform;
				break;
			}
		}
	}

	private void OnDrawGizmos(){
		Gizmos.color = Color.red;
		bool isConnected = false;
		for (int i = 0; i < 4; i++) {
			if (purposeTransform [i] == null) {
				continue;
			}
			var corner = purposeTransform [i].GetComponent<Corner> ();
			if (corner == null) {
				Debug.Log ("Corner is not exist");
				continue;
			}
			switch (i) {
			case (int)Direction.UP:
				isConnected = corner.purposeTransform [(int)Direction.DOWN] == gameObject.transform;
				break;
			case (int)Direction.RIGHT:
				isConnected = corner.purposeTransform [(int)Direction.LEFT] == gameObject.transform;
				break;
			case (int)Direction.DOWN:
				isConnected = corner.purposeTransform [(int)Direction.UP] == gameObject.transform;
				break;
			case (int)Direction.LEFT:
				isConnected = corner.purposeTransform [(int)Direction.RIGHT] == gameObject.transform;
				break;
			}
			if (isConnected) {
				Gizmos.DrawLine (transform.position, purposeTransform [i].transform.position);
			}
		}
	}
	#endif
}

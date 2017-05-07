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
	public Transform[] purposeObject = new Transform[4];
	private Vector2[] slope = new Vector2[4];
	private int[] lineLayer = new int[5];
	[SerializeField] private bool onlyEnemy;
	[SerializeField] private bool onlyBullet;

	private void Awake(){
		int id = gameObject.GetInstanceID ();
		for (int i = 0; i < 4; i++) {
			if (purposeObject [i] == null) {
				slope [i] = Vector2.zero;
				lineLayer [i] = 0;
				continue;
			}
			slope [i] = (purposeObject [i].position - transform.position).normalized;
			lineLayer [i] = id + purposeObject [i].GetInstanceID ();
		}
	}

	// corner tag : RightCorner, LeftCorner, PassCorner, CurveCorner
	// TODO : より良いコードで実装し直し(Vector2の参照渡しがなぜできない？)
	public Vector2 ChangePurpose(ref MoveObjectBase.MoveDir _moveDir, int _moveDesMode, ref int _lineLayer){
		if ((_moveDir == MoveObjectBase.MoveDir.RIGHT || _moveDir == MoveObjectBase.MoveDir.LEFT) && tag != "PassCorner") {
			if (_moveDesMode == 1) {
				_moveDir = MoveObjectBase.MoveDir.UP;
				_lineLayer = lineLayer [0];
				return slope [0];
			} else {
				_moveDir = MoveObjectBase.MoveDir.DOWN;
				_lineLayer = lineLayer [2];
				return slope [2];
			}
		} 

		if ((onlyEnemy && _moveDir == MoveObjectBase.MoveDir.DOWN) || (onlyBullet && _moveDir == MoveObjectBase.MoveDir.UP)) {
			if (transform.tag == "RightCorner") {
				_moveDir = MoveObjectBase.MoveDir.RIGHT;
				_lineLayer = lineLayer [2];
				return slope [2];
			} else {
				_moveDir = MoveObjectBase.MoveDir.LEFT;
				_lineLayer = lineLayer [1];
				return slope [1];
			}
		} else if(onlyEnemy || onlyBullet) {
			return slope [(int)_moveDir];
		}

		if (transform.tag == "RightCorner") {
			if (_moveDir == MoveObjectBase.MoveDir.RIGHT) {
			}
			_moveDir = MoveObjectBase.MoveDir.RIGHT;
			_lineLayer = lineLayer [1];
			return slope [1];
		} else if (transform.tag == "LeftCorner") {
			_moveDir = MoveObjectBase.MoveDir.LEFT;
			_lineLayer = lineLayer [3];
			return slope [3];
		} else if (transform.tag == "PassCorner") {
			_lineLayer = lineLayer [(int)_moveDir];
			return slope [(int)_moveDir];
		}

		Debug.Log ("Error Case Corner " + transform.name);
		return Vector2.zero;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner : MonoBehaviour {
	public Transform[] purposeObject = new Transform[4];
	public Vector2[] slope = new Vector2[4];
	[SerializeField] private bool onlyEnemy;
	[SerializeField] private bool onlyBullet;

	private void Awake(){
		for (int i = 0; i < 4; i++) {
			if (purposeObject [i] == null) {
				slope [i] = Vector2.zero;
				continue;
			}
			slope [i] = (purposeObject [i].position - transform.position).normalized;
		}
	}

	// corner tag : RightCorner, LeftCorner, PassCorner, CurveCorner
	// TODO : より良いコードで実装し直し(Vector2の参照渡しがなぜできない？)
	public Vector2 ChangePurpose(ref MoveObjectBase.MoveDir moveDir){
		if ((onlyEnemy && moveDir == MoveObjectBase.MoveDir.DOWN) || (onlyBullet && moveDir == MoveObjectBase.MoveDir.UP)) {
			if (transform.tag == "RightCorner") {
				moveDir = MoveObjectBase.MoveDir.RIGHT;
				return slope [2];
			} else {
				moveDir = MoveObjectBase.MoveDir.LEFT;
				return slope [1];
			}
		} else if(onlyEnemy || onlyBullet) {
			return slope [(int)moveDir];
		}

		if (transform.tag == "RightCorner") {
			moveDir = MoveObjectBase.MoveDir.RIGHT;
			return slope [1];
		} else if (transform.tag == "LeftCorner") {
			moveDir = MoveObjectBase.MoveDir.LEFT;
			return slope [3];
		} else if (transform.tag == "PassCorner") {
			return slope [(int)moveDir];
		}

		Debug.Log ("Error Case Corner " + transform.name);
		return Vector2.zero;
	}

}

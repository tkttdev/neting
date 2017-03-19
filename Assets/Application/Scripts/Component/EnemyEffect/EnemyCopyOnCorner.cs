using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCopyOnCorner : EnemyEffectBase {
	[HideInInspector] public bool isFirstCopy = true;
	[SerializeField] private bool isOnlyOnceCopy = true;

	public override void Initialize (){
		isFirstCopy = true;
	}

	public override void OnTrriger2DEffect (Collider2D _other, int _enemyId){
		bool isCorner = (_other.tag == "LeftCorner" || _other.tag == "RightCorner" || _other.tag == "LeftEnemyTunnel" || _other.tag == "RightEnemyTunnel");
		if (isFirstCopy && isCorner) {
			gameObject.GetComponent<Enemy> ().isInCorner = true;
			MoveObjectBase.MoveDir originalMoveDir = gameObject.GetComponent<Enemy>().moveDir;
			MoveObjectBase.MoveDir copyMoveDir;

			if ((_other.tag == "LeftCorner" || _other.tag == "LeftEnemyTunnel") && originalMoveDir == MoveObjectBase.MoveDir.FORWARD) {
				copyMoveDir = MoveObjectBase.MoveDir.LEFT;
			} else if ((_other.tag == "RightCorner" || _other.tag == "RightEnemyTunnel") && originalMoveDir == MoveObjectBase.MoveDir.FORWARD) {
				copyMoveDir = MoveObjectBase.MoveDir.RIGHT;
			} else {
				copyMoveDir = MoveObjectBase.MoveDir.FORWARD;
			}

			isFirstCopy = false;
			GameObject copy = Instantiate (gameObject, gameObject.transform.position, Quaternion.identity) as GameObject;
			StageManager.I.CopyEnemy();
			copy.GetComponent<Enemy> ().moveDir = copyMoveDir; 
			isFirstCopy = !isOnlyOnceCopy;
			gameObject.GetComponent<Enemy> ().isInCorner = false;
		}
	}
}

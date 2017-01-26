using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCopyOnCorner : EnemyEffectBase {
	public GameObject spawnCorner;
	public bool isFirstCopy = true;

	public override void OnTrriger2DEffect (Collider2D _other, int _enemyId){
		bool isCorner = (_other.tag == "LeftCorner" || _other.tag == "RightCorner" || _other.tag == "Warp");
		if (isFirstCopy && isCorner) {
			gameObject.GetComponent<Enemy> ().isCopy = true;
			GameObject copy = Instantiate (gameObject, gameObject.transform.position, Quaternion.identity) as GameObject;
			copy.GetComponent<EnemyCopyOnCorner> ().isFirstCopy = false;
			copy.GetComponent<Enemy> ().moveDir = MoveObjectBase.MoveDir.FORWARD;
			isFirstCopy = false;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCopyOnCorner : EnemyEffectBase {
	public GameObject spawnCorner;
	public bool isFirstCopy = true;

	public override void OnTrriger2DEffect (Collider2D _other, int _enemyId){
		base.OnTrriger2DEffect (_other, _enemyId);
		if (isFirstCopy) {
			GameObject copy = Instantiate (gameObject, gameObject.transform.position, Quaternion.identity) as GameObject;
			copy.GetComponent<EnemyCopyOnCorner> ().isFirstCopy = false;
			copy.GetComponent<Enemy> ().moveDir = MoveObjectBase.MoveDir.FORWARD;
			isFirstCopy = false;
		}
	}
}

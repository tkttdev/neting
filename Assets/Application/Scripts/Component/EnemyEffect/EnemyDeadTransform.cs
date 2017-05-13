using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadTransform : EnemyEffectBase {
	[SerializeField] private int transformId = 0;

	private MoveDir moveDir;

	public override void DeadEffect (){
		moveDir = gameObject.GetComponent<Enemy> ().moveDir;
		TransformEnemy ();
	}

	private void TransformEnemy(){
		StageManager.I.CopyEnemy ();
		GameObject enemyPrefab = Resources.Load (ENEMY_DEFINE.PATH [transformId]) as GameObject;
		GameObject transformEnemy = Instantiate (enemyPrefab, gameObject.transform.position, Quaternion.identity);
		transformEnemy.GetComponent<Enemy> ().moveDir = moveDir;
	}
}

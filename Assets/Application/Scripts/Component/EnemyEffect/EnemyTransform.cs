using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTransform : EnemyEffectBase {
	private enum Timing : int {
		DAMAGE = 0,
		DEAD = 1,
	}

	[SerializeField] private Timing timing = Timing.DAMAGE;
	[SerializeField] private int transformId = 0;

	private MoveObjectBase.MoveDir moveDir;

	public override void DamageEffect (int _damage){
		if (timing == Timing.DAMAGE) {
			moveDir = gameObject.GetComponent<Enemy> ().moveDir;
			TransformEnemy ();
		}
	}

	public override void DeadEffect (){
		if (timing == Timing.DEAD) {
			moveDir = gameObject.GetComponent<Enemy> ().moveDir;
			TransformEnemy ();
		}
	}

	private void TransformEnemy(){
		GameObject enemyPrefab = Resources.Load (ENEMY_DEFINE.PATH [transformId]) as GameObject;
		GameObject transformEnemy = Instantiate (enemyPrefab, gameObject.transform.position, Quaternion.identity);
		transformEnemy.GetComponent<Enemy> ().moveDir = moveDir;
		if (timing == Timing.DAMAGE) {
			gameObject.GetComponent<Enemy> ().DestroyOwn ();
		}
	}
}

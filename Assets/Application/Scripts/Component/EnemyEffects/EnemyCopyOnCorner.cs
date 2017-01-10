using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCopyOnCorner : EnemyEffectBase {

	/*
	private bool isCopy = false;
	public GameObject spawnCorner;
	public bool isOriginal = true;

	public override void OnTrriger2DEffect (Collider2D _other, int _enemyId){
		base.OnTrriger2DEffect (_other, _enemyId);
		if (isOriginal) {
			GameObject copyPrefab = Resources.Load (ENEMY_DEFINE.PATH [_enemyId]) as GameObject;
			DestroyImmediate (copyPrefab.GetComponent<EnemyCopyOnCorner> (), true);
			Debug.Log (copyPrefab.GetComponent<EnemyCopyOnCorner> () == null);
			//Instantiate (copyPrefab, gameObject.transform.position, Quaternion.identity);
		}
	}
	*/
}

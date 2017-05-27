﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ignoreの敵とコピーの敵は候補に入れないでください
/// </summary>
public class EnemyRandomSpawnOnCorner : EnemyEffectBase {
	[HideInInspector] public bool isFirstCopy = true;
	[SerializeField] private bool isOnlyOnceCopy = true;
	[SerializeField] private int[] candidateId;

	void OnDisable(){
		isFirstCopy = true;
	}

	public override void OnTrriger2DEffect (Collider2D _other, int _enemyId){
		bool isCorner = (_other.tag == "LeftCorner" || _other.tag == "RightCorner" || _other.tag == "LeftEnemyTunnel" || _other.tag == "RightEnemyTunnel");
		if (isFirstCopy && isCorner) {
			MoveDir originalMoveDir = gameObject.GetComponent<Enemy>().moveDir;
			MoveDir copyMoveDir;

			if (_other.tag == "LeftCorner" && originalMoveDir == MoveDir.DOWN) {
				copyMoveDir = MoveDir.LEFT;
			} else if (_other.tag == "RightCorner" && originalMoveDir == MoveDir.DOWN) {
				copyMoveDir = MoveDir.RIGHT;
			} else {
				copyMoveDir = MoveDir.DOWN;
			}

			int selectId = candidateId [Random.Range (0, candidateId.Length)];
			GameObject spawnEnemyPrefab = Resources.Load (ENEMY_DEFINE.PATH [selectId]) as GameObject;

			isFirstCopy = false;
			GameObject copy = Instantiate (spawnEnemyPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
			StageManager.I.AddAllEnemyNum(1);
			//copy.GetComponent<Enemy> ().moveDir = copyMoveDir; 
			isFirstCopy = !isOnlyOnceCopy;
		}
	}
}

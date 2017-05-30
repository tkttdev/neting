using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCopyOnCorner : EnemyEffectBase {
	[HideInInspector] public bool afterCopy = false;
	private Enemy original;

	private void Awake(){
		original = GetComponent<Enemy> ();
	}

	public override void OnTrriger2DEffect (Collider2D _other, int _enemyId){
		bool isCorner = (_other.tag == "LeftCorner" || _other.tag == "RightCorner");
		if (!afterCopy) {
			afterCopy = true;
			Corner corner = _other.gameObject.GetComponent<Corner> ();
			GameObject copyObj = Instantiate (gameObject, gameObject.transform.position, Quaternion.identity) as GameObject;
			Enemy copy = copyObj.GetComponent<Enemy> ();

			if (corner.CheckCurve (original.moveDir, -1, original.moveMode)) {
				copy.isCurve = true;
				copy.bezerPoints = corner.ChangePurposeCurve (ref copy.moveDir, -1, ref copy.lineId, ref copy.onLineLength, ref copy.lengthOfBezerSection, copy.moveMode);
			} else {
				copy.endPos = corner.ChangePurposeStraight (ref copy.moveDir, -1, ref copy.lineId,ref copy.onLineLength, copy.moveMode);
			}

			StageManager.I.AddAllEnemyNum(1);
		}
	}

	void OnDisable (){
		afterCopy = true;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCopyOnCorner : EnemyEffectBase {
	[HideInInspector] public bool isFirstCopy = true;

	[SerializeField] private bool isOnlyOnceCopy = true;
	private Enemy original;

	private void Awake(){
		original = GetComponent<Enemy> ();
	}

	public override void OnTrriger2DEffect (Collider2D _other, int _enemyId){
		bool isCorner = (_other.tag == "LeftCorner" || _other.tag == "RightCorner");
		if (isFirstCopy && isCorner) {
			original.isInCorner = true;
			Corner corner = _other.gameObject.GetComponent<Corner> ();
			GameObject copyObj = Instantiate (gameObject, gameObject.transform.position, Quaternion.identity) as GameObject;
			Enemy copy = copyObj.GetComponent<Enemy> ();

			if (corner.CheckCurve (original.moveDir, -1, original.moveMode)) {
				copy.isCurve = true;
				copy.bezerPoints = corner.ChangePurposeCurve (ref copy.moveDir, -1, ref copy.lineId, ref copy.onCurveLength, ref copy.lengthOfBezerSection, copy.moveMode);
			} else {
				copy.slope = corner.ChangePurposeStraight (ref copy.moveDir, -1, ref copy.lineId, copy.moveMode);
			}

			isFirstCopy = false;
			StageManager.I.AddAllEnemyNum(1);
			isFirstCopy = !isOnlyOnceCopy;
			gameObject.GetComponent<Enemy> ().isInCorner = false;
		}
	}

	void OnDisable (){
		isFirstCopy = true;
	}
}

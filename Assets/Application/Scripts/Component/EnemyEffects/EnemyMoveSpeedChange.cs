using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveSpeedChange : EnemyEffectBase {

	private enum ChangeMode : int {
		HIGH_TO_LOW = -1,
		LOW_TO_HIGH = 1,
	}

	[SerializeField] private ChangeMode changeMode = ChangeMode.LOW_TO_HIGH;
	[SerializeField] private float minMoveSpeed;
	[SerializeField] private float maxMoveSpeed;
	[SerializeField] private float changeRateParsec;

	private float moveSpeed;
	private bool isChange = true;

	public override void MoveEffect (){
		base.MoveEffect ();

		if (isChange) {
			moveSpeed = targetEnemy.GetMoveSpeed ();
			moveSpeed += (int)changeMode * changeRateParsec * Time.deltaTime;

			switch (changeMode) {
			case ChangeMode.HIGH_TO_LOW:
				if (moveSpeed <= minMoveSpeed) {
					moveSpeed = minMoveSpeed;
					isChange = false;
				}
				break;
			case ChangeMode.LOW_TO_HIGH:
				if (moveSpeed >= maxMoveSpeed) {
					moveSpeed = maxMoveSpeed;
					isChange = false;
				}
				break;
			}

			targetEnemy.SetMoveSpeed (moveSpeed);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGraduallyRecovery : EnemyEffectBase {

	[Range(0.1f,2.0f)]
	[SerializeField] private float recoveryRateParsec = 0.5f;

	public override void MoveEffect (){
		gameObject.GetComponent<Enemy> ().RecoveryHP (recoveryRateParsec * Time.deltaTime);
	}
}

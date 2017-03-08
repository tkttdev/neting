using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRecoveryDamage : EnemyEffectBase {
	[SerializeField] private float rateOfDamage = 0.5f;

	public override void DamageEffect (float _damage){
		gameObject.GetComponent<Enemy> ().RecoveryHP (rateOfDamage * _damage);
	}
}

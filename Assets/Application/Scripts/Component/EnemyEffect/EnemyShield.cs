using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : EnemyEffectBase {

	[SerializeField] private int ProtectTimes = 1;
	[SerializeField] private SpriteRenderer shieldSprite;

	public override void DamageEffect (float _damage){
		if (ProtectTimes > 0) {
			ProtectTimes--;
			gameObject.GetComponent<Enemy> ().RecoveryHP (_damage);
		} else {
			if (shieldSprite != null) {
				shieldSprite.enabled = false;
			}
		}
	}
}

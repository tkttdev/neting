using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : EnemyEffectBase {

	[SerializeField] private int protectTimes = 1;
	[SerializeField] private SpriteRenderer shieldSprite;

	public override void DamageEffect (float _damage){
		if (protectTimes > 0) {
			protectTimes--;
			gameObject.GetComponent<Enemy> ().RecoveryHP (_damage);
			if(protectTimes <= 0) {
				shieldSprite.enabled = false;
			}
		} 
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : EnemyEffectBase {

	private int protectTimes = 1;
	[SerializeField] private int initProtectTimes = 1; 
	[SerializeField] private SpriteRenderer shieldSprite;

	void Start(){
		protectTimes = initProtectTimes;
	}

	void Disabled(){
		protectTimes = initProtectTimes;
		shieldSprite.enabled = true;
	}

	public override void DamageEffect (float _damage){
		if (protectTimes > 0) {
			protectTimes--;
			gameObject.GetComponent<Enemy> ().RecoveryHP (_damage);
			if (protectTimes <= 0) {
				shieldSprite.enabled = false;
			}
		} 
	}
}

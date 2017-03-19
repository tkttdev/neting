using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : EnemyEffectBase {

	[SerializeField] private int protectTimes = 1;
<<<<<<< HEAD
=======
	private int maxProtectTimes = 1; 
>>>>>>> origin/develop
	[SerializeField] private SpriteRenderer shieldSprite;

	void Start(){
		maxProtectTimes = protectTimes;
	}

	public override void Initialize (){
		protectTimes = maxProtectTimes;
	}

	public override void DamageEffect (float _damage){
		if (protectTimes > 0) {
			protectTimes--;
			gameObject.GetComponent<Enemy> ().RecoveryHP (_damage);
<<<<<<< HEAD
			if(protectTimes <= 0) {
=======
			if (protectTimes <= 0) {
>>>>>>> origin/develop
				shieldSprite.enabled = false;
			}
		} 
	}
}

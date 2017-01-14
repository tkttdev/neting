using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBullet : MoveObjectBase {

    private int damage;

    protected override void Initialize() {
		base.Initialize ();
		damage = CHARACTER_DEFINE.BULLET_DAMAGE [UserDataManager.I.GetUseCharaIndex ()];
        SetMoveToEnemy();
    }

    protected override void Update() {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D _other) {
        base.OnTriggerEnter2D(_other);
		if (_other.tag == "Enemy") {
			_other.gameObject.GetComponent<Enemy> ().TakeDamage (damage);
			Destroy (gameObject);
		} else if (_other.tag == "DestroyZone") {
			Destroy (gameObject);
		}
    }
}

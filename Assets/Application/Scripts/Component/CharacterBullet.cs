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

    protected override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
        if (other.tag == "Enemy") {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBullet : MoveObjectBase {

	private int damage = 1;

	protected override void Initialize() {
		base.Initialize ();
		damage = CHARACTER_DEFINE.BULLET_DAMAGE [UserDataManager.I.GetUseCharacterIndex ()];
        SetMoveToEnemy();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
		float scale = Mathf.PingPong (Time.time*2.0f, 1.0f) + 3.0f;
		gameObject.transform.localScale = new Vector3 (scale, scale, scale);
    }

    protected override void OnTriggerEnter2D(Collider2D _other) {
        base.OnTriggerEnter2D(_other);
		if (_other.tag == "Enemy") {
			if (_other.GetComponent<MoveObjectBase> ().lineId.Equals(lineId)) {
				_other.gameObject.GetComponent<Enemy> ().TakeDamage (damage);
				DestroyOwn ();
			}
		} else if (_other.tag == "DestroyZone") {
			DestroyOwn ();
		}
    }

	private void DestroyOwn(){
		base.Initialize ();
		ObjectPool.I.Release (gameObject);
	}
}

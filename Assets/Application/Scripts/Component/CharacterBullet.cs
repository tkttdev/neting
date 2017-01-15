using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBullet : MoveObjectBase {

    private int damage;

    protected override void Initialize() {
		base.Initialize ();
		damage = CHARACTER_DEFINE.BULLET_DAMAGE [UserDataManager.I.GetUseCharacterIndex ()];
        SetMoveToEnemy();
    }

    protected override void Update() {
        base.Update();
		float scale = Mathf.PingPong (Time.time*2.0f, 1.0f) + 3.0f;
		gameObject.transform.localScale = new Vector3 (scale, scale, scale);
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

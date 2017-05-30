using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillMode : int {
	NORMAL = 0,
	MOVE = 1,
	NOTMOVE = 2,
}

public class Bullet : MoveObjectBase {

	private int damage = 1;
	[SerializeField] private bool activeSkill;
	[SerializeField] private SkillMode skillMode;

	protected override void Initialize() {
		base.Initialize ();
		damage = CHARACTER_DEFINE.BULLET_DAMAGE [UserDataManager.I.GetUseCharacterIndex ()];
        SetMoveToEnemy();
    }

	private void OnEnable(){
		base.Initialize ();
	}

    protected override void FixedUpdate() {
        base.FixedUpdate();
		if (skillMode == SkillMode.NORMAL) {
			float scale = Mathf.PingPong(Time.time * 2.0f, 1.0f) + 3.0f;
			gameObject.transform.localScale = new Vector3(scale, scale, scale);
		}
    }

	protected override void OnTriggerEnter2D(Collider2D _other) {
		if (skillMode != SkillMode.NOTMOVE) {
			base.OnTriggerEnter2D(_other);
		}

		if (skillMode == SkillMode.NORMAL) {
			base.OnTriggerEnter2D(_other);
			if (_other.tag == "Enemy") {
				if (_other.GetComponent<MoveObjectBase>().lineId.Equals(lineId)) {
					_other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
					DestroyOwn();
				}
			} else if (_other.tag == "Boss") {
				_other.gameObject.GetComponent<BossBase>().TakeDamage(damage);
				DestroyOwn();
			} else if (_other.tag == "DestroyZone") {
				DestroyOwn();
			}
		}
    }

	private void DestroyOwn(){
		ObjectPool.I.Release (gameObject);
	}
}

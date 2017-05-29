using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillThunder : MoveObjectBase {

	[SerializeField] private GameObject locus;

	private int damage = 10;

	protected override void Initialize() {
		base.Initialize();
		SetMoveToEnemy();
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();

		ObjectPool.I.Instantiate(locus, gameObject.transform.localPosition);
	}

	protected override void OnTriggerEnter2D(Collider2D _other) {
		base.OnTriggerEnter2D(_other);
		if (_other.tag == "Enemy") {
			if (_other.GetComponent<MoveObjectBase>().lineId.Equals(lineId)) {
				_other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
				DestroyAll();
			}
		} else if (_other.tag == "Boss") {
			_other.gameObject.GetComponent<BossBase>().TakeDamage(damage);
			DestroyAll();
		} else if (_other.tag == "DestroyZone") {
			DestroyAll();
		}
	}

	private void DestroyAll() {
		base.Initialize();

		GameObject[] trush = GameObject.FindGameObjectsWithTag("Lucus");
		for(int i = 0; i < trush.Length; i++) {
			ObjectPool.I.Release(trush[i]);
		}

		ObjectPool.I.Release(gameObject);
	}
}

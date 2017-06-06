using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	private int damage = 10;

	// Use this for initialization
	void Start () {
		SkillEffect();
    }

	void OnEnable() {
		Start();
	}

	public void SkillEffect() {
		iTween.ScaleTo(gameObject, iTween.Hash("y", 7, "time", 0.5f));

		StartCoroutine("DestroyLaser");
	}

	public IEnumerator DestroyLaser() {
		yield return new WaitForSeconds(0.5f);

		iTween.ScaleTo(gameObject, iTween.Hash("x", 0, "time", 0.5f));

		yield return new WaitForSeconds(0.5f);

		gameObject.transform.localScale = new Vector3(0.3f, 0, 0);
		ObjectPool.I.Release(gameObject);
	}

	void OnTriggerEnter2D(Collider2D _other) {
		if (_other.tag == "Enemy") {
			_other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
		}
	}
}

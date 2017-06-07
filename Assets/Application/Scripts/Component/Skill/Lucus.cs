using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucus : MonoBehaviour {

	private int damage = 10;

	void OnTriggerEnter2D(Collider2D _other) {
		if (_other.tag == "Enemy") {
			_other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
		}
	}
}

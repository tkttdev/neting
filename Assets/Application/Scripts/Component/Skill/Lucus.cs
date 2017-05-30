using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucus : MonoBehaviour {

	[Range(1.0f, 10.0f)]
	public int damage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter2D(Collider2D _other) {
		if (_other.tag == "Enemy") {
			_other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBomb : MonoBehaviour {

	private int damage = 10;

	// Use this for initialization
	void Start () {
		SkillEffect();
    }
	
	// Update is called once per frame
	void Update () {
		
    }

	public void SkillEffect() {
		iTween.ScaleTo(gameObject, iTween.Hash("x", 8, "y", 8, "time", 5.0f));

		StartCoroutine("DestroyBomb");
	}

	public IEnumerator DestroyBomb() {
		yield return new WaitForSeconds(2.0f);

		Destroy(gameObject);
    }

	public void OnTriggerEnter2D(Collider2D _other) {
		if (_other.tag == "Enemy") {
			_other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
		}
	}
}

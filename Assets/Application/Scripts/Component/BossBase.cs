using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour {

	private float hp = 2.0f;

	private void Awake(){
		Initialize ();
	}

	protected virtual void Initialize(){
	}

	public void TakeDamage(float _damage){
		hp -= _damage;
		CheckHP ();
		MoveCore ();
	}

	private void CheckHP(){
		if (hp <= 0) {
			GameManager.I.SetStatuEnd ();
		}
	}

	private void MoveCore() {
		float afterMoveX = Random.Range (-1, 2);
		while (Mathf.Abs(afterMoveX - transform.position.x) < Mathf.Epsilon) {
			afterMoveX = Random.Range (-1, 2);
		}
		transform.position = new Vector3 (afterMoveX, transform.position.y, 0);
	}

	private void OnTriggerEnter2D(Collider2D _other){
	}
}

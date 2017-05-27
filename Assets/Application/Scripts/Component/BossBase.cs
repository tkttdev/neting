using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour {

	[SerializeField] private float hp = 5.0f;
	[SerializeField] private bool[] isExistLine = new bool[5];

	private void Awake(){
		Initialize ();
	}

	protected virtual void Initialize(){
		MoveCore ();
	}

	public void TakeDamage(float _damage){
		hp -= _damage;
		Debug.Log (hp);
		CheckHP ();
		MoveCore ();
	}

	private void CheckHP(){
		if (hp <= 0) {
			GameManager.I.SetStatuEnd ();
		}
	}

	private void MoveCore() {
		float afterMoveX = Random.Range (-2, 3);
		while (Mathf.Abs(afterMoveX - transform.position.x) < Mathf.Epsilon || !isExistLine[(int)afterMoveX + 2]) {
			afterMoveX = Random.Range (-2, 3);
		}
		transform.position = new Vector3 (afterMoveX, transform.position.y, 0);
	}

	private void OnTriggerEnter2D(Collider2D _other){
	}
}

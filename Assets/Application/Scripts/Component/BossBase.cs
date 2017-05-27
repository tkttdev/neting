using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour {

	private float hp = 100.0f;
	//[SerializeField]private GameObject core;
	//private Collider2D collider;

	private void Awake(){
		Initialize ();
	}

	protected virtual void Initialize(){
	}

	public void TakeDamage(float _damage){
		hp -= _damage;
		MoveCore ();
	}

	private void CheckHP(){
		
	}

	private void MoveCore() {
		float afterMoveX = Random.Range (-2, 3);
		while (Mathf.Abs(afterMoveX - transform.position.x) < Mathf.Epsilon) {
			afterMoveX = Random.Range (-2, 3);
		}
		transform.position = new Vector3 (Random.Range (-2, 3), transform.position.y, 0);
	}

	private void OnTriggerEnter2D(Collider2D _other){
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour {

	private float hp = 5.0f;
	[SerializeField]private GameObject core;
	private Collider2D collider;

	private void Awake(){
		Initialize ();
	}

	protected virtual void Initialize(){
		collider = GetComponent<Collider2D> ();
	}

	public void TakeDamage(float _damage){
		hp -= _damage;
	}

	private void CheckHP(){
		
	}

	private void MoveCore() {
		// core移動処理
	}

	private void OnTriggerEnter2D(Collider2D _other){
	}
}

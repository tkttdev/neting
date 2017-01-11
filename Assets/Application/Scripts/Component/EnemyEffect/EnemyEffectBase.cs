using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectBase : MonoBehaviour {

	protected Enemy targetEnemy;

	protected virtual void Start(){
		targetEnemy = gameObject.GetComponent<Enemy> ();
	}

	public virtual void MoveEffect(){}
	public virtual void DeadEffect(){}
	public virtual void OnTrriger2DEffect(Collider2D _other, int _enemyId){}
}

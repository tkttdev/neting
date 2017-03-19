using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyEffectBase : MonoBehaviour {
	public virtual void Initialize(){}
	public virtual void MoveEffect (){}
	public virtual void DamageEffect (float _damage){}
	public virtual void DeadEffect (){}
	public virtual void OnTrriger2DEffect (Collider2D _other, int _enemyId){}
}

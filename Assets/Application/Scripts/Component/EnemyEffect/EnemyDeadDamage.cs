using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadDamage : EnemyEffectBase {

	[SerializeField] private int damage;

	public override void DeadEffect (){
		base.DeadEffect ();
		BattleShip.I.TakeDamage (damage);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGraduallyDamage : EnemyEffectBase {
	[SerializeField] private float damageParsec = 0.3f;

	public override void MoveEffect (){
		gameObject.GetComponent<Enemy> ().TakeDamage (damageParsec * Time.deltaTime);
		gameObject.GetComponent<Enemy> ().CheckDead ();
	}
}

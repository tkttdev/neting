using UnityEngine;
using System.Collections;

public class Enemy : MoveObjectBase {

    [SerializeField] private int hp;
    public int id;

	protected override void Initialize (){
		base.Initialize ();
		SetMoveToPlayer ();
	}

	protected override void Update (){
		base.Update ();
        if (hp <= 0) {
            Destroy(gameObject);
        }
	}

    public void TakeDamage(int _damage) {
        hp -= _damage;
    }
}

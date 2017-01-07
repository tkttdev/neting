using UnityEngine;
using System.Collections;

public class Enemy : MoveObjectBase {

    [SerializeField] private int hp;
    public int id;
    public float spawnTime;

	protected override void Initialize (){
		base.Initialize ();
		SetMoveToPlayer ();
        spawnTime = Time.timeSinceLevelLoad;
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

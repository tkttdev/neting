using UnityEngine;
using System.Collections;

public class Enemy : MoveObjectBase {

	[SerializeField] private int hp;
	[SerializeField] private int id;
    public float spawnTime;

	protected override void Initialize (){
		base.Initialize ();
		SetMoveToPlayer ();
        spawnTime = Time.timeSinceLevelLoad;
		hp = ENEMY_DEFINE.HP [id];
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

	public void SetId(int _id){
		id = _id;
	}

	public int GetId(){
		return id;
	}
}

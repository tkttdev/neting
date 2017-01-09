using UnityEngine;
using System.Collections;

public class Enemy : MoveObjectBase {
	
	[SerializeField]private int id;
	private int hp;
	private int damage;
    public float spawnTime;

	private EnemyEffectBase[] enemyEffect;

	protected override void Initialize (){
		base.Initialize ();
		SetMoveToPlayer ();
        spawnTime = Time.timeSinceLevelLoad;
		hp = ENEMY_DEFINE.HP [id];
		damage = ENEMY_DEFINE.DAMAGE [id];
		enemyEffect = gameObject.GetComponents<EnemyEffectBase> ();
	}

	protected override void Update (){
		base.Update ();
		for (int i = 0; i < enemyEffect.Length; i++) {
			enemyEffect [i].MoveEffect ();
		}
        if (hp <= 0) {
			DeadEnemy ();
        }
	}

	private void DeadEnemy(){
		for (int i = 0; i < enemyEffect.Length; i++) {
			enemyEffect [i].DeadEffect ();
		}
		Destroy (gameObject);
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

	public void SetMoveSpeed(float _moveSpeed){
		moveSpeed = _moveSpeed;
	}

	public float GetMoveSpeed(){
		return moveSpeed;
	}

	protected override void OnTriggerEnter2D (Collider2D other){
		base.OnTriggerEnter2D (other);
		for (int i = 0; i < enemyEffect.Length; i++) {
			enemyEffect [i].CollisionEffect ();
		}
		if (other.tag == "DamageZone") {
			GameCharacter.I.TakeDamage (damage);
			DeadEnemy ();
		}
	}
}

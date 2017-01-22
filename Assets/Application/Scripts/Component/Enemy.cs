﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Enemy : MoveObjectBase {
	
	[SerializeField]private int id;
	private int hp;
	private int damage;
	private int money;
    public float spawnTime;

	public bool firstColliderAfterCopy = false;

	private EnemyEffectBase[] enemyEffect;

	[SerializeField] private GameObject enemyDeadEffect;

	protected override void Initialize (){
		base.Initialize ();
		SetMoveToPlayer ();
        spawnTime = Time.timeSinceLevelLoad;
		hp = ENEMY_DEFINE.HP [id];
		damage = ENEMY_DEFINE.DAMAGE [id];
		money = ENEMY_DEFINE.MONEY [id];
		enemyEffect = gameObject.GetComponents<EnemyEffectBase> ();
	}

	protected override void Update (){
		base.Update ();
		for (int i = 0; i < enemyEffect.Length; i++) {
			enemyEffect [i].MoveEffect ();
		}
	}

	private void DeadEnemy(){
		for (int i = 0; i < enemyEffect.Length; i++) {
			enemyEffect [i].DeadEffect ();
		}
		DestroyOwn ();
	}

    public void TakeDamage(int _damage) {
        hp -= _damage;
		if (hp <= 0) {
			GetItemManager.I.AddEarnMoney (money);
			SoundManager.I.SoundSE (SE.DEAD);
			Instantiate (enemyDeadEffect, gameObject.transform.position, Quaternion.identity);
			DeadEnemy ();
		}
		StartCoroutine (DamageRendering ());
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

	protected override void OnTriggerEnter2D (Collider2D _other){
		base.OnTriggerEnter2D (_other);
		for (int i = 0; i < enemyEffect.Length; i++) {
			enemyEffect [i].OnTrriger2DEffect (_other, id);
		}
		if (_other.tag == "DamageZone") {
			GameCharacter.I.TakeDamage (damage);
			DestroyOwn ();
		}
	}

	private void DestroyOwn(){
		ExecuteEvents.Execute<IRecieveMessage>(
			target: StageManager.I.gameObject, // 呼び出す対象のオブジェクト
			eventData: null,  // イベントデータ（モジュール等の情報）
			functor: (recieveTarget,y)=>recieveTarget.DeadEnemy());
		Destroy (gameObject);
	}

	private IEnumerator DamageRendering(){
		if (gameObject.GetComponent<SpriteRenderer> ().enabled) {
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		}
		yield return new WaitForSeconds (0.15f);
		if (!gameObject.GetComponent<SpriteRenderer> ().enabled) {
			gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		}
		yield break;
	}
}

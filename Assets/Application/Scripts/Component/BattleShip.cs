﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class BattleShip : SingletonBehaviour<BattleShip> {

	private float intervalCount = 0;
	private int useCharaIndex = 0;
	private float bulletInterval = 2;
	private int bulletStock = 0;
	private int maxBulletStock = 0;
	private int life = 3;
	[SerializeField] private TextAsset bulletSpawnerInfo;
	private GameObject bulletPrefab;

	[SerializeField]private Corner[] bulletSpawnCorner = new Corner[5];

	[SerializeField]private GameObject skillButton;
	private bool activeSkill;
	private bool activeGatling;

	protected override void Initialize() {
		base.Initialize();
		#if UNITY_EDITOR
		if (GameObject.Find("Systems") == null) {
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
	}

	private void Start(){
		LoadSpawnPos ();
		LoadCharaStatus();
	}

	private void LoadSpawnPos(){
		StageManager.I.bulletSpawnCorner.CopyTo (bulletSpawnCorner, 0);
	}

	private void LoadCharaStatus() {
		useCharaIndex = UserDataManager.I.GetUseCharacterIndex();
		bulletInterval = CHARACTER_DEFINE.BULLET_INTERVAL[useCharaIndex];
		maxBulletStock = CHARACTER_DEFINE.MAX_BULLET_STOCK[useCharaIndex];
		bulletPrefab = Resources.Load (CHARACTER_DEFINE.BULLET_PREFAB_PATH [useCharaIndex]) as GameObject;
		bulletStock = maxBulletStock;
		intervalCount = 0;
		UIManager.I.UpdateCharacterInfo (life, bulletStock);
    }

	void Update() {
		if (GameManager.I.CheckGameStatus (GameStatus.PLAY)) {
			if (activeSkill == false) {
				if (bulletStock < maxBulletStock && intervalCount == 0) {
					intervalCount = bulletInterval;
				}

				if (intervalCount > 0.0f) {
					intervalCount -= Time.deltaTime;
					if (intervalCount <= 0.0f) {
						intervalCount = 0.0f;
						bulletStock += 1;
					}
				}
			} else {
				if (bulletStock < 1) {
					bulletPrefab = Resources.Load(CHARACTER_DEFINE.BULLET_PREFAB_PATH[useCharaIndex]) as GameObject;
					bulletStock = maxBulletStock;
					activeSkill = false;
				}
			}
		}

		UIManager.I.UpdateCharacterInfo(life, bulletStock);
	}

	public void Shoot(int _entryX) {
		if (bulletStock == 0 || !GameManager.I.CheckGameStatus(GameStatus.PLAY) || bulletSpawnCorner[_entryX + 2] == null) {
			return;
		}
		if (activeGatling == false) {
			bulletStock--;
		}
		UIManager.I.UpdateCharacterInfo (life, bulletStock);
		Bullet bullet = ObjectPool.I.Instantiate (bulletPrefab, new Vector3 ((float)_entryX, -4f, 0.0f)).GetComponent<Bullet> ();
		if (bulletSpawnCorner [_entryX + 2].CheckCurve (MoveDir.UP, 1, bullet.moveMode)) {
			bullet.isCurve = true;
			bullet.bezerPoints = bulletSpawnCorner [_entryX + 2].ChangePurposeCurve (ref bullet.moveDir, 1, ref bullet.lineId, ref bullet.onLineLength, ref bullet.lengthOfBezerSection, bullet.moveMode);
		} else {
			bullet.endPos = bulletSpawnCorner [_entryX + 2].ChangePurposeStraight (ref bullet.moveDir, 1, ref bullet.lineId, ref bullet.onLineLength, bullet.moveMode);
		}
		SoundManager.I.SoundSE (SE.SHOOT);
	}

	public void TakeDamage(int _damage){
		#if UNITY_EDITOR
		if(GameManager.I.isDemo) return;
		#endif
		StartCoroutine(DamageRendering());
		life -= _damage;
		UIManager.I.UpdateCharacterInfo (life, bulletStock);
		if (life <= 0) {
			GameManager.I.SetStatuEnd ();
		}
	}

	private IEnumerator DamageRendering(){
		for (int i = 0; i < 1; i++) {
			iTween.MoveTo (Camera.main.gameObject, iTween.Hash ("x", 0.3f, "time", 0.05f, "easeType", iTween.EaseType.easeInExpo));
			yield return new WaitForSeconds (0.05f);
			iTween.MoveTo (Camera.main.gameObject, iTween.Hash ("x", -0.3f, "time", 0.1f, "easeType", iTween.EaseType.easeInExpo));
			yield return new WaitForSeconds (0.1f);
		}
		iTween.MoveTo (Camera.main.gameObject, iTween.Hash ("x", 0f, "time", 0.05f, "easeType", iTween.EaseType.easeInExpo));
		yield return new WaitForSeconds (0.05f);
	}

	public int GetLife(){
		return life;
	}

	public float bulletRate{
		get { return (bulletInterval - intervalCount) / bulletInterval; }
	}

	public void SetSkill(GameObject prefab) {
		if(bulletStock != maxBulletStock) {
			intervalCount = 0;
			bulletStock = maxBulletStock;
        }

        bulletPrefab = prefab;
		activeSkill = true;
	}

	public void SetGatling() {
		if (bulletStock != maxBulletStock) {
			intervalCount = 0;
			bulletStock = maxBulletStock;
		}

		activeGatling = true;

		StartCoroutine("EndSkill");
	}

	public IEnumerator EndSkill() {
		yield return new WaitForSeconds(5.0f);

		bulletPrefab = Resources.Load(CHARACTER_DEFINE.BULLET_PREFAB_PATH[useCharaIndex]) as GameObject;
		bulletStock = maxBulletStock;
		activeGatling = false;
	}
}

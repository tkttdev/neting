﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCharacter : SingletonBehaviour<GameCharacter> {

	[SerializeField] private int fullTime = 2;
	private float intervalCount = 0;
	private int entryX = 0;
	[SerializeField] private Text countText;
	[SerializeField] private Text stockText;
	[SerializeField] private Text lifeText;
	private int useCharaIndex = 0;
	private float bulletInterval = 2;
	private int bulletStock = 0;
	private int maxBulletStock = 0;
	private GameObject charaBulletPrefab;
	private int life = 0;
	[SerializeField] private bool isDemo = false;


	protected override void Initialize() {
		base.Initialize();

		#if UNITY_EDITOR
		if (GameObject.Find("Systems") == null) {
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif

		LoadCharaStatus();
	}

	private void LoadCharaStatus() {
		useCharaIndex = UserDataManager.I.GetUseCharaIndex();
		bulletInterval = CHARACTER_DEFINE.BULLET_INTERVAL[useCharaIndex];
		maxBulletStock = CHARACTER_DEFINE.MAX_BULLET_STOCK[useCharaIndex];
		charaBulletPrefab = Resources.Load(CHARACTER_DEFINE.BULLET_PREFAB_PATHS[useCharaIndex]) as GameObject;
		life = CHARACTER_DEFINE.LIFE[useCharaIndex];
	}

	void Update() {
		countText.text = string.Format("{0:f3}", intervalCount);
		lifeText.text = life.ToString ();
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

		stockText.text = bulletStock.ToString();
		if (bulletStock != 0 && Input.GetMouseButtonDown(0)) {
			bulletStock--;
			if (Input.mousePosition.x < Screen.width / 5.0f) {
				entryX = -2;
			} else if (Input.mousePosition.x < (Screen.width / 5.0f) * 2.0f) {
				entryX = -1;
			} else if (Input.mousePosition.x < (Screen.width / 5.0f) * 3.0f) {
				entryX = 0;
			} else if (Input.mousePosition.x < (Screen.width / 5.0f) * 4.0f) {
				entryX = 1;
			} else {
				entryX = 2;
			}
			Shoot();
		}


	}

	private void Shoot() {
		Instantiate(charaBulletPrefab, new Vector3((float)entryX, -4.0f, 0.0f), Quaternion.Euler(0, 0, 0));
	}

	public void TakeDamage(int _damage){
		life -= _damage;
	}
}

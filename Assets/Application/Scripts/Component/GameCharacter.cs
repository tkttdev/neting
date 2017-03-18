using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameCharacter : SingletonBehaviour<GameCharacter> {

	private float intervalCount = 0;
	private int useCharaIndex = 0;
	private float bulletInterval = 2;
	private int bulletStock = 0;
	private int maxBulletStock = 0;
	private int life = 3;
	[SerializeField] private TextAsset bulletSpawnerInfo;
	private bool[] beAbleSpawn = new bool[5];
	private GameObject bulletPrefab;

	protected override void Initialize() {
		base.Initialize();
		#if UNITY_EDITOR
		if (GameObject.Find("Systems") == null) {
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif

		LoadSpawnPos ();
		LoadCharaStatus();
	}

	private void LoadSpawnPos(){
		StringReader sr = new StringReader(bulletSpawnerInfo.text);
		for (int i = 0; i < StageLevelManager.I.GetStageLevel () - 1; i++) {
			sr.ReadLine ();
		}
		string[] line = sr.ReadLine ().Split (',');
		for (int i = 0; i < line.Length; i++) {
			beAbleSpawn[i] = (line [i] == "1");
		}
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

			UIManager.I.UpdateCharacterInfo (life, bulletStock);
		}

	}

	public void Shoot(int _entryX) {
		if (bulletStock == 0 || !beAbleSpawn [_entryX + 2] || !GameManager.I.CheckGameStatus(GameStatus.PLAY)) {
			return;
		}
		bulletStock--;
		UIManager.I.UpdateCharacterInfo (life, bulletStock);
		ObjectPool.I.Instantiate (bulletPrefab, new Vector3 ((float)_entryX, -3.8f, 0.0f));
		SoundManager.I.SoundSE (SE.SHOOT);
	}

	public void TakeDamage(int _damage){
		life -= _damage;
		UIManager.I.UpdateCharacterInfo (life, bulletStock);
		if (life <= 0) {
			GameManager.I.SetStatuEnd ();
		}
	}

	public int GetLife(){
		return life;
	}

	public float bulletRate{
		get { return (bulletInterval - intervalCount) / bulletInterval; }
	}
}

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
	private int[] beAbleSpawn = new int[5];
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
		#if UNITY_EDITOR
		if(GameManager.I.isDemo){
			StageLevelManager.I.SetStageLevel(GameManager.I.demoLevel);
		}
		#endif
		StringReader sr = new StringReader(bulletSpawnerInfo.text);
		for (int i = 0; i < StageLevelManager.I.GetStageLevel () - 1; i++) {
			sr.ReadLine ();
		}
		string[] line = sr.ReadLine ().Split (',');
		for (int i = 0; i < line.Length; i++) {
			beAbleSpawn[i] =  int.Parse(line [i]);
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
		if (bulletStock == 0 || !(beAbleSpawn [_entryX + 2] > -1) || !GameManager.I.CheckGameStatus(GameStatus.PLAY)) {
			return;
		}
		bulletStock--;
		UIManager.I.UpdateCharacterInfo (life, bulletStock);
		ObjectPool.I.Instantiate (bulletPrefab, new Vector3 ((float)_entryX, -3.8f, 0.0f)).GetComponent<MoveObjectBase> ().lineId = beAbleSpawn[_entryX + 2];
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
}

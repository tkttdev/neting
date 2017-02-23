using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameCharacter : SingletonBehaviour<GameCharacter> {

	[SerializeField] private int fullTime = 2;
	private float intervalCount = 0;
	private int entryX = 0;
	private int useCharaIndex = 0;
	private float bulletInterval = 2;
	private float bulletChargeTime = 0.0f;
	private int bulletStock = 0;
	private int maxBulletStock = 0;
	private GameObject charaBulletPrefab;
	private int life = 3;
	[SerializeField] private bool isDemo = false;
	[SerializeField] private TextAsset bulletSpawnerInfo;
	private bool[] beAbleSpawn = new bool[5];
	private float bulletThresholdX;

	protected override void Initialize() {
		base.Initialize();
		//bulletThresholdX  = Screen.width / 5.0f;
		bulletThresholdX = 74.2f;
		Debug.Log (Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0, 10)));
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
		charaBulletPrefab = Resources.Load(CHARACTER_DEFINE.BULLET_PREFAB_PATH[useCharaIndex]) as GameObject;
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

			if (bulletStock != 0 && Input.GetMouseButtonDown (0)) {
				float inputX = Input.mousePosition.x;
				if (inputX < bulletThresholdX) {
					if (beAbleSpawn [0]) {
						entryX = -2;
						Shoot ();
					}
				} else if (inputX < bulletThresholdX * 2.0f) {
					if (beAbleSpawn [1]) {
						entryX = -1;
						Shoot ();
					}
				} else if (inputX < bulletThresholdX * 3.0f) {
					if (beAbleSpawn [2]) {
						entryX = 0;
						Shoot ();
					}
				} else if (inputX < bulletThresholdX * 4.0f) {
					if (beAbleSpawn [3]) {
						entryX = 1;
						Shoot ();
					}
				} else if (beAbleSpawn [4]) {
					if (beAbleSpawn [4]) {
						entryX = 2;
						Shoot ();
					}
				}
			}
			UIManager.I.UpdateCharacterInfo (life, bulletStock);
		}

	}

	private void Shoot() {
		bulletStock--;
		UIManager.I.UpdateCharacterInfo (life, bulletStock);
		Instantiate(charaBulletPrefab, new Vector3((float)entryX, -3.8f, 0.0f), Quaternion.Euler(0, 0, 0));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
	private int life = 3;
	[SerializeField] private bool isDemo = false;
	[SerializeField] private TextAsset bulletSpawnerInfo;
	private bool[] beAbleSpawn = new bool[5];
	private float bulletThresholdX;

	protected override void Initialize() {
		base.Initialize();
		bulletThresholdX  = Screen.width / 5.0f;
		lifeText.text = life.ToString ();
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
	}

	void Update() {
		if (GameManager.I.CheckGameStatus (GameStatus.PLAY)) {
			countText.text = string.Format ("{0:f3}", intervalCount);
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

			stockText.text = bulletStock.ToString ();
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
		}

	}

	private void Shoot() {
		bulletStock--;
		Instantiate(charaBulletPrefab, new Vector3((float)entryX, -4.0f, 0.0f), Quaternion.Euler(0, 0, 0));
	}

	public void TakeDamage(int _damage){
		life -= _damage;
		lifeText.text = life.ToString ();
		if (life <= 0) {
			GameManager.I.SetEnd ();
		}
	}

	public int GetLife(){
		return life;
	}
}

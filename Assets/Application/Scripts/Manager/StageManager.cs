using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;

public class StageManager : SingletonBehaviour<StageManager>, IRecieveMessage {

	[SerializeField] private Transform[] enemySpawnPos;
	[SerializeField] private Transform[] itemSpawnPos;
	[SerializeField] private TextAsset enemySpawnInfoText;
	[SerializeField] private TextAsset itemSpawnInfoText; 
	private GameObject[] enemyPrefabs = new GameObject[ENEMY_DEFINE.enemyVarietyNum];
	private GameObject[] itemPrefabs = new GameObject[10];

	private EnemySpawnInfo enemySpawnInfo = new EnemySpawnInfo();
	private ItemSpawnInfo itemSpawnInfo = new ItemSpawnInfo();

	private int destroyEnemyNumInWave = 0;

	private int maxWaveNum = 0;
	private int waveNum = -1;

	private float waveStartTime = 0.0f;
	private float wavePlayTime = 0.0f;

	private int duplicateEnemyNum = 0;

	// Use this for initialization
	protected override void Initialize () {
		ParseEnemySpawnInfoText ();
		ParseItemSpawnInfoText ();
	}

	private void ParseEnemySpawnInfoText() {
		//EnemySpawnInfo.csv => enemyID,spawnTime,spawnPos
		StringReader sr = new StringReader(enemySpawnInfoText.text);
		List<int> id = new List<int> ();
		List<float> spawnTime = new List<float> ();
		List<int> spawnPos = new List<int> ();

		while(sr.Peek() > -1) {
			string line = sr.ReadLine();
			string[] values = line.Split(',');
			if (values.Length == 1) {
				enemySpawnInfo.id.Add (new List<int> (id));
				enemySpawnInfo.spawnTime.Add (new List<float> (spawnTime));
				enemySpawnInfo.spawnPos.Add (new List<int> (spawnPos));
				enemySpawnInfo.allWaveEnemyNum.Add (id.Count + duplicateEnemyNum);
	
				id.Clear ();
				spawnTime.Clear ();
				spawnPos.Clear ();
				duplicateEnemyNum = 0;
			} else {
				if (int.Parse (values [0]) == 5) {
					duplicateEnemyNum++;
				}
				id.Add (int.Parse (values [0]));
				spawnTime.Add (float.Parse (values [1]));
				spawnPos.Add (int.Parse (values [2]));
			}
		}
		maxWaveNum = enemySpawnInfo.id.Count;
	}

	private void ParseItemSpawnInfoText(){
		if (itemSpawnInfoText == null) {
			return;
		}
		//ItemSpawnInfo.csv => itenID,spawnTime,destroyTime,spawnPos
		StringReader sr = new StringReader(itemSpawnInfoText.text);
		List<int> id = new List<int> ();
		List<float> spawnTime = new List<float> ();
		List<float> destroyTime = new List<float> ();
		List<int> spawnPos = new List<int> ();

		while(sr.Peek() > -1) {
			string line = sr.ReadLine();
			string[] values = line.Split(',');
			if (values.Length == 1) {
				if (int.Parse (values [0]) != 0) {
					itemSpawnInfo.id.Add (new List<int> (id));
					itemSpawnInfo.spawnTime.Add (new List<float> (spawnTime));
					itemSpawnInfo.destroyTime.Add (new List<float> (destroyTime));
					itemSpawnInfo.spawnPos.Add (new List<int> (spawnPos));

				}
				id.Clear ();
				spawnTime.Clear ();
				destroyTime.Clear ();
				spawnPos.Clear ();
			} else {
				id.Add (int.Parse (values [0]));
				spawnTime.Add (float.Parse (values [1]));
				destroyTime.Add (float.Parse (values [2]));
				spawnPos.Add (int.Parse (values [3]));
			}
		}
	}

	public void StartNextWave(){
		waveNum++;
		destroyEnemyNumInWave = 0;
		UIManager.I.WaveStart (waveNum+1, maxWaveNum);
		Debug.Log ("Start wave " + waveNum.ToString ());
		Debug.Log ("ALL ENEMY NUM " + enemySpawnInfo.allWaveEnemyNum [waveNum]);
	}

	public void StartSpawn(){
		waveStartTime = Time.timeSinceLevelLoad;
		wavePlayTime = Time.timeSinceLevelLoad;
	}

	// Update is called once per frame
	void Update() {
		if (GameManager.I.CheckGameStatus(GameStatus.PLAY)) {
			wavePlayTime += Time.deltaTime;
			CheckSpawnEnemy();
		}
	}

	private void CheckSpawnEnemy() {
		if (enemySpawnInfo.id[waveNum].Count == 0) {
			return;
		}

		while (wavePlayTime - waveStartTime > enemySpawnInfo.spawnTime[waveNum][0]) {
			SpawnEnemy();
			if (enemySpawnInfo.id[waveNum].Count == 0) {
				return;
			}
		}
	}

	private void SpawnEnemy() {
		if (enemyPrefabs[enemySpawnInfo.id[waveNum][0]] == null) {
			enemyPrefabs [enemySpawnInfo.id[waveNum][0]] = Resources.Load (ENEMY_DEFINE.PATH [enemySpawnInfo.id[waveNum][0]]) as GameObject;
			enemyPrefabs [enemySpawnInfo.id[waveNum][0]].GetComponent<Enemy> ().SetId (enemySpawnInfo.id[waveNum][0]);
		}
		Instantiate(enemyPrefabs[enemySpawnInfo.id[waveNum][0]], enemySpawnPos[enemySpawnInfo.spawnPos[waveNum][0]].position, Quaternion.identity);

		enemySpawnInfo.RemoveFirstElement (waveNum);

	}

	public void DeadEnemy(int _id, bool _isCopy){
		if (_id == 5 && !_isCopy) {
			destroyEnemyNumInWave += 2;
		} else {
			destroyEnemyNumInWave++;
		}
		if (enemySpawnInfo.allWaveEnemyNum [waveNum] == destroyEnemyNumInWave) {
			destroyEnemyNumInWave = 0;
			Debug.Log ("END WAVE");
			if (GameCharacter.I.GetLife () > 0) {
				if (waveNum < maxWaveNum - 1) {
					GameManager.I.SetStatusWait ();
					StartNextWave ();
				} else {
					GameManager.I.SetStatuEnd ();
				}
			}
		}
	}

	private class EnemySpawnInfo{
		public List<List<int>> id = new List<List<int>>();
		public List<List<float>> spawnTime = new List<List<float>> ();
		public List<List<int>> spawnPos = new List<List<int>>();
		public List<int> allWaveEnemyNum = new List<int> ();

		public void RemoveFirstElement(int _waveNum){
			id [_waveNum].RemoveAt (0);
			spawnTime [_waveNum].RemoveAt (0);
			spawnPos [_waveNum].RemoveAt (0);
		}
	}

	private class ItemSpawnInfo{
		public List<List<int>> id = new List<List<int>>();
		public List<List<float>> spawnTime = new List<List<float>> ();
		public List<List<float>> destroyTime = new List<List<float>> ();
		public List<List<int>> spawnPos = new List<List<int>>();

		public void RemoveFirstElement(int _waveNum){
			id [_waveNum].RemoveAt (0);
			spawnTime [_waveNum].RemoveAt (0);
			destroyTime [_waveNum].RemoveAt (0);
			spawnPos [_waveNum].RemoveAt (0);
		}
	}
}
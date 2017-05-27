using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;

public class EnemySpawner : MonoBehaviour {

	#region PublicField
	public Corner[] enemySpawnCorner;
	#endregion

	#region PrivateField
	[SerializeField] private TextAsset enemySpawnInfoText;
	private GameObject[] enemyPrefabs = new GameObject[ENEMY_DEFINE.enemyVarietyNum];
	private EnemySpawnInfo enemySpawnInfo = new EnemySpawnInfo();
	private List<EnemySpawnInfo> bossStageEnemySpawnInfo = new List<EnemySpawnInfo> ();
	private float playTime = 0.0f;
	#endregion

	void Awake(){
		ParseEnemySpawnInfoText ();
	}

	private void ParseEnemySpawnInfoText() {
		//EnemySpawnInfo.csv => enemyID,spawnTime,spawnPos
		StringReader sr = new StringReader(enemySpawnInfoText.text);
		int i = 0;
		List<int> id = new List<int> ();
		List<float> spawnTime = new List<float> ();
		List<int> spawnPos = new List<int> ();

		while(sr.Peek() > -1) {
			string line = sr.ReadLine();
			string[] values = line.Split(',');
			if (StageManager.I.isBossStage) {
				if (values.Length != 1) {
					id.Add (int.Parse (values [0]));
					spawnTime.Add (float.Parse (values [1]));
					spawnPos.Add (int.Parse (values [2]));
				} else if(values.Length == 3) {
					bossStageEnemySpawnInfo.Add (new EnemySpawnInfo ());
					bossStageEnemySpawnInfo [i].id = new List<int> (id);
					bossStageEnemySpawnInfo [i].spawnTime = new List<float> (spawnTime);
					bossStageEnemySpawnInfo [i].spawnPos = new List<int> (spawnPos);
					i++;
					id.Clear ();
					spawnTime.Clear ();
					spawnPos.Clear ();
				}
			} else if(values.Length == 3){
				enemySpawnInfo.id.Add (int.Parse (values [0]));
				enemySpawnInfo.spawnTime.Add (float.Parse (values [1]));
				enemySpawnInfo.spawnPos.Add (int.Parse (values [2]));
			}
		}
		if (StageManager.I.isBossStage) {
			StageManager.I.SetAllEnemyNum (-1);
		} else {
			StageManager.I.SetAllEnemyNum (enemySpawnInfo.id.Count);
		}
	}

	// Update is called once per frame
	void Update() {
		if (GameManager.I.CheckGameStatus(GameStatus.PLAY)) {
			playTime += Time.deltaTime;
			CheckSpawnEnemy();
		}
	}

	private void CheckSpawnEnemy() {
		if (enemySpawnInfo.id.Count == 0 && StageManager.I.isBossStage) {
			int i = Random.Range (0, bossStageEnemySpawnInfo.Count);
			enemySpawnInfo.id = new List<int> (bossStageEnemySpawnInfo [i].id);
			enemySpawnInfo.spawnTime = new List<float> (bossStageEnemySpawnInfo [i].spawnTime);
			enemySpawnInfo.spawnPos = new List<int> (bossStageEnemySpawnInfo [i].spawnPos);
			playTime = 0f;
			return;
		} else if (enemySpawnInfo.id.Count == 0) {
			return;
		}
		while (playTime > enemySpawnInfo.spawnTime[0]) {
			SpawnEnemy();
			if (enemySpawnInfo.id.Count == 0) {
				return;
			}
		}
	}

	private void SpawnEnemy() {
		if (enemyPrefabs [enemySpawnInfo.id[0]] == null) {
			enemyPrefabs [enemySpawnInfo.id[0]] = Resources.Load (ENEMY_DEFINE.PATH [enemySpawnInfo.id[0]]) as GameObject;
			enemyPrefabs [enemySpawnInfo.id[0]].GetComponent<Enemy> ().SetId (enemySpawnInfo.id[0]);
		}
		Enemy enemy = ObjectPool.I.Instantiate (enemyPrefabs [enemySpawnInfo.id[0]], enemySpawnCorner [enemySpawnInfo.spawnPos[0]].transform.position).GetComponent<Enemy> ();
		if (enemySpawnCorner [enemySpawnInfo.spawnPos[0]].CheckCurve (MoveDir.DOWN, -1, enemy.moveMode)) {
			enemy.isCurve = true;
			enemy.bezerPoints = enemySpawnCorner [enemySpawnInfo.spawnPos[0]].ChangePurposeCurve (ref enemy.moveDir, -1, ref enemy.lineId, ref enemy.onCurveLength, ref enemy.lengthOfBezerSection, enemy.moveMode);
		} else {
			enemy.slope = enemySpawnCorner [enemySpawnInfo.spawnPos[0]].ChangePurposeStraight (ref enemy.moveDir, -1, ref enemy.lineId, enemy.moveMode);
		}
		enemySpawnInfo.RemoveFirstElement ();
	}

	private class EnemySpawnInfo{
		public List<int> id = new List<int>();
		public List<float> spawnTime = new List<float> ();
		public List<int> spawnPos = new List<int>();
		public List<int> enemyNum = new List<int> ();

		public void RemoveFirstElement(){
			id.RemoveAt (0);
			spawnTime.RemoveAt (0);
			spawnPos.RemoveAt (0);
		}
	}
}
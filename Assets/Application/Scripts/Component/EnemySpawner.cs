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
	private float playTime = 0.0f;
	#endregion

	void Awake(){
		ParseEnemySpawnInfoText ();
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
			if (values.Length != 1) {
				enemySpawnInfo.id.Add (int.Parse (values [0]));
				enemySpawnInfo.spawnTime.Add (float.Parse (values [1]));
				enemySpawnInfo.spawnPos.Add (int.Parse (values [2]));
			}
		}

		StageManager.I.SetAllEnemyNum (enemySpawnInfo.id.Count);
	}

	// Update is called once per frame
	void Update() {
		if (GameManager.I.CheckGameStatus(GameStatus.PLAY)) {
			playTime += Time.deltaTime;
			CheckSpawnEnemy();
		}
	}

	private void CheckSpawnEnemy() {
		if (enemySpawnInfo.id.Count == 0) {
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
		if (enemyPrefabs[enemySpawnInfo.id[0]] == null) {
			enemyPrefabs [enemySpawnInfo.id[0]] = Resources.Load (ENEMY_DEFINE.PATH [enemySpawnInfo.id[0]]) as GameObject;
			enemyPrefabs [enemySpawnInfo.id[0]].GetComponent<Enemy> ().SetId (enemySpawnInfo.id[0]);
		}
		Enemy enemy = ObjectPool.I.Instantiate (enemyPrefabs [enemySpawnInfo.id[0]], enemySpawnCorner [enemySpawnInfo.spawnPos[0]].transform.position).GetComponent<Enemy> ();
		if (enemySpawnCorner [enemySpawnInfo.spawnPos[0]].CheckCurve (MoveDir.DOWN, -1)) {
			enemy.isCurve = true;
			enemy.bezerPoints = enemySpawnCorner [enemySpawnInfo.spawnPos[0]].ChangePurposeCurve (ref enemy.moveDir, -1, ref enemy.lineId, ref enemy.onCurveLength, ref enemy.lengthOfBezerSection);
		} else {
			enemy.slope = enemySpawnCorner [enemySpawnInfo.spawnPos[0]].ChangePurposeStraight (ref enemy.moveDir, -1, ref enemy.lineId);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;

public class EnemySpawner : SingletonBehaviour<EnemySpawner> {

    [SerializeField] private Transform[] spawnerPos;
    [SerializeField] private TextAsset spawnerInfoText;
    private SpawnerInfo spawnerInfo = new SpawnerInfo();
    private GameObject[] enemyPrefabs = new GameObject[ENEMY_DEFINE.enemyVarietyNum];

    private float playTime = 0.0f;
    private int allEnemyNum = 0;

	private int maxWave = 0;
	private int waveNum = 0;

	// Use this for initialization
	void Start () {
		ParseSpawnerInfoText();
        playTime = 0.0f;
	}

    private void ParseSpawnerInfoText() {
        //spawnerInfoText.csv => enemyID,spawnerTime,spawnerPos
        StringReader sr = new StringReader(spawnerInfoText.text);
        while(sr.Peek() > -1) {
            string line = sr.ReadLine();
            string[] values = line.Split(',');
            spawnerInfo.enemyId.Add(int.Parse(values[0]));
            spawnerInfo.spawnTime.Add(float.Parse(values[1]));
            spawnerInfo.spawnPos.Add(int.Parse(values[2]));
			spawnerInfo.lineLayer.Add (1);
        }
        allEnemyNum = spawnerInfo.enemyId.Count;
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.I.CheckGameStatus(GameStatus.PLAY)) {
            playTime += Time.deltaTime;
            CheckSpawn();
        }
    }

    private void CheckSpawn() {
        if (spawnerInfo.allSpawnEnemyNum == allEnemyNum) {
            return;
        }

        while (playTime > spawnerInfo.spawnTime[0]) {
            SpawnEnemy();
            if (spawnerInfo.allSpawnEnemyNum == allEnemyNum) {
                return;
            }
        }
    }

    private void SpawnEnemy() {
        if (enemyPrefabs[spawnerInfo.enemyId[0]] == null) {
			enemyPrefabs [spawnerInfo.enemyId [0]] = Resources.Load (ENEMY_DEFINE.PATH [spawnerInfo.enemyId [0]]) as GameObject;
			enemyPrefabs [spawnerInfo.enemyId [0]].GetComponent<Enemy> ().SetId (spawnerInfo.enemyId [0]);
        }
		ObjectPool.I.Instantiate (enemyPrefabs [spawnerInfo.enemyId [0]], spawnerPos [spawnerInfo.spawnPos [0]].position);
		//spawnEnemy.transform.name = "Enemy" + spawnerInfo.allSpawnEnemyNum.ToString ();

        spawnerInfo.enemyId.RemoveAt(0);
        spawnerInfo.spawnTime.RemoveAt(0);
        spawnerInfo.spawnPos.RemoveAt(0);
		spawnerInfo.lineLayer.RemoveAt (0);
		spawnerInfo.allSpawnEnemyNum++;
    }
}

public class SpawnerInfo {
    public List<int> enemyId = new List<int>();
    public List<float> spawnTime = new List<float>();
    public List<int> spawnPos = new List<int>();
	public List<int> lineLayer = new List<int>();
    public int allSpawnEnemyNum = 0;
}
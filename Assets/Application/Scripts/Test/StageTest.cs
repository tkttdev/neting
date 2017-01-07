using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StageTest : GameManager {

    [SerializeField] private GameObject[] testStagePrefabs;
    private EnemySpawner testStageSpawner;
    private List<float> enemyArriveTime = new List<float>();
    private List<int> enemyId = new List<int>();
    private int testStageIndex = 0;
    private float spendGameTime = 0.0f;
    private bool isSpawn = false;
    private string stageName = "";

    private int[] charaLifes = new int[CHARACTER_DEFINE.characterVarietyNum];
    private float[] charaBulletIntervals = new float[CHARACTER_DEFINE.characterVarietyNum];
    private int[] charaMaxBulletStocks = new int[CHARACTER_DEFINE.characterVarietyNum];
    private int[] characterBulletDamages = new int[CHARACTER_DEFINE.characterVarietyNum];

    private void Start() {
        StartTest();
    }

    private void Update() {
        if (isSpawn) {
            spendGameTime += Time.deltaTime;
        }
    }

    private void StartTest() {
        GameObject testStage = Instantiate(testStagePrefabs[testStageIndex++]);
        stageName = testStage.transform.name;
        testStageSpawner = testStage.GetComponent<EnemySpawner>();
        gameStatus = GameStatus.PLAY;
        spendGameTime = 0.0f;
        isSpawn = true;
    }

    private void TestChara() {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        enemyId.Add(other.GetComponent<Enemy>().id);
        enemyArriveTime.Add(spendGameTime);
        if(enemyId.Count == testStageSpawner.allEnemyNum) {
            isSpawn = false;
            gameStatus = GameStatus.END;
            TestChara();
        }
    }
}
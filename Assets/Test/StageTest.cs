using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using UnityEditor;

public class StageTest : GameManager {

    [SerializeField] private GameObject[] testStagePrefabs;
    private EnemySpawner testStageSpawner;
    private List<float> enemySpawnTime = new List<float>();
    private List<float> enemyArriveTime = new List<float>();
    private List<int> enemyId = new List<int>();
    private List<int> enemyHP = new List<int>();
    private List<int> enemyDamage = new List<int>();
    private int testStageIndex = 0;
    private float spendGameTime = 0.0f;
    private bool isSpawn = false;
    private string stageName = "";

    private int[] charaLife = new int[CHARACTER_DEFINE.characterVarietyNum];
    private float[] charaBulletInterval = new float[CHARACTER_DEFINE.characterVarietyNum];
    private int[] charaMaxBulletStock = new int[CHARACTER_DEFINE.characterVarietyNum];
    private int[] charaBulletDamage = new int[CHARACTER_DEFINE.characterVarietyNum];
    private int[] charaBulletStock = new int[CHARACTER_DEFINE.characterVarietyNum];

    private int[] testCharaLife = new int[CHARACTER_DEFINE.characterVarietyNum];
    private float[] testCharaBulletInterval = new float[CHARACTER_DEFINE.characterVarietyNum];
    private int[] testCharaBulletStock = new int[CHARACTER_DEFINE.characterVarietyNum];

    private float stageStartTime = 0.0f;
	private bool notFirstTest = false;

    private void Start() {
		notFirstTest = false;

        for (int i = 0; i < CHARACTER_DEFINE.characterVarietyNum; i++) {
            //charaLife[i] = CHARACTER_DEFINE.LIFE[i];
            charaBulletInterval[i] = CHARACTER_DEFINE.BULLET_INTERVAL[i];
            charaMaxBulletStock[i] = CHARACTER_DEFINE.MAX_BULLET_STOCK[i];
            charaBulletDamage[i] = CHARACTER_DEFINE.BULLET_DAMAGE[i];
        }
        StartTest();
    }

    private void TestCharaInit() {
        for (int i = 0; i < CHARACTER_DEFINE.characterVarietyNum; i++) {
            //testCharaLife[i] = charaLife[i];
            testCharaBulletInterval[i] = 0.0f;
            testCharaBulletStock[i] = 0;
        }
    }

    private void Update() {
        if (isSpawn) {
            spendGameTime += Time.deltaTime;
        }
    }

    private void StartTest() {
        stageStartTime = Time.timeSinceLevelLoad;
        gameStatus = GameStatus.PLAY;
        GameObject testStage = Instantiate(testStagePrefabs[testStageIndex]);
        stageName = testStagePrefabs[testStageIndex].transform.name;
        testStageIndex++;
        testStageSpawner = testStage.GetComponent<EnemySpawner>();
        spendGameTime = 0.0f;

        //enemy情報格納部分初期化
        enemySpawnTime.Clear();
        enemyArriveTime.Clear();
        enemyId.Clear();
        enemyHP.Clear();
        enemyDamage.Clear();

        isSpawn = true;
    }

    private void TestChara() {
        TestCharaInit();
        
        for (int charaId = 0; charaId < CHARACTER_DEFINE.characterVarietyNum; charaId++) {
            //テスト用にステージ実行により得られた敵のデータを複製
			List<int> testEnemyId = new List<int>(enemyId);
            List<float> testEnemySpawnTime = new List<float>(enemySpawnTime);
            List<float> testEnemyArriveTime = new List<float>(enemyArriveTime);
            List<int> testEnemyHP = new List<int>(enemyHP);
            List<int> testEnemyDamage = new List<int>(enemyDamage);

			//テスト結果用リスト
			List<float> testArriveEnemySpawnTime = new List<float>();
			List<int> testArriveEnemyId = new List<int> ();
			List<int> testArriveEnemyHP = new List<int> ();

			List<float> testDeadEnemySpawnTime = new List<float> ();
			List<int> testDeadEnemyId = new List<int> ();
			List<float> expectedUserMarginTime = new List<float> ();

            for (float time = 0.0f; time <= spendGameTime + 0.5f; time += 1.0f / 60.0f) {
                //敵到着の処理
                List<int> removeIndex = new List<int>();
                foreach (float eAriveTime in testEnemyArriveTime) {
					if (time > eAriveTime) {
						testCharaLife [charaId] -= testEnemyDamage [testEnemyArriveTime.IndexOf (eAriveTime)];
						removeIndex.Add (testEnemyArriveTime.IndexOf (eAriveTime));

						testArriveEnemySpawnTime.Add (testEnemySpawnTime [testEnemyArriveTime.IndexOf (eAriveTime)]);
						testArriveEnemyId.Add (testEnemyId [testEnemyArriveTime.IndexOf (eAriveTime)]);
						testArriveEnemyHP.Add (testEnemyHP [testEnemyArriveTime.IndexOf (eAriveTime)]);
					}
                }

                foreach(int index in removeIndex) {
					testEnemyId.RemoveAt (index);
                    testEnemySpawnTime.RemoveAt(index);
                    testEnemyArriveTime.RemoveAt(index);
                    testEnemyHP.RemoveAt(index);
                    testEnemyDamage.RemoveAt(index);
                }

                removeIndex.Clear();

                //弾のinterval、shoot処理
                //stock処理
                if (testCharaBulletStock[charaId] < charaMaxBulletStock[charaId]) {
					testCharaBulletInterval [charaId] += 1.0f / 60.0f;
                }
                if (testCharaBulletInterval[charaId] > charaBulletInterval[charaId]) {
                    testCharaBulletInterval[charaId] = 0.0f;
                    testCharaBulletStock[charaId]++;
                }
                //shoot処理
                while (testCharaBulletStock[charaId] > 0) {
                    foreach (float eSpawnTime in testEnemySpawnTime) {
                        if (time > eSpawnTime) {
                            int targetIndex = testEnemySpawnTime.IndexOf(eSpawnTime);
                            if (testEnemyHP[targetIndex] <= 0) {
                                continue;
                            }
                            testCharaBulletStock[charaId]--;
                            testEnemyHP[targetIndex] -= charaBulletDamage[charaId];
							//敵キャラクター死亡処理
                            if (testEnemyHP[targetIndex] <= 0) {
                                removeIndex.Add(targetIndex);
								testDeadEnemyId.Add (testEnemyId [targetIndex]);
								testDeadEnemySpawnTime.Add (testEnemySpawnTime [targetIndex]);
								expectedUserMarginTime.Add (testEnemyArriveTime[targetIndex] - time);
                            }
                            if(testCharaBulletStock[charaId] == 0) {
                                break;
                            }
                        }
                    }
                    break;
                }
                
                foreach (int index in removeIndex) {
					testEnemyId.RemoveAt (index);
                    testEnemySpawnTime.RemoveAt(index);
                    testEnemyArriveTime.RemoveAt(index);
                    testEnemyHP.RemoveAt(index);
                    testEnemyDamage.RemoveAt(index);
                }

                removeIndex.Clear();


            }
			SaveResult (charaId, testCharaLife, testArriveEnemySpawnTime, testArriveEnemyId, testArriveEnemyHP, testDeadEnemySpawnTime, testDeadEnemyId, expectedUserMarginTime);
        }

        //まだテストしていないステージがあれば実行する
		if (testStageIndex < testStagePrefabs.Length) {
			StartTest ();
		} else {
			Debug.Log ("TEST COMPLETE");
			//EditorApplication.isPlaying = false;
		}
    }

	private void SaveResult(int _charaId, int[] _testCharaLife, List<float> _testArriveEnemySpawnTime, List<int> _testArriveEnemyId, List<int> _testArriveEnemyHP, List<float> _testDeadEnemySpawnTime, List<int> _testDeadEnemyId, List<float> _expectedUserMarginTime) {
        StreamWriter sw = new StreamWriter(Application.dataPath + "/Test/test_result.txt", notFirstTest);

		if(!notFirstTest){
			notFirstTest = true;
		}

		if (_charaId == 0) {
			sw.WriteLine ("=======================================" + stageName + " Test Info =======================================");
		}

		sw.WriteLine ("\r\nCharaID : " + _charaId.ToString () + " Result");
		sw.WriteLine (string.Format("Last Chara Life : {0}", _testCharaLife[_charaId]));
		sw.WriteLine ("Arrive Enemies");
		string arriveResult = "";

		if (_testArriveEnemyId.Count == 0) {
			arriveResult = "NONE\r\n";
		}

		for(int i = 0; i < _testArriveEnemyId.Count; i++){
			arriveResult += string.Format("EnemyID : {0} SpawnTime : {1:f3} Arrived HP : {2}{3}",_testArriveEnemyId[i],_testArriveEnemySpawnTime[i],_testArriveEnemyHP[i],"\r\n");
		}
		sw.Write (arriveResult);

		sw.WriteLine ("Dead Enemies");
		string deadResult = "";

		if (_testDeadEnemyId.Count == 0) {
			deadResult = "NONE\r\n";
		}
			
		for(int i = 0; i < _testDeadEnemyId.Count; i++){
			deadResult += string.Format("EnemyID : {0} SpawnTime : {1:f3} ExpectedUserMarginTime : {2:f3}{3}",_testDeadEnemyId[i],_testDeadEnemySpawnTime[i], _expectedUserMarginTime[i],"\r\n");
		}

		sw.Write (deadResult);

		if (_charaId == CHARACTER_DEFINE.characterVarietyNum - 1) {
			sw.WriteLine ("===============================================================================================\r\n");
		}
        sw.Flush();
        sw.Close();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        //敵情報の登録
		enemyId.Add(other.GetComponent<Enemy>().GetId());
        enemySpawnTime.Add(other.GetComponent<Enemy>().spawnTime - stageStartTime);
        enemyArriveTime.Add(spendGameTime);
		enemyHP.Add(ENEMY_DEFINE.HP[other.GetComponent<Enemy>().GetId()]);
		enemyDamage.Add(ENEMY_DEFINE.DAMAGE[other.GetComponent<Enemy>().GetId()]);

        Destroy(other.gameObject);
		/*if(enemyId.Count == this.allEnemyNum) {
            isSpawn = false;
            gameStatus = GameStatus.END;
            Destroy(testStageSpawner.gameObject);
            TestChara();
        }*/
    }
}
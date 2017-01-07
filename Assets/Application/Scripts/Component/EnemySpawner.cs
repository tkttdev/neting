using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemySpawnerManager : MonoBehaviour {

    
    [SerializeField] private GameObject[] spawnerPos;
    [SerializeField] private TextAsset spawnerInfoText;
    private SpawnerInfo spawnerInfo = new SpawnerInfo();

    private float playTime = 0.0f;
    

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
            spawnerInfo.spawnerPos.Add(int.Parse(values[2]));
        }
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.I.CheckGameStatus(GameStatus.PLAY)) {
            playTime += Time.deltaTime;
            CheckSpawn();
        }
    }

    private void CheckSpawn() {
        if(playTime > spawnerInfo.spawnTime[0]) {
            spawnerInfo.spawnTime.RemoveAt(0);
        }
    }
}

public class SpawnerInfo {
    public List<int> enemyId = new List<int>();
    public List<float> spawnTime = new List<float>();
    public List<int> spawnerPos = new List<int>();
}
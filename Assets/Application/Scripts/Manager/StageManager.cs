using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;

public class StageManager : SingletonBehaviour<StageManager> {

	#region PublicField
	public Corner[] enemySpawnCorner;
	public Corner[] bulletSpawnCorner = new Corner[5];
	public static readonly float[] regularSpawnPosX = new float[5];
	#endregion

	#region PrivateField
	private int destroyEnemyNum = 0;
	private int allEnemyNum = 0;
	#endregion

	// Use this for initialization
	protected override void Initialize () {
	}

	public void StartNextWave(){
		/*waveNum++;
		destroyEnemyNumInWave = 0;
		UIManager.I.WaveStart (waveNum+1, maxWaveNum);
		Debug.Log ("Start wave " + waveNum.ToString ());
		Debug.Log ("ALL ENEMY NUM " + enemySpawnInfo.allWaveEnemyNum [waveNum]);*/
	}

	public void SetAllEnemyNum(int _num){
		allEnemyNum = _num;
		Debug.Log (allEnemyNum);
	}

	public void AddAllEnemyNum(int _num){
		allEnemyNum += _num;
	}

	public void AddDestoryEnemyNum(int _num){
		destroyEnemyNum += _num;
		Debug.Log (destroyEnemyNum);
		if (destroyEnemyNum == allEnemyNum) {
			GameManager.I.SetStatuEnd ();
		}
	}
}
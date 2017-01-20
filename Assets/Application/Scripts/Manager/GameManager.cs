using UnityEngine;
using System.Collections;
using System;

public enum GameStatus : int {
    WAIT = 0,
    PLAY = 1,
    PAUSE = 2,
    END = 3,
}

public class GameManager : SingletonBehaviour<GameManager>, IRecieveMessage {

	[SerializeField] protected bool isDemo = false;
    protected GameStatus gameStatus = GameStatus.WAIT;
	public int allDeadEnemyNum;
	public int allEnemyNum;

	protected override void Initialize (){
		base.Initialize ();
		#if UNITY_EDITOR
		if (GameObject.Find("Systems") == null) {
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif

		if (!isDemo) {
			GameObject stage = Resources.Load ("Prefabs/Stage/Stage" + StageLevelManager.I.GetStageLevel ().ToString ()) as GameObject;
			Instantiate (stage);
		}

		if (StageLevelManager.I.GetStageLevel () < 8) {
			SoundManager.I.SoundBGM (BGM.STAGE_EASY);
		} else if (StageLevelManager.I.GetStageLevel () < 15) {
			SoundManager.I.SoundBGM (BGM.STAGE_NORMAL);
		} else {
			SoundManager.I.SoundBGM (BGM.STAGE_HARD);
		}

		UIManager.I.CountStart (3);
	}

    public void SetStatusWait() {
        gameStatus = GameStatus.WAIT;
    }

	public void SetStatuPlay() {
        gameStatus = GameStatus.PLAY;
    }

	public void SetStatuPause() {
        gameStatus = GameStatus.PAUSE;
    }

	public void SetStatuEnd() {
		if (isDemo) {
			return;
		}
        gameStatus = GameStatus.END;
		//AdsManager.I.EnableWatchAds ();
		UserDataManager.I.AddMoney (GetItemManager.I.GetEarnMoney ());
		UserDataManager.I.SaveData ();
		if (GameCharacter.I.GetLife() <= 0) {
			UIManager.I.gameOverDialog.Show ();
		} else {
			UIManager.I.stageClearDialog.Show ();
		}
    }

    public bool CheckGameStatus(GameStatus _checkStatus) {
        return (_checkStatus == gameStatus);
    }

	public void OnRecieveInfo(int _allEnemyNum){
		allEnemyNum = _allEnemyNum;
	}

	public void OnRecieveInfo(){
		allDeadEnemyNum++;
		if (allDeadEnemyNum == allEnemyNum) {
			SetStatuEnd ();
		}
	}

}

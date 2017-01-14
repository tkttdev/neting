using UnityEngine;
using System.Collections;
using System;

public enum GameStatus : int {
    WAIT = 0,
    PLAY = 1,
    PAUSE = 2,
    END = 3,
}

public class GameManager : SingletonBehaviour<GameManager> {

	[SerializeField] protected bool isDemo = false;
    protected GameStatus gameStatus = GameStatus.WAIT;

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
        gameStatus = GameStatus.PLAY;
	}

    public void SetWait() {
        gameStatus = GameStatus.WAIT;
    }

    public void SetPlay() {
        gameStatus = GameStatus.PLAY;
    }

    public void SetPause() {
        gameStatus = GameStatus.PAUSE;
    }

    public void SetEnd() {
        gameStatus = GameStatus.END;
    }

    public bool CheckGameStatus(GameStatus _checkStatus) {
        return (_checkStatus == gameStatus);
    }
}

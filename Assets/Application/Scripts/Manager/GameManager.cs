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

    GameStatus gameStatus = GameStatus.WAIT;

	protected override void Initialize (){
		base.Initialize ();
        gameStatus = GameStatus.WAIT;
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

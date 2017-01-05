using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public enum GameSceneType : int {
    TITLE_SCENE = 0,
    MENU_SCENE = 1,
    CHARACTER_SELECT_SCENE = 2,
    STORE_SCENE = 3,
    GAME_SCENE = 4,
    CREDIT_SCENE = 5,
}

public class AppSceneManager : SingletonBehaviour<AppSceneManager> {

	protected override void Initialize (){
		base.Initialize ();
		DontDestroyOnLoad (this);
	}

	public void GoScene(GameSceneType _gameSceneType = GameSceneType.TITLE_SCENE) {
        SceneManager.LoadScene((int)_gameSceneType);
    }
}

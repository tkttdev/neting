using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class AppSceneManager : SingletonBehaviour<AppSceneManager> {

	private enum GameSceneType : int{
		TITLE_SCENE = 0,
		STAGE_SELECT_SCENE = 1,
		CHARACTER_SELECT_SCENE = 2,
		GAME_SCENE = 3,
		CREDIT_SCENE = 4,
	}

	protected override void Initialize (){
		base.Initialize ();
		DontDestroyOnLoad (this);
	}

	public void GoTitle(){
		FreeMemory ();
		SceneManager.LoadScene ((int)GameSceneType.TITLE_SCENE);
	}

	public void GoStageSelect(){
		FreeMemory ();
		SceneManager.LoadScene ((int)GameSceneType.STAGE_SELECT_SCENE);
	}

	public void GoCharacterSelect(){
		FreeMemory ();
		SceneManager.LoadScene ((int)GameSceneType.CHARACTER_SELECT_SCENE);
	}

	public void GoGame(){
		FreeMemory ();
		SceneManager.LoadScene ((int)GameSceneType.GAME_SCENE);
	}

	public void GoCredit(){
		FreeMemory ();
		SceneManager.LoadScene ((int)GameSceneType.CREDIT_SCENE);
	}

	private void FreeMemory(){
		Resources.UnloadUnusedAssets ();
		GC.Collect ();
	}
}

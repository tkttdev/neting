using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum GameSceneType : int {
    TITLE_SCENE = 0,
    MENU_SCENE = 1,
    CHARACTER_STORE = 2,
    GAME_SCENE = 3,
    CREDIT_SCENE = 4,
}

public class AppSceneManager : SingletonBehaviour<AppSceneManager> {

	[SerializeField] private SpriteRenderer fadeSprite;
	private EventSystem eventSystem;

	protected override void Initialize (){
		base.Initialize ();
		DontDestroyOnLoad (this);
	}

	public void GoScene(GameSceneType _gameSceneType = GameSceneType.TITLE_SCENE) {
		SceneManager.LoadScene ((int)_gameSceneType);
		//StartCoroutine (FadeGoScene ((int)_gameSceneType));
    }

	private IEnumerator FadeGoScene(int _gameSceneType){
		if (eventSystem != null) {
			eventSystem.enabled = false;
		}
		while (fadeSprite.color.a < 1.0f) {
			fadeSprite.color = new Color (fadeSprite.color.r, fadeSprite.color.g, fadeSprite.color.b, Mathf.Clamp (fadeSprite.color.a + 0.05f, 0.0f, 1.0f));
			yield return new WaitForSeconds (0.02f);
		}
		SceneManager.LoadScene(_gameSceneType);
		yield break;
	}

	private void OnLevelWasLoaded( int level ){
		//SceneManager.sceneLoaded ();
		eventSystem = GameObject.FindObjectOfType<EventSystem> ();
		fadeSprite.color = new Color (fadeSprite.color.r, fadeSprite.color.g, fadeSprite.color.b, 0.0f);
	}
}

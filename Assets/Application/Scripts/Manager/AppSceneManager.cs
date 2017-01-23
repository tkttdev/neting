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

	[SerializeField] private Image fadeSprite;
	[SerializeField] private Canvas canvas;
	private EventSystem eventSystem;

	public bool isFade = false;

	protected override void Initialize (){
		base.Initialize ();
		DontDestroyOnLoad (this);
		SceneManager.sceneLoaded += FindEventSystem;
	}

	public void GoScene(GameSceneType _gameSceneType = GameSceneType.TITLE_SCENE) {
		if (isFade) {
			return;
		}
		StartCoroutine (FadeOutGoScene ((int)_gameSceneType));
    }

	private IEnumerator FadeOutGoScene(int _gameSceneType){
		isFade = true;
		if (eventSystem != null) {
			eventSystem.enabled = false;
		}
		fadeSprite.enabled = true;
		while (fadeSprite.color.a < 1.0f) {
			fadeSprite.color = new Color (fadeSprite.color.r, fadeSprite.color.g, fadeSprite.color.b, Mathf.Clamp (fadeSprite.color.a + 0.1f, 0.0f, 1.0f));
			yield return new WaitForSeconds (0.02f);
		}
		SceneManager.LoadScene(_gameSceneType);
		yield break;
	}

	private IEnumerator FadeInScene(){
		if (canvas == null) {
			yield break;
		}
		fadeSprite.enabled = true;
		while (fadeSprite.color.a > 0.0f) {
			fadeSprite.color = new Color (fadeSprite.color.r, fadeSprite.color.g, fadeSprite.color.b, Mathf.Clamp (fadeSprite.color.a - 0.1f, 0.0f, 1.0f));
			yield return new WaitForSeconds (0.02f);
		}
		if (eventSystem != null) {
			eventSystem.enabled = true;
		}
		fadeSprite.enabled = false;
		isFade = false;
		yield break;
	}

	private void FindEventSystem(Scene scenename,LoadSceneMode SceneMode){
		if (I != null && this == I) {
			eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
			canvas.worldCamera = Camera.main;
			StartCoroutine (FadeInScene ());
		}
	}
}

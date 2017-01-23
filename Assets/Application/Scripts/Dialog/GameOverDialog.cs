using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameOverDialog : DialogBase {

	[SerializeField]private GameObject returnButton;
	[SerializeField]private GameObject titleButton;
	[SerializeField]private GameObject panel;

	// Use this for initialization
	protected override void Start() {
		base.Start();
		Hide();
	}

	// Update is called once per frame
	void Update() {
		
	}

	public override void Show() {
		base.Show();
		SetComponentsActive();
	}

	public override void Hide() {
		base.Hide();
		SetComponentsInactive();
	}

	private void SetComponentsInactive() {
		returnButton.SetActive(false);
		titleButton.SetActive(false);
		panel.SetActive(false);
	}

	private void SetComponentsActive() {
		returnButton.SetActive(true);
		titleButton.SetActive(true);
		panel.SetActive(true);
	}

	public void RetryButton() {
		UIManager.I.gameOverDialog.Hide ();
		SoundManager.I.SoundSE (SE.BUTTON2);
		AppSceneManager.I.GoScene(GameSceneType.GAME_SCENE);
	}

	public void MenuButton() {
		UIManager.I.gameOverDialog.Hide ();
		SoundManager.I.SoundSE (SE.BUTTON2);
		AppSceneManager.I.GoScene(GameSceneType.MENU_SCENE);
	}
}
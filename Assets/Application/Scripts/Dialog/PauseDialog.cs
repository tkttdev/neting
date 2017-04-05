using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PauseDialog : DialogBase {

	[SerializeField] private GameObject restartButton;
	[SerializeField] private GameObject exitButton;
	[SerializeField] private GameObject panel;
	[SerializeField] private Text pauseText;

	protected override void Start() {
		base.Start();
		Hide ();
	}

	public override void Show() {
		base.Show();
		pauseText.text = string.Format ("Stage {0}\nPause", StageLevelManager.I.GetStageLevel ());
		SetComponentsActive();
	}

	public override void Hide() {
		base.Hide();
		SetComponentsInactive();
	}

	private void SetComponentsInactive() {
		exitButton.SetActive (false);
		restartButton.SetActive (false);
		panel.SetActive (false);
	}

	private void SetComponentsActive() {
		exitButton.SetActive (true);
		restartButton.SetActive (true);
		panel.SetActive (true);
	}

	public void RestartButton(){
		AppSceneManager.I.GoScene (GameSceneType.GAME_SCENE);
	}

	public void ExitButton(){
		AppSceneManager.I.GoScene (GameSceneType.MENU_SCENE);
	}
}
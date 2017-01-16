using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class StageClearDialog : DialogBase {

	[SerializeField]private GameObject nextButton;
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
		nextButton.SetActive(false);
		titleButton.SetActive(false);
		panel.SetActive(false);
	}

	private void SetComponentsActive() {
		nextButton.SetActive(true);
		titleButton.SetActive(true);
		panel.SetActive(true);
	}

	public void NextButton() {
		StageLevelManager.I.SetStageLevel(StageLevelManager.I.GetStageLevel()+1);
		AppSceneManager.I.GoScene(GameSceneType.GAME_SCENE);
	}

	public void TitleButton() {
		AppSceneManager.I.GoScene(GameSceneType.MENU_SCENE);
	}
}
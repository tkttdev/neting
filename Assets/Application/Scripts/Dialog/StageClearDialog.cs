using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class StageClearDialog : DialogBase {

	[SerializeField]private GameObject nextButton;
	[SerializeField]private GameObject titleButton;
	[SerializeField]private GameObject panel;
	[SerializeField]private Text coinText;
	[SerializeField]private Text stageClearText;
	[SerializeField]private Text rankText;

	// Use this for initialization
	protected override void Start() {
		base.Start();
		Hide ();
	}

	public override void Show() {
		base.Show();
		StartCoroutine (ShowAnimation ());
		coinText.text = GetItemManager.I.GetEarnMoney ().ToString ();
		switch (GameCharacter.I.GetLife ()) {
		case 3:
			rankText.text = "RANK : S";
			break;
		case 2:
			rankText.text = "RANK : A";
			break;
		case 1:
			rankText.text = "RANK : B";
			break;
		}
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
		titleButton.SetActive (true);
		panel.SetActive (true);
		if (StageLevelManager.I.GetStageLevel () != 20) {
			nextButton.SetActive (true);
		} else {
			titleButton.transform.localPosition = new Vector3 (0, titleButton.transform.localPosition.y, titleButton.transform.localPosition.z);
		}
	}

	public void NextButton() {
		UIManager.I.stageClearDialog.Hide ();
		StageLevelManager.I.SetStageLevel(StageLevelManager.I.GetStageLevel()+1);
		SoundManager.I.SoundSE (SE.BUTTON2);
		AppSceneManager.I.GoScene(GameSceneType.GAME_SCENE);
	}

	public void MenuButton() {
		UIManager.I.stageClearDialog.Hide ();
		SoundManager.I.SoundSE (SE.BUTTON2);
		AppSceneManager.I.GoScene(GameSceneType.MENU_SCENE);
	}

	private IEnumerator ShowAnimation(){
		iTween.ScaleTo (stageClearText.gameObject, iTween.Hash ("x", 0.8f, "y", 0.8f, "time", 0.5f));
		yield return new WaitForSeconds (0.3f);
		iTween.ScaleTo (panel, iTween.Hash ("y", 1.0f, "time", 0.3f));
		yield break;
	}
}
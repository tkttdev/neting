using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameOverDialog : DialogBase {

	[SerializeField]private GameObject returnButton;
	[SerializeField]private GameObject titleButton;
	[SerializeField]private GameObject panel;
	[SerializeField]private Text coinText;
	[SerializeField]private Text gameOverText;

	// Use this for initialization
	protected override void Start() {
		base.Start();
		Hide();
	}

	public override void Show() {
		base.Show();
		StartCoroutine (ShowAnimation ());
		coinText.text = GetItemManager.I.GetEarnMoney ().ToString ();
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
		GameUIManager.I.gameOverDialog.Hide ();
		SoundManager.I.SoundSE (SE.BUTTON2);
		AppSceneManager.I.GoScene(GameSceneType.GAME_SCENE);
	}

	public void MenuButton() {
		GameUIManager.I.gameOverDialog.Hide ();
		SoundManager.I.SoundSE (SE.BUTTON2);
		AppSceneManager.I.GoScene(GameSceneType.MENU_SCENE);
	}

	private IEnumerator ShowAnimation(){
		StartCoroutine (AlphaAnimation ());
		yield return new WaitForSeconds (0.3f);
		iTween.ScaleTo (panel, iTween.Hash ("y", 1.0f, "time", 0.3f));
		yield break;
	}

	private IEnumerator AlphaAnimation(){
		float a = 0.0f;
		while (gameOverText.color.a < 1.0f) {
			a += Mathf.Clamp (a + 0.05f, 0.0f, 1.0f);
			gameOverText.color = new Color (gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, a);
			yield return new WaitForSeconds (0.1f);
		}
		yield break;
	}
}
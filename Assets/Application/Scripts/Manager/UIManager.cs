using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager> {

	public DialogBase gameOverDialog;
	public DialogBase pauseDialog;
	public DialogBase stageClearDialog;

	[SerializeField] private GameObject countPanel;
	[SerializeField] private Text countText;
	[SerializeField] private Text waveText;


	public void CountStart(int _countTime){
		StartCoroutine (WaveStartCorutine ());
		///StartCoroutine (CountCorutine (_countTime));
	}

	private IEnumerator CountCorutine(int _countTime){
		int limitTime = _countTime;
		countPanel.SetActive (true);
		for (int i = 0; i < limitTime; i++) {
			countText.text = _countTime.ToString ();
			_countTime--;
			yield return new WaitForSeconds (1.0f);
		}
		countText.text = "<size=120>GO!</size>";
		yield return new WaitForSeconds (0.7f);
		countPanel.SetActive (false);
		StageManager.I.StartNextWave ();
		GameManager.I.SetStatuPlay ();
		yield break;
	}

	public void WaveStartCount(){
		StartCoroutine (WaveStartCorutine ());
	}

	private IEnumerator WaveStartCorutine(){
		waveText.enabled = true;
		yield return new WaitForSeconds (1.0f);
		waveText.enabled = false;
		StageManager.I.StartNextWave ();
		if (!GameManager.I.CheckGameStatus (GameStatus.PLAY)) {
			GameManager.I.SetStatuPlay ();
		}
		yield break;
	}
}

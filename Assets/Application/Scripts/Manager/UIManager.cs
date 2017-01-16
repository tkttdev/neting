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


	public void CountStart(int _countTime){
		StartCoroutine (CountCorutine (_countTime));
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
		GameManager.I.SetPlay ();
		yield break;
	}
}

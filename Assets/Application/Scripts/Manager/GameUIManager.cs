using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : SingletonBehaviour<GameUIManager> {

	public DialogBase gameOverDialog;
	public DialogBase pauseDialog;
	public DialogBase stageClearDialog;
	public WavePanel wavePanel;
	
	[SerializeField] private Text bulletNum;
	[SerializeField] private GameObject batteryGage;
	[SerializeField] private GameObject lifeGauge1;
	[SerializeField] private GameObject lifeGauge2;
	[SerializeField] private GameObject damageGauge;
	[SerializeField] private GameObject pauseButton;
	[SerializeField] private GameObject resumeButton;

	protected override void Initialize (){
		base.Initialize ();
		int useCharaIndex = UserDataManager.I.GetUseCharacterIndex();
		int useCharaLv = UserDataManager.I.GetCharacterLevel(useCharaIndex);
		string charaName = CHARACTER_DEFINE.NAME[useCharaIndex];
		CharacterStatusManager.I.ParseStatusInfoText(charaName);

		float maxLife = CharacterStatusManager.I.GetCharacterHealth(useCharaLv);
		if (maxLife <= 5) {
			damageGauge.transform.localScale = new Vector3(25, maxLife % 6 * 80, 0);
		} else {
			damageGauge.transform.localScale = new Vector3(25, maxLife % 6 + 1 * 80, 0);
		}
    }

	private void Update(){
		if (GameManager.I.CheckGameStatus (GameStatus.PLAY)) {
			batteryGage.GetComponent<Image>().fillAmount = BattleShip.I.bulletRate * 0.75f;
		}
	}

	public void UpdateCharacterInfo(float _life,int _bulletStock){
		if (_life <= 5) {
			lifeGauge1.transform.localScale = new Vector3(25, _life * 80, 0);
			lifeGauge2.transform.localScale = new Vector3(25, 0, 0);
		} else {
			lifeGauge1.transform.localScale = new Vector3(25, 400, 0);
			lifeGauge2.transform.localScale = new Vector3(25, (_life - 5) * 80, 0);
		}

		bulletNum.text = _bulletStock.ToString();
	}

	public void PauseButton(){
		if (GameManager.I.CheckGameStatus (GameStatus.PLAY)) {
			GameManager.I.SetStatuPause ();
			pauseButton.SetActive (false);
			StartCoroutine (PauseAnimation ());
		}
	}

	public void ResumeButton(){
		if (GameManager.I.CheckGameStatus (GameStatus.PAUSE)) {
			resumeButton.SetActive (false);
			StartCoroutine (ResumeAnimation ());
		}
	}

	IEnumerator PauseAnimation(){
		while (Camera.main.orthographicSize > 1.0f) {
			Camera.main.orthographicSize -= 0.5f;
			yield return new WaitForSeconds (0.010f);
		}
		resumeButton.SetActive (true);
		pauseDialog.Show ();
	}

	IEnumerator ResumeAnimation(){
		pauseDialog.Hide ();
		while (Camera.main.orthographicSize < 5.0f) {
			Camera.main.orthographicSize += 0.5f;
			yield return new WaitForSeconds (0.010f);
		}
		pauseButton.SetActive (true);
		GameManager.I.SetStatuPlay ();
	}
}

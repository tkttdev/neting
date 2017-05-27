using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager> {

	public DialogBase gameOverDialog;
	public DialogBase pauseDialog;
	public DialogBase stageClearDialog;

	[SerializeField] private GameObject countPanel;
	[SerializeField] private Text waveText;
	[SerializeField] private GameObject batteryGage;

	[SerializeField] private GameObject[] life;
	[SerializeField] private Image[] bullet;
	[SerializeField] private Image characterFace;

	[SerializeField] private Sprite chargeBulletSprite;
	[SerializeField] private Sprite emptyBulletSprite;

	[SerializeField] private WavePanel wavePanel;
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject pauseButton;
	[SerializeField] private GameObject resumeButton;

	protected override void Initialize (){
		base.Initialize ();
		characterFace.GetComponent<Image> ().sprite = Resources.Load<Sprite>(CHARACTER_DEFINE.FACE_IMAGE_RESOURCES_PATH[UserDataManager.I.GetUseCharacterIndex()]);
		for (int i = CHARACTER_DEFINE.MAX_BULLET_STOCK [UserDataManager.I.GetUseCharacterIndex ()]; i < 5; i++) {
			bullet [i].enabled = false;
		}
	}

	void Update(){
		if (GameManager.I.CheckGameStatus (GameStatus.PLAY)) {
			batteryGage.transform.localScale = new Vector3 (BattleShip.I.bulletRate, 1, 1);
		}
	}

	public void UpdateCharacterInfo(int _life,int _bulletStock){
		for (int i = 0; i < life.Length; i++) {
			life [i].SetActive (false);
		}
		if (_life > 0) {
			life [_life - 1].SetActive (true);
		}
		for (int i = 0; i < bullet.Length; i++) {
			if (i < _bulletStock) {
				bullet [i].GetComponent<Image> ().sprite = chargeBulletSprite;
			} else {
				bullet [i].GetComponent<Image> ().sprite = emptyBulletSprite;
			}
		}
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

	/*public void WaveStart(int _wave, int _maxWave){
		StartCoroutine (WaveStartCorutine (_wave, _maxWave));
	}*/

	/*private IEnumerator WaveStartCorutine(int _wave, int _maxWave){
		wavePanel.gameObject.SetActive (true);
		wavePanel.Show (_wave,_maxWave);
		yield return new WaitForSeconds (2.3f);
		wavePanel.Hide ();
		StageManager.I.StartSpawn ();
		if (!GameManager.I.CheckGameStatus (GameStatus.PLAY)) {
			GameManager.I.SetStatuPlay ();
		}
		yield break;
	}*/
}

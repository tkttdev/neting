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
	[SerializeField] private GameObject[] bullet;
	[SerializeField] private Image characterFace;

	protected override void Initialize (){
		base.Initialize ();
		Debug.Log (CHARACTER_DEFINE.FACE_IMAGE_RESOURCES_PATH [UserDataManager.I.GetUseCharacterIndex ()]);
		characterFace.GetComponent<Image> ().sprite = Resources.Load<Sprite>(CHARACTER_DEFINE.FACE_IMAGE_RESOURCES_PATH[UserDataManager.I.GetUseCharacterIndex()]);
	}

	void Update(){
		if (GameManager.I.CheckGameStatus (GameStatus.PLAY)) {
			batteryGage.transform.localScale = new Vector3 (GameCharacter.I.bulletRate, 1, 1);
		}
	}

	public void UpdateCharacterInfo(int _life,int _bulletStock){
		for (int i = 0; i < life.Length; i++) {
			life [i].SetActive (false);
		}
		if (_life != 0) {
			life [_life - 1].SetActive (true);
		}
		for (int i = 0; i < bullet.Length; i++) {
			if (i < _bulletStock) {
				bullet [i].SetActive (true);
			} else {
				bullet [i].SetActive (false);
			}
		}
	}

	/*public void CountStart(int _countTime){
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
	}*/

	public void WaveStart(int _wave, int _maxWave){
		StartCoroutine (WaveStartCorutine (_wave, _maxWave));
	}

	private IEnumerator WaveStartCorutine(int _wave, int _maxWave){
		waveText.enabled = true;
		waveText.text = string.Format ("WAVE{0}/{1}\nSTART", _wave, _maxWave);
		yield return new WaitForSeconds (1.0f);
		waveText.enabled = false;
		StageManager.I.StartSpawn ();
		if (!GameManager.I.CheckGameStatus (GameStatus.PLAY)) {
			GameManager.I.SetStatuPlay ();
		}
		yield break;
	}
}

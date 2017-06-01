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

	[SerializeField] private GameObject lifeGauge;
	[SerializeField] private GameObject damageGauge;
	[SerializeField] private Image[] bullet;
	[SerializeField] private Image characterFace;

	[SerializeField] private Sprite chargeBulletSprite;
	[SerializeField] private Sprite emptyBulletSprite;

	[SerializeField] private WavePanel wavePanel;
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject pauseButton;
	[SerializeField] private GameObject resumeButton;

	[HideInInspector] public int useCharaIndex;
	[HideInInspector] public int useCharaLv;

	protected override void Initialize (){
		base.Initialize ();
		useCharaIndex = UserDataManager.I.GetUseCharacterIndex();
		useCharaLv = UserDataManager.I.GetCharacterLevel(useCharaIndex);
		string charaName = CHARACTER_DEFINE.NAME[useCharaIndex];
		CharacterStatusManager.I.ParseStatusInfoText(charaName);

		characterFace.GetComponent<Image> ().sprite = Resources.Load<Sprite>(CHARACTER_DEFINE.FACE_IMAGE_RESOURCES_PATH[useCharaIndex]);
		damageGauge.transform.localScale = new Vector3(25, CharacterStatusManager.I.GetCharacterHealth(useCharaLv) * 40, 0);
        for (int i = CharacterStatusManager.I.GetCharacterBulletNum(useCharaLv); i < bullet.Length; i++) {
			bullet [i].enabled = false;
		}
	}

	void Update(){
		if (GameManager.I.CheckGameStatus (GameStatus.PLAY)) {
			batteryGage.transform.localScale = new Vector3 (BattleShip.I.bulletRate, 1, 1);
		}
	}

	public void UpdateCharacterInfo(float _life,int _bulletStock){
		lifeGauge.transform.localScale = new Vector3(25, _life * 40, 0);

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
}

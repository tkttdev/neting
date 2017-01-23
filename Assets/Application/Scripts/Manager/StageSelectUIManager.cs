using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUIManager : SingletonBehaviour<StageSelectUIManager> {

	[SerializeField] private Text moneyText;
	[SerializeField] private GameObject[] buttonRoot;
	[SerializeField] private GameObject[] stageButtonRoot;
	[SerializeField] private Image[] stageButtonImage;

	[SerializeField] private GameObject[] coverPanel;
	[SerializeField] private Image characterImage;

	[SerializeField] private GameObject getMoneyDialog;

	//0~10 => 0, 11~20 => 1
	private int showStageIndex = 0;
	private int minStageIndex = 0;
	private int maxStageIndex = 1;
	private int purposeStageIndex = 0;

	private float unitX;

	protected override void Initialize (){
		base.Initialize ();
		#if UNITY_EDITOR
		if(GameObject.Find("Systems") == null){
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
		unitX = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0, 0)).x * 2.0f;
		moneyText.text = string.Format ("{0}", UserDataManager.I.GetMoney ().ToString ());
		characterImage.sprite = Resources.Load<Sprite> (CHARACTER_DEFINE.IMAGE_RESOURCES_PATH [UserDataManager.I.GetUseCharacterIndex ()]);
		for (int i = 0; i < stageButtonImage.Length; i++) {
			if (UserDataManager.I.IsClearStage (i)) {
				stageButtonImage [i].sprite = Resources.Load<Sprite> ("Images/Circle/stageicon2");
			}
		}
	}

	void Update(){
		moneyText.text = string.Format ("{0}", UserDataManager.I.GetMoney ().ToString ());
	}

	public void ShowGetMoneyDialog(){
		for (int i = 0; i < coverPanel.Length; i++) {
			coverPanel [i].SetActive (true);
		}
		getMoneyDialog.SetActive (true);
	}

	public void HideGetMoneyDialog(){
		for (int i = 0; i < coverPanel.Length; i++) {
			coverPanel [i].SetActive (false);
		}
		getMoneyDialog.SetActive (false);
	}

	/// <summary>
	/// left : dir => 1, right : dir => -1
	/// </summary>
	/// <param name="_dir">Dir.</param>
	public void MoveStageButton(int _dir){
		SoundManager.I.SoundSE (SE.BUTTON1);
		StartCoroutine (ButtonMoveAnimation (_dir));
	}

	public void InactiveNotDialogComponent(){
		for (int i = 0; i < coverPanel.Length; i++) {
			coverPanel [i].SetActive (true);
		}
	}

	private IEnumerator ButtonMoveAnimation(int _dir){
		for (int i = 0; i < coverPanel.Length; i++) {
			coverPanel [i].SetActive (true);
		}
		for (int i = 0; i < buttonRoot.Length; i++) {
			iTween.ScaleTo (buttonRoot [i].gameObject, iTween.Hash ("x", 0.7f, "y", 0.7f, "time", 0.25f));
		}
		yield return new WaitForSeconds(0.2f);
		for (int i = 0; i < stageButtonRoot.Length; i++) {
			iTween.MoveBy (stageButtonRoot [i].gameObject, iTween.Hash ("x", _dir * unitX, "time", 0.65f));
		}
		yield return new WaitForSeconds (0.5f);
		for (int i = 0; i < buttonRoot.Length; i++) {
			iTween.ScaleTo (buttonRoot [i].gameObject, iTween.Hash ("x", 1.0f, "y", 1.0f, "time", 0.25f));
		}
		yield return new WaitForSeconds (0.25f);
		for (int i = 0; i < coverPanel.Length; i++) {
			coverPanel [i].SetActive (false);
		}
		yield break;
	}
}

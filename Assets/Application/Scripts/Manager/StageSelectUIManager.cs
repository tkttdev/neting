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
	[SerializeField] private GameObject moveCharacterImage;

	[SerializeField] private GameObject getMoneyDialog;

	protected override void Initialize() {
		base.Initialize();
		#if UNITY_EDITOR
		if (GameObject.Find("Systems") == null) {
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
		moneyText.text = string.Format("{0}", UserDataManager.I.GetMoney().ToString());
		characterImage.sprite = Resources.Load<Sprite>(CHARACTER_DEFINE.IMAGE_RESOURCES_PATH[UserDataManager.I.GetUseCharacterIndex()]);
		moveCharacterImage.GetComponent<SpriteRenderer>().sprite = characterImage.sprite;
		for (int i = 0; i < stageButtonImage.Length; i++) {
			if (UserDataManager.I.IsClearStage(i)) {
				stageButtonImage[i].sprite = Resources.Load<Sprite>("Images/Line2");
			}
		}
		gameObject.transform.localPosition = new Vector3(0, 0, 0);
	}

	void Update() {
		moneyText.text = string.Format("{0}", UserDataManager.I.GetMoney().ToString());
	}

	public void ShowGetMoneyDialog() {
		for (int i = 0; i < coverPanel.Length; i++) {
			coverPanel[i].SetActive(true);
		}
		getMoneyDialog.SetActive(true);
	}
}

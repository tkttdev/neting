﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterStoreController : SingletonBehaviour<CharacterStoreController> {

	[SerializeField] private Image[] characterBackground;
	[SerializeField] private GameObject[] characterKey;

	[SerializeField] private GameObject statusPanel;
	[SerializeField] private Sprite[] characterStatus;
	[SerializeField] private Image showStatusImage;

	[SerializeField] private GameObject purchasePanel;
	[SerializeField] private Image showPurchaseStatusImage;
	[SerializeField] private Text purchaseMoneyText;
	[SerializeField] private Button purchaseButton;

	[SerializeField] private GameObject purchaseInfoPanel;
	[SerializeField] private Text purchaseInfoText;
	[SerializeField] private Image purchaseInfoCharacterImage;

	[SerializeField] private Text moneyText;


	private int willPurchaseCharaId = 0;
	//new
	private float posx = 0;
	private int tposx = 0;
	[SerializeField] private GameObject CharacterLocation;
	[SerializeField] private Text purchaseButtonMoneyText;
	[SerializeField] private Button truePurchaseButton;
	[SerializeField] private Text levelText;
	[SerializeField] private Text purchaseButtonText;
	private int PurchaseButtonId;


	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		if(GameObject.Find("Systems") == null){
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif

		SoundManager.I.SoundBGM (BGM.CHARACTER_STORE_BGM);
		DesignCharacterButton ();
		moneyText.text = UserDataManager.I.GetMoney ().ToString ();
		//
		//truePurchaseButton.interactable = false;
		levelText.text = "Lv:" + UserDataManager.I.GetCharacterLevel (0).ToString();
	}

	private void DesignCharacterButton(){
		for (int i = 0; i < CHARACTER_DEFINE.characterVarietyNum; i++) {
			if (UserDataManager.I.IsPermitUseCharacter (i)) {
				characterKey [i].SetActive (false);
			}

			if (i == UserDataManager.I.GetUseCharacterIndex ()) {
				characterBackground [i].color = new Color (60f / 255f, 0, 255f / 255f);
			} else if (UserDataManager.I.IsPermitUseCharacter (i)) {
				characterBackground [i].color = Color.white;
			} else {
				characterBackground [i].color = new Color (70.0f / 255.0f, 70.0f / 255.0f, 70.0f / 255.0f);
			}
		}
	}

	public void CharacterSelectButton(int _charaId) {
		if (UserDataManager.I.IsPermitUseCharacter (_charaId)) {
			SoundManager.I.SoundSE (SE.BUTTON2);
			characterBackground [UserDataManager.I.GetUseCharacterIndex ()].color = Color.white;
			UserDataManager.I.SetUseCharacterIndex (_charaId);	
			characterBackground [UserDataManager.I.GetUseCharacterIndex ()].color = Color.red;
		} else {
			SoundManager.I.SoundSE (SE.BUTTON2);
			ShowPurchasePanel (_charaId);
		}
		DesignCharacterButton ();
	}

	private void ShowPurchasePanel(int _charaId){
		if (UserDataManager.I.GetMoney () < CHARACTER_DEFINE.MONEY [_charaId]) {
			purchaseButton.interactable = false;
		} else {
			purchaseButton.interactable = true;
			willPurchaseCharaId = _charaId;
		}
		showPurchaseStatusImage.sprite = characterStatus [_charaId];
		purchaseMoneyText.text = CHARACTER_DEFINE.MONEY [_charaId].ToString ();
		purchasePanel.SetActive (true);
	}

	public void ShowCharacterStatusPanel(int _charaId){
		showStatusImage.sprite = characterStatus [_charaId];
		statusPanel.SetActive (true);
	}

	public void HideCharacterStatusPanel(){
		statusPanel.SetActive (false);
	}

	public void PurchaseButton(){
		UserDataManager.I.ReduceMoney (CHARACTER_DEFINE.MONEY [willPurchaseCharaId]);
		SoundManager.I.SoundSE (SE.PURCHASE);
		moneyText.text = UserDataManager.I.GetMoney ().ToString ();
		UserDataManager.I.GetCharacter (willPurchaseCharaId);
		HidePurchasePanel ();
		ShowPurchaseInforPanel ();
		DesignCharacterButton ();
	}

	public void CancelButton(){
		SoundManager.I.SoundSE (SE.BUTTON2);
		HidePurchasePanel ();
	}

	private void HidePurchasePanel(){
		purchasePanel.SetActive (false);
	}

	public void ShowPurchaseInforPanel(){
		purchaseInfoCharacterImage.sprite = Resources.Load<Sprite> (CHARACTER_DEFINE.IMAGE_RESOURCES_PATH [willPurchaseCharaId]);
		purchaseInfoText.text = CHARACTER_DEFINE.NAME [willPurchaseCharaId];
		purchaseInfoPanel.SetActive (true);
	}

	public void OkButton(){
		SoundManager.I.SoundSE (SE.BUTTON2);
		HidePurchaseInfoPanel ();
	}

	private void HidePurchaseInfoPanel(){
		purchaseInfoPanel.SetActive (false);
	}

	public void ReturnButton() {
		SoundManager.I.SoundSE (SE.BUTTON2);
		AppSceneManager.I.GoScene(GameSceneType.MENU_SCENE);
	}

	public void DirButton(int _dir){
		//CharacterLocation.gameObject.transform.position -= new Vector3(100/10, 0, 0);
		if((_dir == -1 && PurchaseButtonId == 0)||(_dir == 1 && PurchaseButtonId == CHARACTER_DEFINE.characterVarietyNum - 1)) return;



		posx = CharacterLocation.transform.position.x - (100 / 10) * _dir;
		tposx = Mathf.RoundToInt (posx / 10);
		iTween.MoveTo (CharacterLocation, iTween.Hash ("x", tposx * 10, "time", 2));

		Debug.Log (tposx * 10);

		PurchaseButtonId = -tposx;

		if ((CHARACTER_DEFINE.MONEY [PurchaseButtonId] > UserDataManager.I.GetMoney ())/*|| (UserDataManager.I.IsPermitUseCharacter(PurchaseButtonId) == true)*/) {
			truePurchaseButton.interactable = false;
		} else {
			truePurchaseButton.interactable = true;
		}

		if (UserDataManager.I.IsPermitUseCharacter (PurchaseButtonId) == true) {
			purchaseButtonText.text = "Grade UP";

		} else {
			purchaseButtonText.text = "PURCHASE";
		}


		ShowLevelText();
		//levelText.text = "Lv:" + UserDataManager.I.GetCharacterLevel (PurchaseButtonId).ToString();
		Debug.Log ("CharacterLevel = " + UserDataManager.I.GetCharacterLevel(PurchaseButtonId).ToString());

		purchaseButtonMoneyText.text = CHARACTER_DEFINE.MONEY [PurchaseButtonId].ToString ();
		Debug.Log ("PurchaseButtonId ="+PurchaseButtonId);





	}

	public void ShowLevelText(){
		levelText.text = "Lv:" + UserDataManager.I.GetCharacterLevel (PurchaseButtonId).ToString();
	}

	public void ShowMoneyText(){
		moneyText.text = UserDataManager.I.GetMoney ().ToString ();
	}

	public void NewPurchaseButton(){
		

		UserDataManager.I.ReduceMoney (CHARACTER_DEFINE.MONEY [PurchaseButtonId]);
		SoundManager.I.SoundSE (SE.PURCHASE);

		if ((UserDataManager.I.IsPermitUseCharacter (PurchaseButtonId) == true) && (UserDataManager.I.GetMoney() >= 0 /*CHARACTER_DEFINE.MONEY[PurchaseButtonId]*/)) {
			UserDataManager.I.AddCharacterLevel (PurchaseButtonId);
			//moneyText.text = UserDataManager.I.GetMoney ().ToString ();
			ShowMoneyText();
			ShowLevelText ();
			HideTruePurchaseButton ();
		}


		UserDataManager.I.GetCharacter (PurchaseButtonId);
		moneyText.text = UserDataManager.I.GetMoney ().ToString ();

		UserDataManager.I.GetCharacter (PurchaseButtonId);
		Debug.Log ("You bought it");
		purchaseButtonText.text = "Grade UP";


		HideTruePurchaseButton ();

	}

	public void AddMoney(){
		UserDataManager.I.AddMoney (1000);
		ShowMoneyText();
	}

	public void LossCharacterButton(){
		UserDataManager.I.LossCharacter (PurchaseButtonId);
	}

	public void LevelUpButton(){
		UserDataManager.I.AddCharacterLevel (PurchaseButtonId);
		ShowLevelText ();
	}

	public void ResetLevel(){
		UserDataManager.I.ResetLevel (PurchaseButtonId);
		ShowLevelText ();
	}

	public void HideTruePurchaseButton(){
		if ((UserDataManager.I.GetMoney() < CHARACTER_DEFINE.MONEY [PurchaseButtonId])) {
			truePurchaseButton.interactable = false;
		}
	}
}

﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterStoreController : SingletonBehaviour<CharacterStoreController> {

	[SerializeField] private Image[] selectCharacterBackground;
	[SerializeField] private GameObject statusPanel;
	[SerializeField] private Sprite[] characterStatus;
	[SerializeField] private Image showStatusImage;

	[SerializeField] private GameObject purchasePanel;
	[SerializeField] private Image showPurchaseStatusImage;
	[SerializeField] private Button purchaseButton;

	[SerializeField] private GameObject purchaseInfoPanel;
	[SerializeField] private Text purchaseInfoText;

	private int willPurchaseCharaId = 0;

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		if(GameObject.Find("Systems") == null){
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
		selectCharacterBackground [UserDataManager.I.GetUseCharacterIndex ()].color = Color.red;
	}

	public void CharacterSelectButton(int _charaId) {
		if (UserDataManager.I.IsPermitUseCharacter (_charaId)) {
			selectCharacterBackground [UserDataManager.I.GetUseCharacterIndex ()].color = Color.white;
			UserDataManager.I.SetUseCharacterIndex (_charaId);	
			selectCharacterBackground [UserDataManager.I.GetUseCharacterIndex ()].color = Color.red;
		} else {
			ShowPurchasePanel (_charaId);
		}
	}

	private void ShowPurchasePanel(int _charaId){
		if (UserDataManager.I.GetMoney () < CHARACTER_DEFINE.MONEY [_charaId]) {
			purchaseButton.interactable = false;
		} else {
			purchaseButton.interactable = true;
			willPurchaseCharaId = _charaId;
		}
		showPurchaseStatusImage.sprite = characterStatus [_charaId];
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
		UserDataManager.I.GetCharacter (willPurchaseCharaId);
		HidePurchasePanel ();
		ShowPurchaseInforPanel ();
	}

	public void HidePurchasePanel(){
		purchasePanel.SetActive (false);
	}

	public void ShowPurchaseInforPanel(){
		purchaseInfoText.text = "<color=#FF0FF0>" + CHARACTER_DEFINE.NAME [willPurchaseCharaId] + "</color>\nを手に入れた";
		purchaseInfoPanel.SetActive (true);
	}

	public void HidePurchaseInfoPanel(){
		purchaseInfoPanel.SetActive (false);
	}

	public void ReturnButton() {
		AppSceneManager.I.GoScene(GameSceneType.MENU_SCENE);
	}
}

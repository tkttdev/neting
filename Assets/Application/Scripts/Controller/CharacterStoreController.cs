using UnityEngine;
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

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		if(GameObject.Find("Systems") == null){
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
		DesignCharacterButton ();
		moneyText.text = UserDataManager.I.GetMoney ().ToString ();
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
			characterBackground [UserDataManager.I.GetUseCharacterIndex ()].color = Color.white;
			UserDataManager.I.SetUseCharacterIndex (_charaId);	
			characterBackground [UserDataManager.I.GetUseCharacterIndex ()].color = Color.red;
		} else {
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
		moneyText.text = UserDataManager.I.GetMoney ().ToString ();
		UserDataManager.I.GetCharacter (willPurchaseCharaId);
		HidePurchasePanel ();
		ShowPurchaseInforPanel ();
		DesignCharacterButton ();
	}

	public void HidePurchasePanel(){
		purchasePanel.SetActive (false);
	}

	public void ShowPurchaseInforPanel(){
		purchaseInfoCharacterImage.sprite = Resources.Load<Sprite> (CHARACTER_DEFINE.IMAGE_RESOURCES_PATH [willPurchaseCharaId]);
		purchaseInfoText.text = CHARACTER_DEFINE.NAME [willPurchaseCharaId];
		purchaseInfoPanel.SetActive (true);
	}

	public void HidePurchaseInfoPanel(){
		purchaseInfoPanel.SetActive (false);
	}

	public void ReturnButton() {
		AppSceneManager.I.GoScene(GameSceneType.MENU_SCENE);
	}
}

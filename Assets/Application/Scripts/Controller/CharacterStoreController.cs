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
	//new
	private float posx = 0;
	private int tposx = 0;
	private int loop = 0;
	private bool ignoreLoopFlag1 = false;
	private bool ignoreLoopFlag2 = true;
	private bool dirFlag = true;
	private int PurchaseButtonId;
	private bool loopflag = true;
	[SerializeField] private GameObject TopCharacterLocation;
	[SerializeField] private GameObject CharacterLocation1;
	[SerializeField] private GameObject CharacterLocation2;
	[SerializeField] private Text purchaseButtonMoneyText;
	[SerializeField] private Button truePurchaseButton;
	[SerializeField] private Text levelText;
	[SerializeField] private Text purchaseButtonText;




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
		if (UserDataManager.I.GetMoney () < GetPrice ()) {
			truePurchaseButton.interactable = false;
		}
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
		//if((_dir == -1 && PurchaseButtonId == 0)||(_dir == 1 && PurchaseButtonId == CHARACTER_DEFINE.characterVarietyNum - 1)) return;





		posx = TopCharacterLocation.transform.position.x - 10 * _dir;
		tposx = Mathf.RoundToInt (posx / 10);
		iTween.MoveTo (TopCharacterLocation, iTween.Hash ("x", tposx * 10, "time", 2));

		Debug.Log ("tposx*10 = " + tposx * 10);

		PurchaseButtonId = -tposx - loop * 10;
		//Debug.Log ("PrePurchaseButtonId =" + PurchaseButtonId + "-tposx = " + (-tposx));

		//loop1
		if (_dir == 1 && PurchaseButtonId == CHARACTER_DEFINE.characterVarietyNum) {
				
			if (loopflag == true && dirFlag == true) {
				Vector3 pos = CharacterLocation2.transform.position;
				Vector3 pos2 = CharacterLocation1.transform.position;
				pos.x = pos2.x + 100f;
				CharacterLocation2.transform.position = pos;
				Debug.Log ("loopflag1" + loopflag);
				loopflag = false;
			} else if (loopflag == false && dirFlag == true) {
				Vector3 pos = CharacterLocation1.transform.position;
				Vector3 pos2 = CharacterLocation2.transform.position;
				pos.x = pos2.x + 100f;
				CharacterLocation1.transform.position = pos;
				Debug.Log ("loopflag2" + loopflag);
				loopflag = true;
			} else if (dirFlag == false) {
				dirFlag = true;
			}
				//初期化
			PurchaseButtonId = 0;
			loop += 1;
			ignoreLoopFlag1 = true;
			//CharacterLocation2.transform.position = new Vector3 (100f, 0, 0);
			Debug.Log ("looped , PurchaseNButtonID =" + PurchaseButtonId + "loop = " + loop);
		}

		//loop2
		if (_dir == -1 && PurchaseButtonId == -1) {
			if (loopflag == true && dirFlag == false) {
				Vector3 pos = CharacterLocation1.transform.position;
				Vector3 pos2 = CharacterLocation2.transform.position;
				pos.x = pos2.x - 100f;
				CharacterLocation1.transform.position = pos;
				Debug.Log ("loopflag3" + loopflag);
				loopflag = false;
					
					
			} else if (loopflag == false && dirFlag == false) {
				Vector3 pos = CharacterLocation2.transform.position;
				Vector3 pos2 = CharacterLocation1.transform.position;
				pos.x = pos2.x - 100f;
				CharacterLocation2.transform.position = pos;
				Debug.Log ("loopflag4" + loopflag);
				loopflag = true;
			} else if (dirFlag == true) {
				dirFlag = false;
			}
			//初期化

			PurchaseButtonId = 9;
			loop -= 1;
			//CharacterLocation2.transform.position = new Vector3 (100f, 0, 0);
			//Debug.Log ("looped , PurchaseNButtonID =" + PurchaseButtonId + "loop = " + loop);
		}
			


		if (((CHARACTER_DEFINE.MONEY [PurchaseButtonId] > UserDataManager.I.GetMoney ()) && UserDataManager.I.IsPermitUseCharacter(PurchaseButtonId) == false)||GetPrice() > UserDataManager.I.GetMoney() && UserDataManager.I.IsPermitUseCharacter(PurchaseButtonId)) {
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
		//Debug.Log ("CharacterLevel = " + UserDataManager.I.GetCharacterLevel(PurchaseButtonId).ToString());

		//PurchaseButtonのテキスト更新
		ReloadMoneyText();
		Debug.Log ("PurchaseButtonId ="+PurchaseButtonId);





	}
		
	private void ReloadMoneyText(){
		//PurchaseButtonのテキスト更新
		if(UserDataManager.I.GetCharacterLevel(PurchaseButtonId) >= 20) return;
		purchaseButtonMoneyText.text = GetPrice().ToString ();
	}

	public void ShowLevelText(){
		levelText.text = "Lv:" + UserDataManager.I.GetCharacterLevel (PurchaseButtonId).ToString();
	}

	public void ShowMoneyText(){
		moneyText.text = UserDataManager.I.GetMoney ().ToString ();
	}

	public void NewPurchaseButton(){


		//Upgrade
		if (UserDataManager.I.IsPermitUseCharacter (PurchaseButtonId) == true && UserDataManager.I.GetMoney() >= GetPrice()){
			//LVに応じて金額変動
			UserDataManager.I.ReduceMoney (GetPrice());
			UserDataManager.I.AddCharacterLevel (PurchaseButtonId);
			ShowMoneyText();
			ShowLevelText ();
			if (UserDataManager.I.GetCharacterLevel (PurchaseButtonId) >= 20) {
				truePurchaseButton.interactable = false;
				purchaseButtonText.text = "Lv MAX";
			}
			HideTruePurchaseButton ();
		}

		//Purchase
		if (UserDataManager.I.IsPermitUseCharacter (PurchaseButtonId) == false && UserDataManager.I.GetMoney() >= CHARACTER_DEFINE.MONEY[PurchaseButtonId]) {
			UserDataManager.I.ReduceMoney (CHARACTER_DEFINE.MONEY [PurchaseButtonId]);
			UserDataManager.I.GetCharacter (PurchaseButtonId);

		}

		SoundManager.I.SoundSE (SE.PURCHASE);
	



		moneyText.text = UserDataManager.I.GetMoney ().ToString ();
		ReloadMoneyText ();

		//UserDataManager.I.GetCharacter (PurchaseButtonId);
		//Debug.Log ("You bought it");
		purchaseButtonText.text = "Grade UP";


		HideTruePurchaseButton ();

	}

	/*
	private int GetMoneyPattern(){
		if (UserDataManager.I.GetCharacterLevel (PurchaseButtonId) >= 20) {
			
			return 99999;
		}
		return CHARACTER_DEFINE.MONEYPATTERN [UserDataManager.I.GetCharacterLevel (PurchaseButtonId)];
	}
	*/

	private int GetPrice(){
		CharacterStatusManager.I.ParseStatusInfoText ("ATLANTA");
		return CharacterStatusManager.I.GetCharacterMoney (PurchaseButtonId,UserDataManager.I.GetCharacterLevel (PurchaseButtonId));
	}

	public void ShowDebugLog(){
		Debug.Log ("Price = " + GetPrice());
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
		if (UserDataManager.I.GetMoney() < GetPrice() ){
			truePurchaseButton.interactable = false;
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterStoreController : SingletonBehaviour<CharacterStoreController> {

	[SerializeField] private Image[] selectCharacterBackground;
	[SerializeField] private GameObject statusPanel;
	[SerializeField] private Sprite[] characterStatus;
	[SerializeField] private Image showStatusImage;

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
		selectCharacterBackground [UserDataManager.I.GetUseCharacterIndex ()].color = Color.white;
		UserDataManager.I.SetUseCharacterIndex (_charaId);	
		selectCharacterBackground [UserDataManager.I.GetUseCharacterIndex ()].color = Color.red;
		UserDataManager.I.SaveData ();
	}

	public void ShowCharacterStatus(int _charaId){
		Debug.Log ("Show");
		showStatusImage.sprite = characterStatus [_charaId];
		statusPanel.SetActive (true);
	}

	public void HideCharacterStatus(){
		statusPanel.SetActive (false);
	}

	public void ReturnButton() {
		AppSceneManager.I.GoScene(GameSceneType.MENU_SCENE);
	}
}

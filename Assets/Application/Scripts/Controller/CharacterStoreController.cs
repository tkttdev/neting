using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterStoreController : MonoBehaviour {

	[SerializeField] private Image[] selectCharacterBackground;

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

	public void CharacterSelectButton(int _charaIndex) {
		if (UserDataManager.I.IsPermitUseCharacter (_charaIndex)) {
			selectCharacterBackground [UserDataManager.I.GetUseCharacterIndex ()].color = Color.white;
			selectCharacterBackground [_charaIndex].color = Color.red;
			UserDataManager.I.SetUseCharacterIndex (_charaIndex);
			UserDataManager.I.SaveData ();
		}

	}

	public void ReturnButton() {
		AppSceneManager.I.GoScene(GameSceneType.MENU_SCENE);
	}
}

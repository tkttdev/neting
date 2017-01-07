using UnityEngine;
using System.Collections;

public class CharacterSelectController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		if(GameObject.Find("Systems") == null){
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
	}

    public void CharacterSelectButton(int _charaIndex) {
        UserDataManager.I.SetUseCharacterIndex(_charaIndex);
        
    }

    public void ReturnButton() {
        AppSceneManager.I.GoScene(GameSceneType.MENU_SCENE);
    }
}

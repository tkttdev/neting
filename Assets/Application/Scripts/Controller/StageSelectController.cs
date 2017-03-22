using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageSelectController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		if(GameObject.Find("Systems") == null){
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif

		SoundManager.I.SoundBGM (BGM.MENU_BGM);
	}

	public void StageSelectButton(int stageLevel){
		if (stageLevel == 0) {
			stageLevel = 1;
		}
		StageLevelManager.I.SetStageLevel (stageLevel);
		SoundManager.I.SoundSE (SE.BUTTON0);
		AppSceneManager.I.GoScene(GameSceneType.GAME_SCENE);
	}

	public void CharacterButton() {
		SoundManager.I.SoundSE (SE.BUTTON2);
		AppSceneManager.I.GoScene(GameSceneType.CHARACTER_STORE);
	}
}

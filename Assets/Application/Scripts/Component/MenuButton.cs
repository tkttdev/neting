using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {

	public void StageSelectButtonPressed(int stageLevel){
		if (stageLevel == 0) {
			stageLevel = 1;
		}
		StageLevelManager.I.SetStageLevel (stageLevel);
		AppSceneManager.I.GoScene(GameSceneType.GAME_SCENE);
	}

    public void CharacterButtonPressed() {
        AppSceneManager.I.GoScene(GameSceneType.CHARACTER_SELECT_SCENE);
    }

    public void StoreButton() {
        AppSceneManager.I.GoScene(GameSceneType.STORE_SCENE);
    }
}

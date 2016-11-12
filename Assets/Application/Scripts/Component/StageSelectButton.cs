using UnityEngine;
using System.Collections;

public class StageSelectButton : MonoBehaviour {

	public void StageSelectButtonPressed(int stageLevel){
		if (stageLevel == 0) {
			stageLevel = 1;
		}
		StageLevelManager.I.SetStageLevel (stageLevel);
		AppSceneManager.I.GoCharacterSelect ();
	}
}

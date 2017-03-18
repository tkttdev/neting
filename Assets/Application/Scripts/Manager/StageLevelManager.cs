using UnityEngine;
using System.Collections;

public class StageLevelManager : SingletonBehaviour<StageLevelManager> {
	[SerializeField] private int stageLevel = 1;

	protected override void Initialize (){
		base.Initialize ();
		stageLevel = 1;
	}

	public void SetStageLevel(int _stageLevel){
		stageLevel = _stageLevel;
	}

	public int GetStageLevel(){
		return stageLevel;
	}
}

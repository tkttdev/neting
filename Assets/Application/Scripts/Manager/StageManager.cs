using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonBehaviour<StageManager> {

	public bool isDemo = false;

	protected override void Initialize (){
		base.Initialize ();
		if (!isDemo) {
			GameObject stage = Resources.Load ("Prefabs/Stage/Stage" + StageLevelManager.I.GetStageLevel ().ToString ()) as GameObject;
			Instantiate (stage);
		}
	}
}

using UnityEngine;
using System.Collections;

public class GameManager : SingletonBehaviour<GameManager> {

	protected override void Initialize (){
		base.Initialize ();

		#if UNITY_EDITOR
		if(GameObject.Find("Systems") == null){
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
	}
}

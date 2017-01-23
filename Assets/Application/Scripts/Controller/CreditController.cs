using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditController : MonoBehaviour {

	void Start(){
		#if UNITY_EDITOR
		if (GameObject.Find("Systems") == null) {
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
	}

	public void TitleButton(){
		SoundManager.I.SoundSE (SE.BUTTON2);
		AppSceneManager.I.GoScene (GameSceneType.TITLE_SCENE);
	}
}

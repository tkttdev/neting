using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditController : MonoBehaviour {

	[SerializeField]private GameObject tapText;

	void Start(){
		#if UNITY_EDITOR
		if (GameObject.Find("Systems") == null) {
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
		SoundManager.I.SoundBGM (BGM.CREDIT);
		iTween.ScaleTo (tapText, iTween.Hash ("x", 1.0f, "y", 1.0f, "easeType", iTween.EaseType.linear, "loopType", iTween.LoopType.pingPong));
	}

	public void TitleButton(){
		SoundManager.I.SoundSE (SE.BUTTON2);
		AppSceneManager.I.GoScene (GameSceneType.TITLE_SCENE);
	}
}

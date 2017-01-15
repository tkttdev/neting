using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditController : MonoBehaviour {

	public void TitleButton(){
		AppSceneManager.I.GoScene (GameSceneType.TITLE_SCENE);
	}
}

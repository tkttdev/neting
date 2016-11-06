using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			AppSceneManager.I.GoStageSelect ();
		}
	}
}

using UnityEngine;
using System.Collections;

public class StageSelectController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		if(GameObject.Find("Systems") == null){
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

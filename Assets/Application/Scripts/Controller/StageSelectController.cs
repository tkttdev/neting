using UnityEngine;
using System.Collections;

public class StageSelectController : MonoBehaviour {

    [SerializeField] RectTransform stageButtonTransform;
    private bool isTouch = false;
    private float[] stageButtonRootFixedX = new float[2];
    private float frickDist = 0.0f;

	// Use this for initialization
	void Start () {
        stageButtonRootFixedX[0] = 0;
        stageButtonRootFixedX[1] = 800;
		#if UNITY_EDITOR
		if(GameObject.Find("Systems") == null){
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
	}
	
	// Update is called once per frame
	void Update () {
       /* if (Input.GetMouseButtonDown(0)) {
            isTouch = true;
            frickDist = 0.0f;
        } else if (Input.GetMouseButton(0)) {
            if(stageButtonTransform.position.x >= 0 && stageButtonTransform.position.x <= 800) {

            }
        } else if (Input.GetMouseButtonUp(0)) {
            isTouch = false;
        }

        if (!isTouch) {
            if(stageButtonTransform.position.x != stageButtonRootFixedX[0] && stageButtonTransform.position.x != stageButtonRootFixedX[1]) {
            }
        }*/
	}
}

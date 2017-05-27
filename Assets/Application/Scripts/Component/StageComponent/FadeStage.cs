using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeStage : MonoBehaviour {

	private bool isAnimation = true;

	// Use this for initialization
	void Start () {
        iTween.ValueTo(gameObject,iTween.Hash("loopType",iTween.LoopType.pingPong,"easeType",iTween.EaseType.linear,"from",0.0f,"to",1.0f,"onupdate","SetAlpha"));
	}
	
    private void SetAlpha(float _a){
        gameObject.GetComponent<SpriteRenderer>().color =new Color(255,255,255,_a);
    }
	// Update is called once per frame
	void Update () {
		if (!GameManager.I.CheckGameStatus (GameStatus.PLAY) && isAnimation) {
			iTween.Pause (gameObject);
			isAnimation = false;
		} else if (!isAnimation) {
			iTween.Resume (gameObject);
			isAnimation = true;
		}
	}
}

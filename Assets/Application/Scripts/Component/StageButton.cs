using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		MoveCamera();
	}

	public void MoveCamera() {
		float scale = gameObject.transform.localScale.x;

		if (Input.touchCount == 1) {
			Touch tap = Input.GetTouch(0);
			Vector3 deltaPos = tap.deltaPosition;

			iTween.MoveBy(gameObject, iTween.Hash("x", deltaPos.x / 10, "y", deltaPos.y / 20));
		}

		if (Input.touchCount == 2) {
			Touch zero = Input.GetTouch(0);
			Touch one = Input.GetTouch(1);

			Vector2 zeroPre = zero.position - zero.deltaPosition;
			Vector2 onePre = one.position - one.deltaPosition;

			float magPre = (zeroPre - onePre).magnitude;
			float mag = (zero.position - one.position).magnitude;

			float deltaScale = magPre - mag;

			gameObject.transform.localScale -= new Vector3(deltaScale / 1000, deltaScale / 1000, 0);
		}

		if (gameObject.transform.localPosition.x <= -700 + (1 - scale) * 800) {
			gameObject.transform.localPosition = new Vector3(-700 + (1 - scale) * 800, gameObject.transform.localPosition.y, 0);
		}
		if (gameObject.transform.localPosition.x >= 0 - (1 - scale) * 120) {
			gameObject.transform.localPosition = new Vector3(0 - (1 - scale) * 120, gameObject.transform.localPosition.y, 0);
		}
		if (gameObject.transform.localPosition.y <= -300 + (1 - scale) * 500) {
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, -300 + (1 - scale) * 500, 0);
		}
		if (gameObject.transform.localPosition.y >= 0 - (1 - scale) * 100) {
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0 - (1 - scale) * 100, 0);
		}
		if (gameObject.transform.localScale.x <= 0.5) {
			gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
		}
		if (gameObject.transform.localScale.x >= 1) {
			gameObject.transform.localScale = new Vector3(1, 1, 1);
		}
	}
}

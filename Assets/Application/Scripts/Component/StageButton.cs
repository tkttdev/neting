using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour {

	private float scale;
	private Vector3 speed;
	private static Vector3 PreviousPosition = Vector3.zero;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		MoveCamera();
	}

	private void MoveCamera() {
		scale = gameObject.transform.localScale.x;
		speed = new Vector3(1, 1, 0);
		TouchInfo info = GetTouch();

		if (info == TouchInfo.Moved) {
			Vector3 deltaPos = GetDeltaPosition();

			if (gameObject.transform.localPosition.x <= -600 + (1 - scale) * 800 && deltaPos.x < 0) {
				speed -= new Vector3((-600 + (1 - scale) * 800 - gameObject.transform.localPosition.x) / 100, 0, 0);
			}
			if (gameObject.transform.localPosition.x >= -100 - (1 - scale) * 120 && deltaPos.x > 0) {
				speed -= new Vector3((gameObject.transform.localPosition.x + 100 + (1 - scale) * 120) / 100, 0, 0);
			}
			if (gameObject.transform.localPosition.y <= -200 + (1 - scale) * 500 && deltaPos.y < 0) {
				speed -= new Vector3(0, (-200 + (1 - scale) * 500 - gameObject.transform.localPosition.y) / 100, 0);
			}
			if (gameObject.transform.localPosition.y >= -100 - (1 - scale) * 100 && deltaPos.y > 0) {
				speed -= new Vector3(0, (gameObject.transform.localPosition.y + 100 + (1 - scale) * 100) / 100, 0);
			}

			iTween.MoveAdd(gameObject, iTween.Hash("x", deltaPos.x * 50 * speed.x / Screen.height, "y", deltaPos.y * 50 * speed.y / Screen.width));
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

	public static TouchInfo GetTouch() {
		if (Application.isEditor) {
			if (Input.GetMouseButtonDown(0)) { return TouchInfo.Began; }
			if (Input.GetMouseButton(0)) { return TouchInfo.Moved; }
			if (Input.GetMouseButtonUp(0)) { return TouchInfo.Ended; }
		} else {
			if (Input.touchCount > 0) {
				return (TouchInfo)((int)Input.GetTouch(0).phase);
			}
		}
		return TouchInfo.None;
	}

	public static Vector3 GetDeltaPosition() {
		if (Application.isEditor) {
			TouchInfo info = GetTouch();
			if (info != TouchInfo.None) {
				Vector3 currentPosition = Input.mousePosition;
				Vector3 delta = currentPosition - PreviousPosition;
				PreviousPosition = currentPosition;
				return delta;
			}
		} else {
			if (Input.touchCount > 0) {
				Touch touch = Input.GetTouch(0);
				PreviousPosition.x = touch.deltaPosition.x;
				PreviousPosition.y = touch.deltaPosition.y;
				return PreviousPosition;
			}
		}
		return Vector3.zero;
	}

	public enum TouchInfo {
		None = 99,
		Began = 0,
		Moved = 1,
		Stationary = 2,
		Ended = 3,
		Canceled = 4,
	}
}
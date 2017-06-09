using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour {
	#region private_field
	[SerializeField]private float thresholdX = 10f;
	[SerializeField]private float thresholdY = 10f;
	[SerializeField]private float maxSpeed = 1f;
	[SerializeField]private float maxSize = 10f;
	[SerializeField]private float minSize = 5f;
	[SerializeField]private float dynamicFriction = 1f;
	private Vector3 moveDis;
	private Vector3 moveVector;
	private float speed = 0;
	private float screenSpeed = 0f;
	private float inertiaTime = 0.0f;
	private float beforePinchDis;
	private float pinchDis;
	private float size = 0f;
	private Vector3 pinchCenterPos;
	private Camera mainCamera;
	#endregion

	void Start(){
		size = Camera.main.orthographicSize;
		mainCamera = Camera.main;
	}

	void LateUpdate () {
		if (Input.touchCount > 1) {
			Touch touch0 = Input.touches [0];
			Touch touch1 = Input.touches [1];
			if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began) {
				Vector3 touch0Pos = new Vector3 (touch0.position.x / Screen.width, touch0.position.y / Screen.height, 0);
				Vector3 touch1Pos = new Vector3 (touch1.position.x / Screen.width, touch1.position.y / Screen.height, 0);
				beforePinchDis = (touch0Pos - touch1Pos).magnitude;
				pinchCenterPos = (Camera.main.ScreenToWorldPoint (touch0.position) + Camera.main.ScreenToWorldPoint (touch1.position)) / 2;
				pinchCenterPos = new Vector3 (pinchCenterPos.x, pinchCenterPos.y, -10);
			} else if(touch0.phase == TouchPhase.Moved){
				Vector3 touch0Pos = new Vector3 (touch0.position.x / Screen.width, touch0.position.y / Screen.height, 0);
				Vector3 touch1Pos = new Vector3 (touch1.position.x / Screen.width, touch1.position.y / Screen.height, 0);
				pinchDis = (touch0Pos - touch1Pos).magnitude;
				float diff = pinchDis - beforePinchDis;
				float beforeSize = size;
				size += diff * 5f;
				size = Mathf.Clamp (size, minSize, maxSize);
				float sizeDiff = size - beforeSize;
				mainCamera.orthographicSize = size;
				if (diff < 0) {
					Vector3 moveVec = sizeDiff * 2 * (pinchCenterPos - Camera.main.transform.position).normalized;
					if ((pinchCenterPos - Camera.main.transform.position).magnitude < moveVec.magnitude) {
						Camera.main.transform.position = pinchCenterPos;
					} else {
						Camera.main.transform.position -= moveVec;
					}
					
				}
				beforePinchDis = pinchDis;
			}
		} else if (Input.touchCount > 0) {
			Touch touch = Input.touches [0];
			if (touch.phase == TouchPhase.Moved) {
				moveVector = new Vector3 (touch.deltaPosition.x / Screen.width, touch.deltaPosition.y / Screen.height, 0);
				screenSpeed = moveVector.magnitude / Time.deltaTime * 6;
				speed = (screenSpeed > speed) ? screenSpeed : speed;
				speed = Mathf.Clamp (speed, 0, maxSpeed);
				moveDis = moveVector * speed * Time.deltaTime;
				DecayMoveDis ();
				transform.position += moveDis;
				CheckLimit ();
			}
			if (touch.phase == TouchPhase.Ended) {
				inertiaTime = 0f;
			}
		}

		if (speed > Mathf.Epsilon) {
			speed -= dynamicFriction * inertiaTime;
			speed = Mathf.Clamp (speed, 0, maxSpeed);
			moveDis = moveVector * speed * Time.deltaTime;
			DecayMoveDis ();
			transform.position += moveDis;
			inertiaTime += Time.deltaTime;
		}
		CheckLimit ();
	}

	private void DecayMoveDis(){
		float targetThresholdX = moveVector.x > 0 ? thresholdX : -thresholdX;
		float targetThresholdY = moveVector.y > 0 ? thresholdY : -thresholdY;
		float nowX = moveVector.x > 0 ? Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, 10)).x : Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 10)).x;
		float nowY = moveVector.y > 0 ? Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, 10)).y : Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 10)).y;
		float disX = Mathf.Abs (targetThresholdX - nowX);
		float disY = Mathf.Abs (targetThresholdY - nowY);
		if (disX < 1.0f) {
			moveDis = new Vector3 (moveDis.x * disX * disX, moveDis.y, moveDis.z);
		}
		if (disY < 1.0f) {
			moveDis = new Vector3 (moveDis.x, moveDis.y * disY * disY, moveDis.z);
		}
	}

	private void CheckLimit(){
		Vector3 cameraLeftTop = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 10));
		Vector3 cameraRightDown = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 10));
		float modifyX = 0f;
		float modifyY = 0f;
		if (cameraLeftTop.x < -thresholdX) {
			modifyX = -thresholdX - cameraLeftTop.x;
		} else if (cameraRightDown.x > thresholdX) {
			modifyX = thresholdX - cameraRightDown.x;
		} 
		if (cameraLeftTop.y < -thresholdY) {
			modifyY = -thresholdY - cameraLeftTop.y;
		} else if (cameraRightDown.y > thresholdY) {
			modifyY = thresholdY - cameraRightDown.y;
		}
		transform.position += new Vector3 (modifyX, modifyY, 0);
	}

	#if UNITY_EDITOR
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine (new Vector3 (-thresholdX, -thresholdY, 0), new Vector3 (thresholdX, -thresholdY, 0));
		Gizmos.DrawLine (new Vector3 (-thresholdX, thresholdY, 0), new Vector3 (thresholdX, thresholdY, 0));
		Gizmos.DrawLine (new Vector3 (-thresholdX, -thresholdY, 0), new Vector3 (-thresholdX, thresholdY, 0));
		Gizmos.DrawLine (new Vector3 (thresholdX, -thresholdY, 0), new Vector3 (thresholdX, thresholdY, 0));
	}
	#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	#region private_field
	[SerializeField]private float limitWidth = 10f;
	[SerializeField]private float limitHeight = 10f;
	[SerializeField]private float maxSpeed = 1f;
	[SerializeField]private float maxSize = 10f;
	[SerializeField]private float minSize = 5f;
	[SerializeField]private float dynamicFriction = 1f;
	[SerializeField]private Vector2 limitCenterPoint;
	private float[] thresholdX = new float[2];
	private float[] thresholdY = new float[2];
	private Vector3 moveDis;
	private Vector3 moveVector;
	private Vector3 touch0Pos;
	private Vector3 touch1Pos;
	private float speed = 0;
	private float screenSpeed = 0f;
	private float inertiaTime = 0.0f;
	private float beforePinchDis;
	private float pinchDis;
	private float size = 0f;
	private Vector3 pinchCenterPos;
	private Camera mainCamera;
	#if UNITY_EDITOR
	private Vector3 beforePos;
	#endif
	#endregion

	void Start(){
		size = Camera.main.orthographicSize;
		mainCamera = Camera.main;
		thresholdX [0] = limitCenterPoint.x - limitWidth / 2f;
		thresholdX [1] = limitCenterPoint.x + limitWidth / 2f;
		thresholdY [0] = limitCenterPoint.y - limitHeight / 2f;
		thresholdY [1] = limitCenterPoint.y + limitHeight / 2f;
	}

	void LateUpdate () {
		#if UNITY_EDITOR
		//TODO : ガクつくのをなくす
		if(Input.GetMouseButtonDown(0)){
			beforePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		}else if(Input.GetMouseButton(0)){
			Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			Vector3 diffVec = mousePos - beforePos;
			mainCamera.transform.position -= diffVec;
			beforePos = mousePos;
		}
		#elif UNITY_IOS || UNITY_ANDROID
		if (Input.touchCount > 1) {
			Touch touch0 = Input.touches [0];
			Touch touch1 = Input.touches [1];
			if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began) {
				InitPinch (touch0, touch1);
			} else if(touch0.phase == TouchPhase.Moved){
				touch0Pos = NormalizeTouchPos (touch0);
				touch1Pos = NormalizeTouchPos (touch1);
				pinchDis = (touch0Pos - touch1Pos).magnitude;

				float diff = pinchDis - beforePinchDis;
				float beforeSize = size;
				size -= diff * 5f;
				size = Mathf.Clamp (size, minSize, maxSize);
				float sizeDiff = size - beforeSize;
				mainCamera.orthographicSize = size;
				if (diff > 0) {
					Vector3 moveVec = sizeDiff * 2 * (pinchCenterPos - Camera.main.transform.position).normalized;
					if ((pinchCenterPos - Camera.main.transform.position).magnitude < moveVec.magnitude) {
						mainCamera.transform.position = pinchCenterPos;
					} else {
						mainCamera.transform.position -= moveVec;
					}
				}
				beforePinchDis = pinchDis;
			}
		} else if (Input.touchCount > 0) {
			Touch touch = Input.touches [0];
			if (touch.phase == TouchPhase.Moved) {
				moveVector = -new Vector3 (touch.deltaPosition.x / Screen.width, touch.deltaPosition.y / Screen.height, 0);
				screenSpeed = moveVector.magnitude / Time.deltaTime * 6;
				speed = (screenSpeed > speed) ? screenSpeed : speed;
				speed = Mathf.Clamp (speed, 0, maxSpeed);
				moveDis = moveVector * speed * Time.deltaTime;
				DecayMoveDis ();
				mainCamera.transform.position += moveDis;
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
			mainCamera.transform.position += moveDis;
			inertiaTime += Time.deltaTime;
		}
		#endif
		CheckLimit ();
	}

	private Vector3 NormalizeTouchPos(Touch _touch){
		return new Vector3 (_touch.position.x / Screen.width, _touch.position.y / Screen.height, 0);
	}

	private void InitPinch(Touch _touch0, Touch _touch1){
		touch0Pos = NormalizeTouchPos (_touch0);
		touch1Pos = NormalizeTouchPos (_touch1);
		beforePinchDis = (touch0Pos - touch1Pos).magnitude;
		pinchCenterPos = (mainCamera.ScreenToWorldPoint (_touch0.position) + mainCamera.ScreenToWorldPoint (_touch1.position)) / 2;
		pinchCenterPos = new Vector3 (pinchCenterPos.x, pinchCenterPos.y, -10);
	}

	private void DecayMoveDis(){
		float targetThresholdX = moveVector.x > 0 ? thresholdX[1] : thresholdX[0];
		float targetThresholdY = moveVector.y > 0 ? thresholdY[1] : thresholdY[0];
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
		if (cameraLeftTop.x < thresholdX[0]) {
			modifyX = thresholdX[0] - cameraLeftTop.x;
		} else if (cameraRightDown.x > thresholdX[1]) {
			modifyX = thresholdX[1] - cameraRightDown.x;
		} 
		if (cameraLeftTop.y < thresholdY[0]) {
			modifyY = thresholdY[0] - cameraLeftTop.y;
		} else if (cameraRightDown.y > thresholdY[1]) {
			modifyY = thresholdY[1] - cameraRightDown.y;
		}
		transform.position += new Vector3 (modifyX, modifyY, 0);
	}

	#if UNITY_EDITOR
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine (new Vector3 (limitCenterPoint.x - limitWidth / 2f, limitCenterPoint.y - limitHeight / 2f, 0), new Vector3 (limitCenterPoint.x + limitWidth / 2f, limitCenterPoint.y - limitHeight / 2f, 0));
		Gizmos.DrawLine (new Vector3 (limitCenterPoint.x - limitWidth / 2f, limitCenterPoint.y + limitHeight / 2f, 0), new Vector3 (limitCenterPoint.x + limitWidth / 2f, limitCenterPoint.y + limitHeight / 2f, 0));
		Gizmos.DrawLine (new Vector3 (limitCenterPoint.x - limitWidth / 2f, limitCenterPoint.y - limitHeight / 2f, 0), new Vector3 (limitCenterPoint.x - limitWidth / 2f, limitCenterPoint.y + limitHeight / 2f, 0));
		Gizmos.DrawLine (new Vector3 (limitCenterPoint.x + limitWidth / 2f, limitCenterPoint.y - limitHeight / 2f, 0), new Vector3 (limitCenterPoint.x + limitWidth / 2f, limitCenterPoint.y + limitHeight / 2f, 0));
		Gizmos.DrawCube (new Vector3 (limitCenterPoint.x, limitCenterPoint.y, 0), new Vector3 (0.2f, 0.2f, 0.1f));
		//Gizmos.DrawCube (pinchCenterPos, new Vector3 (1f, 1f, 1f));
	}
	#endif
}

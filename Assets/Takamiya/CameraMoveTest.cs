using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveTest : MonoBehaviour {
	#region private_field
	[SerializeField]private float thresholdX = 10f;
	[SerializeField]private float thresholdY = 10f;
	[SerializeField]private float maxSpeed = 1f;
	[SerializeField]private float dynamicFriction = 1f;
	private Vector3 moveDis;
	private float speed = 0;
	private Vector3 moveVector;
	private float screenSpeed = 0f;
	private float inertiaTime = 0.0f;
	#endregion

	void LateUpdate () {
		if (Input.touchCount > 0) {
			Touch touch = Input.touches [0];
			if (touch.phase == TouchPhase.Moved) {
				moveVector = new Vector3 (touch.deltaPosition.x / Screen.width, touch.deltaPosition.y / Screen.height, 0);
				screenSpeed = moveVector.magnitude / Time.deltaTime * 6;
				speed = (screenSpeed > speed) ? screenSpeed : speed;
				speed = Mathf.Clamp (speed, 0, maxSpeed);
				Debug.Log (speed);
				moveDis = moveVector * speed * Time.deltaTime;
				DecayMoveDis ();
				transform.position += moveDis;
				return;
			} if (touch.phase == TouchPhase.Ended) {
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
	}

	void DecayMoveDis(){
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
		if (Mathf.Abs (moveDis.x) > disX) {
			moveDis = new Vector3 (Mathf.Abs (moveDis.x)/moveDis.x * disX, moveDis.y, moveDis.z);
		}
		if (Mathf.Abs (moveDis.y) > disY) {
			moveDis = new Vector3 (moveDis.x, Mathf.Abs (moveDis.y)/moveDis.y * disY, moveDis.z);
		}
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

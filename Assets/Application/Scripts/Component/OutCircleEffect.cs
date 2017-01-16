using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutCircleEffect : MonoBehaviour {
	private float rotateZ = 0.0f;
	private RectTransform rectTransform;
	private float rotateSpeed = 50.0f;

	void Start(){
		rectTransform = gameObject.GetComponent<RectTransform> ();
	}

	// Update is called once per frame
	void Update () {
		rotateZ = Mathf.Repeat (rotateZ + Time.deltaTime * rotateSpeed, 360.0f);
		rectTransform.rotation = Quaternion.Euler (new Vector3 (0.0f, 0.0f, rotateZ));
	}
}

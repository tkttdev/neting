using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	float angle = 0f;

	// Update is called once per frame
	void Update () {
		angle -= Time.deltaTime * 30.0f;
		gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler (0, 0, Mathf.Repeat (angle, 360.0f)); 
	}
}

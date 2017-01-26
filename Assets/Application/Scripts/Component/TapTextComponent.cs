using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTextComponent : MonoBehaviour {

	private float spendTime = 0.0f;
	
	// Update is called once per frame
	void Update () {
		spendTime += Time.deltaTime;
		if (spendTime >= 2.5f) {
			Destroy (gameObject);
		}
	}
}

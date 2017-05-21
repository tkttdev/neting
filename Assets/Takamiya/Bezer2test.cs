using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezer2test : MonoBehaviour {

	public GameObject[] obj = new GameObject[4];
	public int fineness = 20;
	public int[,] test = new int[3,3];

	void OnDrawGizmos(){
		for (int i = 0; i < 4; i++) {
			if (obj [i] == null)
				return;
		}
		Gizmos.color = Color.red;
		float t = 0.0f;
		for (int i = 0; i < fineness - 1; i++) {
			t += 1f / fineness;
			Vector3 tmp1 = GetPoint (obj [0].transform.position, obj [1].transform.position, obj [2].transform.position, obj [3].transform.position, t);
			Vector3 tmp2 = GetPoint (obj [0].transform.position, obj [1].transform.position, obj [2].transform.position, obj [3].transform.position, Mathf.Clamp (t + 1f/fineness, 0.0f, 1.0f));
			Gizmos.DrawLine (tmp1, tmp2);
		}
	}

	Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		var oneMinusT = 1f - t;
		return oneMinusT * oneMinusT * oneMinusT * p0 +
			3f * oneMinusT * oneMinusT * t * p1 +
			3f * oneMinusT * t * t * p2 +
			t * t * t * p3;
	}
}

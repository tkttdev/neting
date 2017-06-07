using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaButton : MonoBehaviour {

	[SerializeField]private GameObject[] objectSet;
	[SerializeField]private GameObject chara;
	private Vector3 pos;

	void Start () {
		for (int i = 0; i < objectSet.Length; i++) {
			objectSet [i].SetActive (false);
		}
	}

	void Update () {
		pos = gameObject.transform.position;

		var mag = pos - chara.transform.position;
		if (mag.magnitude <= 0.5) {
			for (int i = 0; i < objectSet.Length; i++) {
				objectSet[i].SetActive(true);
			}
		} else {
			for (int i = 0; i < objectSet.Length; i++) {
				objectSet[i].SetActive(false);
			}
		}
	}

	public void PushAreaButton() {
		var dis = pos - chara.transform.position;
		iTween.MoveBy(chara, iTween.Hash("x", dis.x, "y", dis.y, "time", 5.0f));
	}
}

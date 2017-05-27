using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaButton : MonoBehaviour {

	[SerializeField]private GameObject[] objectSet;

	private GameObject chara;
	private Vector3 pos;

	// Use this for initialization
	void Start () {
		chara = GameObject.Find("Chara");

		foreach (GameObject n in objectSet) {
			n.gameObject.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update () {
		pos = gameObject.transform.position;
	}

	public void PushAreaButton() {
		GameObject[] stageButtons = GameObject.FindGameObjectsWithTag("StageButton");
		foreach (GameObject stageButton in stageButtons) {
			stageButton.SetActive(false);
		}

		var dis = pos - chara.transform.position;
		iTween.MoveBy(chara, iTween.Hash("x", dis.x, "y", dis.y, "time", 5.0f));
		StartCoroutine("SetStageButton");
	}

	public IEnumerator SetStageButton() {
		yield return new WaitForSeconds(2.0f);

		foreach (GameObject n in objectSet) {
			n.gameObject.SetActive(true);
		}
	}
}

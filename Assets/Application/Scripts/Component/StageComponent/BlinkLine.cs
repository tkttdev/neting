using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BlinkLine : MonoBehaviour {
	[SerializeField] private SpriteRenderer[] targetLine;
	[SerializeField] private GameObject[] targetCorner;

	private List<GameObject> stayMoveObject = new List<GameObject>();
	private bool isExist = true;

	void Update(){
		for (int i = 0; i < stayMoveObject.Count; i++) {
			if (stayMoveObject [i] == null) {
				stayMoveObject.RemoveAt (i);
			}
			CheckBlink ();
		}
	}

	private void OnTriggerEnter2D(Collider2D _other){
		stayMoveObject.Add (_other.gameObject);
	}

	private void OnTriggerExit2D(Collider2D _other){
		stayMoveObject.Remove (_other.gameObject);
		CheckBlink ();
	}

	private void CheckBlink(){
		if (stayMoveObject.Count == 0) {
			if (isExist) {
				for (int i = 0; i < targetLine.Length; i++) {
					targetLine[i].enabled = false;
				}
				for (int i = 0; i < targetCorner.Length; i++) {
					targetCorner [i].SetActive (false);
				}
				isExist = false;
			} else {
				for (int i = 0; i < targetLine.Length; i++) {
					targetLine[i].enabled = true;
				}
				for (int i = 0; i < targetCorner.Length; i++) {
					targetCorner [i].SetActive (true);
				}
				isExist = true;
			}
		}
	}
}

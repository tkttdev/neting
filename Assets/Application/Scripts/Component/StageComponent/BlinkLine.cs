using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BlinkLine : MonoBehaviour {
	[SerializeField] private SpriteRenderer[] targetLine;
	[SerializeField] private GameObject[] targetCorner;
	private string[] targetCornerTags;

	private List<GameObject> stayMoveObject = new List<GameObject>();
	private bool isExist = true;

	void Awake(){
		targetCornerTags = new string[targetCorner.Length];
		for (int i = 0; i < targetCorner.Length; i++) {
			targetCornerTags [i] = targetCorner [i].tag;
		}
	}

	void Update(){
		for (int i = 0; i < stayMoveObject.Count; i++) {
			if (!stayMoveObject [i].activeInHierarchy) {
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
					targetCorner [i].tag = "PassCorner";
				}
				isExist = false;
			} else {
				for (int i = 0; i < targetLine.Length; i++) {
					targetLine[i].enabled = true;
				}
				for (int i = 0; i < targetCorner.Length; i++) {
					//targetCorner [i].SetActive (true);
					targetCorner [i].tag = targetCornerTags[i];
				}
				isExist = true;
			}
		}
	}
}

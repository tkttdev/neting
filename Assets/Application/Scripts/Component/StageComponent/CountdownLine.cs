using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CountdownLine : MonoBehaviour {

	[SerializeField] private SpriteRenderer[] targetLine;
	[SerializeField] private GameObject[] targetCorner;
	[SerializeField] private int count = 3;

	private List<GameObject> stayMoveObject = new List<GameObject>();

	void Update(){
		for (int i = 0; i < stayMoveObject.Count; i++) {
			if (!stayMoveObject [i].activeInHierarchy) {
				stayMoveObject.RemoveAt (i);
			}
			CheckCount ();
		}
	}

	private void OnTriggerEnter2D(Collider2D _other){
		stayMoveObject.Add (_other.gameObject);
		if (count > 0) {
			count--;
		}
	}

	private void OnTriggerExit2D(Collider2D _other){
		stayMoveObject.Remove (_other.gameObject);
		CheckCount ();
	}

	private void CheckCount(){
		if (count == 0 && stayMoveObject.Count == 0) {
			for (int i = 0; i < targetLine.Length; i++) {
				targetLine[i].enabled = false;
			}
			for (int i = 0; i < targetCorner.Length; i++) {
				targetCorner [i].SetActive (false);
			}
		}
	}
}

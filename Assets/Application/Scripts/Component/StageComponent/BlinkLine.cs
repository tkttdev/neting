using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BlinkLine : MonoBehaviour {
	[SerializeField] private SpriteRenderer[] targetLine;
	[SerializeField] private GameObject[] targetCorner;

	private List<GameObject> stayMoveObject = new List<GameObject>();
	private bool isExist = true;

	private void OnTriggerEnter2D(Collider2D _other){
		stayMoveObject.Add (_other.gameObject);
	}

	void Update(){
		List<GameObject> removeTarget = new List<GameObject> ();
		foreach (GameObject obj in stayMoveObject) {
			if(GameObject.Find(obj.transform.name) == null){
				removeTarget.Add (obj);
			}
		}

		foreach (GameObject obj in removeTarget) {
			stayMoveObject.Remove (obj);
		}
	}

	private void OnTriggerExit2D(Collider2D _other){
		stayMoveObject.Remove (_other.gameObject);
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

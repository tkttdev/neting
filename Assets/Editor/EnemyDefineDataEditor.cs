using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyDefineData))]
public class EnemyDefineDataEditor : Editor {
	private SerializedProperty enemyDefine;
	private int enemyVarietyCount = 0;
	private List<bool> enemyFoldOut = new List<bool>();
	private List<GameObject> enemyPrefab =  new List<GameObject>();
	private List<GameObject> formerEnemyPrefab;

	private void OnEnable(){
		enemyDefine = serializedObject.FindProperty ("enemyDefine");
		enemyVarietyCount = enemyDefine.arraySize;
		for (int i = 0; i < enemyVarietyCount; i++) {
			enemyFoldOut.Add (false);
			if (enemyDefine.GetArrayElementAtIndex(i).FindPropertyRelative("PATH").stringValue == "") {
				enemyPrefab.Add (null);
			} else {
				GameObject obj = Resources.Load (enemyDefine.GetArrayElementAtIndex (i).FindPropertyRelative ("PATH").stringValue) as GameObject;
				enemyPrefab.Add (obj);
			}
		}
		formerEnemyPrefab = new List<GameObject> (enemyPrefab);
	}

	public override void OnInspectorGUI () {
		serializedObject.Update ();

		EditorGUI.BeginChangeCheck ();
		enemyVarietyCount = enemyDefine.arraySize;

		for (int i = 0; i < enemyVarietyCount; i++) {
			SerializedProperty tmp = enemyDefine.GetArrayElementAtIndex (i);
			enemyFoldOut [i] = EditorGUILayout.Foldout (enemyFoldOut [i], "ENEMY" + i.ToString ());
			if (enemyFoldOut [i]) {
				EditorGUILayout.BeginHorizontal ();
				if (enemyPrefab [i] != null) {
					Texture t = enemyPrefab [i].GetComponent<SpriteRenderer> ().sprite.texture;
					GUILayout.Box(t, GUILayout.Width(30), GUILayout.Height(30));
				} else {
					GUILayout.Box ("NONE", GUILayout.Width (30), GUILayout.Height (30));
				}
				enemyPrefab[i] = EditorGUILayout.ObjectField (enemyPrefab[i], typeof(Object), false) as GameObject;
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.PropertyField (tmp.FindPropertyRelative ("HP"));
				EditorGUILayout.PropertyField (tmp.FindPropertyRelative ("DAMAGE"));
				EditorGUILayout.PropertyField (tmp.FindPropertyRelative ("MONEY"));
				EditorGUILayout.PropertyField (tmp.FindPropertyRelative ("SPEED"));
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("PATH", GUILayout.Width (130));
				EditorGUILayout.LabelField (tmp.FindPropertyRelative ("PATH").stringValue);
				EditorGUILayout.EndHorizontal ();
				if (GUILayout.Button ("Remove")) {
					enemyPrefab.RemoveAt (i);
					formerEnemyPrefab.RemoveAt (i);
					enemyFoldOut.RemoveAt (i);
					enemyDefine.DeleteArrayElementAtIndex (i);
					i--;
					enemyVarietyCount--;
				}
			}
		}

		if (GUILayout.Button ("Add")) {
			enemyDefine.arraySize = enemyDefine.arraySize + 1;
			enemyFoldOut.Add (false);
			enemyPrefab.Add (null);
			formerEnemyPrefab.Add (null);
		}

		if (GUILayout.Button ("RemoveAll")) {
			enemyDefine.ClearArray ();
			enemyPrefab.Clear ();
			formerEnemyPrefab.Clear ();
			enemyFoldOut.Clear ();
		}

		serializedObject.ApplyModifiedProperties ();

		if (EditorGUI.EndChangeCheck ()) {
			CheckDuplicationPrefab ();
			CheckFormerPrefab ();
			UpdateFormerInfo ();
		}
	}

	private void CheckDuplicationPrefab(){
		for (int i = 0; i < enemyPrefab.Count; i++) {
			if (enemyPrefab [i] != formerEnemyPrefab [i]) {
				GameObject checkTarget = enemyPrefab [i];
				for (int j = 0; j < enemyPrefab.Count; j++) {
					if (j == i) {
						continue;
					}
					if (checkTarget == enemyPrefab [j]) {
						enemyPrefab [i] = null;
					}
				}
			}
		}
	}

	private void CheckFormerPrefab(){
		for (int i = 0; i < enemyPrefab.Count; i++) {
			if (enemyPrefab [i] != formerEnemyPrefab [i]) {
				if (enemyPrefab [i] == null) {
					enemyDefine.GetArrayElementAtIndex (i).FindPropertyRelative ("PATH").stringValue = "";
				} else {
					string path = "Prefabs/Enemy/" + enemyPrefab [i].name;
					enemyDefine.GetArrayElementAtIndex (i).FindPropertyRelative ("PATH").stringValue = path;
				}
			}
		}
		serializedObject.ApplyModifiedProperties ();
	}

	private void UpdateFormerInfo(){
		for (int i = 0; i < enemyPrefab.Count; i++) {
			formerEnemyPrefab [i] = enemyPrefab [i];
		}
	}
}

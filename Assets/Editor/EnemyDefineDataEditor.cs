using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyDefine))]
public class EnemyDefineDataEditor : Editor {
	private SerializedProperty enemyDataProp;
	private SerializedProperty varietyNum;
	private int enemyVarietyCount = 0;
	private List<bool> enemyFoldOut = new List<bool>();
	private List<GameObject> enemyPrefab =  new List<GameObject>();
	private List<GameObject> formerEnemyPrefab;

	private void OnEnable(){
		enemyDataProp = serializedObject.FindProperty ("enemy");
		varietyNum = serializedObject.FindProperty ("varietyNum");
		enemyVarietyCount = enemyDataProp.arraySize;
		for (int i = 0; i < enemyVarietyCount; i++) {
			enemyFoldOut.Add (false);
			if (enemyDataProp.GetArrayElementAtIndex(i).FindPropertyRelative("PATH").stringValue == "") {
				enemyPrefab.Add (null);
			} else {
				GameObject obj = Resources.Load (enemyDataProp.GetArrayElementAtIndex (i).FindPropertyRelative ("PATH").stringValue) as GameObject;
				enemyPrefab.Add (obj);
			}
		}
		formerEnemyPrefab = new List<GameObject> (enemyPrefab);
		varietyNum.intValue = enemyVarietyCount;
	}

	public override void OnInspectorGUI () {
		serializedObject.Update ();

		EditorGUI.BeginChangeCheck ();
		enemyVarietyCount = enemyDataProp.arraySize;

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("VARIETY NUM", GUILayout.Width (130));
		EditorGUILayout.LabelField (varietyNum.intValue.ToString ());
		EditorGUILayout.EndHorizontal ();
		for (int i = 0; i < enemyVarietyCount; i++) {
			SerializedProperty tmp = enemyDataProp.GetArrayElementAtIndex (i);
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
					enemyDataProp.DeleteArrayElementAtIndex (i);
					i--;
					enemyVarietyCount--;
				}
			}
		}

		if (GUILayout.Button ("Add")) {
			enemyDataProp.arraySize = enemyDataProp.arraySize + 1;
			enemyFoldOut.Add (false);
			enemyPrefab.Add (null);
			formerEnemyPrefab.Add (null);
		}

		if (GUILayout.Button ("RemoveAll")) {
			enemyDataProp.ClearArray ();
			enemyPrefab.Clear ();
			formerEnemyPrefab.Clear ();
			enemyFoldOut.Clear ();
		}

		serializedObject.ApplyModifiedProperties ();

		if (EditorGUI.EndChangeCheck ()) {
			CheckDuplicationPrefab ();
			CheckFormerPrefab ();
			UpdateFormerInfo ();
			varietyNum.intValue = enemyPrefab.Count;
			serializedObject.ApplyModifiedProperties ();
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
					enemyDataProp.GetArrayElementAtIndex (i).FindPropertyRelative ("PATH").stringValue = "";
				} else {
					string path = "Prefabs/Enemy/" + enemyPrefab [i].name;
					enemyDataProp.GetArrayElementAtIndex (i).FindPropertyRelative ("PATH").stringValue = path;
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

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyDefine))]
public class EnemyDefineEditor : Editor {
	/*private SerializedProperty enemyStatus;
	private int enemyVarietyCount = 0;
	private List<bool> enemyFoldOut = new List<bool>();
	private GameObject targetObject;

	private void OnEnable(){
		enemyStatus = serializedObject.FindProperty ("enemyStatus");
		enemyVarietyCount = enemyStatus.arraySize;
		targetObject = target as GameObject;
	}

	public override void OnInspectorGUI () {
		//base.OnInspectorGUI ();

		serializedObject.Update ();
		enemyVarietyCount = enemyStatus.arraySize;

		for (int i = 0; i < enemyVarietyCount; i++) {
			SerializedObject statusObj = new SerializedObject (targetObject.GetComponent<EnemyDefine> ().enemyStatus [i]);
			SerializedProperty hp = statusObj.FindProperty ("HP");
			SerializedProperty damage = statusObj.FindProperty ("DAMAGE");
			SerializedProperty money = statusObj.FindProperty ("MONEY");
			SerializedProperty speed = statusObj.FindProperty ("SPEED");
			SerializedProperty path = statusObj.FindProperty ("PATH");
			EditorGUILayout.PropertyField (hp);
			EditorGUILayout.PropertyField (damage);
			EditorGUILayout.PropertyField (money);
			EditorGUILayout.PropertyField (speed);
			EditorGUILayout.PropertyField (path);
		}

		if (GUILayout.Button ("AddEnemyInfo")) {
			enemyStatus.arraySize += 1;
		}
		serializedObject.ApplyModifiedProperties ();
	}*/
}

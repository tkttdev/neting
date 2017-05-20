using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Corner))]
public class CornerEditor : Editor {

	private Transform[] formerPurposeTransform = new Transform[4];
	private Corner corner;
	private SerializedProperty purposeTransformProp;
	private SerializedProperty purposeTransform;
	private SerializedProperty bezerControlTransform;
	private SerializedProperty onlyEnemyProp;
	private SerializedProperty onlyBulletProp;
	private SerializedProperty onlyForwardProp;
	private SerializedProperty isBezerProp;
	private SerializedProperty isBezer;

	void OnEnable(){
		corner = target as Corner;
		for (int i = 0; i < 4; i++) {
			formerPurposeTransform [i] = corner.purposeTransform [i];
		}
		purposeTransformProp = serializedObject.FindProperty ("purposeTransform");
		onlyEnemyProp = serializedObject.FindProperty ("onlyEnemy");
		onlyBulletProp = serializedObject.FindProperty ("onlyBullet");
		onlyForwardProp = serializedObject.FindProperty ("onlyForward");
		isBezerProp = serializedObject.FindProperty("isBezer");
	}

	public override void OnInspectorGUI (){
		serializedObject.Update ();
		EditorGUI.BeginChangeCheck ();
		for (int i = 0; i < 4; i++) {
			isBezer = isBezerProp.GetArrayElementAtIndex (i);
			EditorGUILayout.PropertyField (isBezer);
		}

		for(int i = 0; i < 4; i++){
			isBezer = isBezerProp.GetArrayElementAtIndex (i);
			if (isBezer.boolValue) {
				purposeTransform = purposeTransformProp.GetArrayElementAtIndex (i);
				purposeTransform.objectReferenceValue = null;
				switch (i) {
				case 0:
					bezerControlTransform = serializedObject.FindProperty ("upBezerTransform");
					EditorGUILayout.PropertyField (bezerControlTransform, true);
					break;
				case 1:
					bezerControlTransform = serializedObject.FindProperty ("rightBezerTransform");
					EditorGUILayout.PropertyField (bezerControlTransform, true);
					break;
				case 2:
					bezerControlTransform = serializedObject.FindProperty ("downBezerTransform");
					EditorGUILayout.PropertyField (bezerControlTransform, true);
					break;
				case 3:
					bezerControlTransform = serializedObject.FindProperty ("leftBezerTransform");
					EditorGUILayout.PropertyField (bezerControlTransform, true);
					break;
				}
			} else {
				purposeTransform = purposeTransformProp.GetArrayElementAtIndex (i);
				EditorGUILayout.PropertyField (purposeTransform);
			}
		}

		EditorGUILayout.PropertyField (onlyEnemyProp);
		EditorGUILayout.PropertyField (onlyBulletProp);
		EditorGUILayout.PropertyField (onlyForwardProp);

		if (GUILayout.Button ("ResetAll")) {
			for (int i = 0; i < 4; i++) {
				purposeTransform = purposeTransformProp.GetArrayElementAtIndex (i);
				purposeTransform.objectReferenceValue = null;
			}
		}
		serializedObject.ApplyModifiedProperties ();

		if (EditorGUI.EndChangeCheck ()) {
			CheckTargetCorner ();
			ModifyDuplicateElement ();
			ModifyCornerConnection ();
			ModifyFormerCornerConnection ();
			UpdateFormerInfo ();
			serializedObject.ApplyModifiedProperties ();
		}
			
	}

	private void CheckTargetCorner(){
		for (int i = 0; i < 4; i++) {
			if (corner.purposeTransform [i] == null) {
				continue;
			}
			if (corner.purposeTransform [i] == corner.transform) {
				corner.purposeTransform [i] = null;
			}
		}
	}

	private void ModifyDuplicateElement(){
		bool isDuplication = false;
		int duplicationIndex = -1;
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				if (i == j || corner.purposeTransform[i] == null) {
					continue;
				}
				isDuplication = corner.purposeTransform [i] == corner.purposeTransform [j];
				if (isDuplication) {
					duplicationIndex = i;
					break;
				}
			}
			if (isDuplication) {
				break;
			}
		}

		if (isDuplication) {
			for (int i = 0; i < 4; i++) {
				if (formerPurposeTransform [i] == corner.purposeTransform [duplicationIndex]) {
					corner.purposeTransform[i] = null;
					break;
				}
			}
		}
	}

	private void ModifyCornerConnection(){
		SerializedObject partnerCorner;
		SerializedProperty partnerPurposeTransformProp;
		SerializedProperty partnerPurposeTransform;
		for (int i = 0; i < 4; i++) {
			if (corner.purposeTransform [i] == null) {
				continue;
			}
			partnerCorner = new SerializedObject (corner.purposeTransform [i].gameObject.GetComponent<Corner> ());
			partnerCorner.Update ();
			partnerPurposeTransformProp = partnerCorner.FindProperty ("purposeTransform");
			partnerPurposeTransform = partnerPurposeTransformProp.GetArrayElementAtIndex ((i+2)%4);
			/*if (partnerPurposeTransform.objectReferenceValue != null) {
				Transform purposeOfPartnerTransform = partnerPurposeTransform.objectReferenceValue as Transform;
				SerializedObject obj = new SerializedObject (purposeOfPartnerTransform.gameObject.GetComponent<Corner> ());
				obj.Update ();
				SerializedProperty prop = obj.FindProperty ("purposeTransform").GetArrayElementAtIndex (i);
				prop.objectReferenceValue = null;
				obj.ApplyModifiedProperties ();
				partnerPurposeTransform.objectReferenceValue = corner.transform;
			} else {
				partnerPurposeTransform.objectReferenceValue = corner.transform;
			}*/
			partnerPurposeTransform.objectReferenceValue = corner.transform;
			partnerCorner.ApplyModifiedProperties ();
		}
	}

	private void ModifyFormerCornerConnection () {
		SerializedObject partnerCorner;
		SerializedProperty partnerPurposeTransformProp;
		SerializedProperty partnerPurposeTransform;
		for (int i = 0; i < 4; i++) {
			if (formerPurposeTransform[i] != corner.purposeTransform[i] && corner.purposeTransform[i] == null) {
				//partnerCorner = new SerializedObject (formerPurposeTransform[i].gameObject.GetComponent<Corner> ());
				//partnerCorner.Update ();
				//partnerPurposeTransform = partnerCorner.FindProperty ("purposeTransform").GetArrayElementAtIndex ((i + 2) % 4);
				//partnerPurposeTransform.objectReferenceValue = null;
				//partnerCorner.ApplyModifiedProperties ();
				partnerCorner = new SerializedObject(formerPurposeTransform[i].GetComponent<Corner>());
				partnerCorner.Update ();
				partnerPurposeTransformProp = partnerCorner.FindProperty ("purposeTransform");
				partnerPurposeTransform = partnerPurposeTransformProp.GetArrayElementAtIndex ((i + 2) % 4);
				partnerPurposeTransform.objectReferenceValue = null;
				partnerCorner.ApplyModifiedProperties ();
			}
		}
	}

	private void UpdateFormerInfo(){
		for (int i = 0; i < 4; i++) {
			formerPurposeTransform [i] = corner.purposeTransform [i];
		}
	}
}

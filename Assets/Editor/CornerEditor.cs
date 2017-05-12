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

	void OnEnable(){
		corner = target as Corner;
		for (int i = 0; i < 4; i++) {
			formerPurposeTransform [i] = corner.purposeTransform [i];
		}
		purposeTransformProp = serializedObject.FindProperty ("purposeTransform");
		serializedObject.Update ();
	}

	public override void OnInspectorGUI (){
		serializedObject.Update ();
		EditorGUI.BeginChangeCheck ();

		base.OnInspectorGUI ();
		if (GUILayout.Button ("ResetAll")) {
			for (int i = 0; i < 4; i++) {
				corner.purposeTransform [i] = null;
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
			partnerPurposeTransform.objectReferenceValue = corner.transform;
			partnerCorner.ApplyModifiedProperties ();
		}
	}

	private void ModifyFormerCornerConnection () {
		Corner partnerCorner;
		SerializedProperty partnerPurposeTransformProp;
		SerializedProperty partnerPurposeTransform;
		for (int i = 0; i < 4; i++) {
			if (formerPurposeTransform[i] != corner.purposeTransform[i] && corner.purposeTransform[i] == null) {
				partnerCorner = formerPurposeTransform[i].GetComponent<Corner> ();
				partnerCorner.purposeTransform [(i+2)%4] = null;
			}
		}
	}

	private void UpdateFormerInfo(){
		for (int i = 0; i < 4; i++) {
			formerPurposeTransform [i] = corner.purposeTransform [i];
		}
	}
}

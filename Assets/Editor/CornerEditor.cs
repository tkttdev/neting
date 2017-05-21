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
	private SerializedProperty bezerPointsProp;
	private SerializedProperty bezerPoints;
	private SerializedProperty onlyEnemyProp;
	private SerializedProperty onlyBulletProp;
	private SerializedProperty onlyForwardProp;
	private SerializedProperty isCurveProp;
	private SerializedProperty isCurve;
	private SerializedProperty bezerStartPosProp;
	private SerializedProperty bezerStartPos;
	private SerializedProperty bezerEndPosProp;
	private SerializedProperty bezerEndPos;
	private bool[] curveFoldOut = new bool[4];

	void OnEnable(){
		corner = target as Corner;
		for (int i = 0; i < 4; i++) {
			formerPurposeTransform [i] = corner.purposeTransform [i];
		}
		purposeTransformProp = serializedObject.FindProperty ("purposeTransform");
		onlyEnemyProp = serializedObject.FindProperty ("onlyEnemy");
		onlyBulletProp = serializedObject.FindProperty ("onlyBullet");
		onlyForwardProp = serializedObject.FindProperty ("onlyForward");
		isCurveProp = serializedObject.FindProperty("isCurve");
		bezerPointsProp = serializedObject.FindProperty ("bezerPoints");
		bezerStartPosProp = serializedObject.FindProperty ("bezerStartPos");
		bezerEndPosProp = serializedObject.FindProperty ("bezerEndPos");
	}

	public override void OnInspectorGUI (){
		serializedObject.Update ();
		EditorGUI.BeginChangeCheck ();
		for (int i = 0; i < 4; i++) {
			isCurve = isCurveProp.GetArrayElementAtIndex (i);
			EditorGUILayout.PropertyField (isCurve);
		}

		for(int i = 0; i < 4; i++){
			isCurve = isCurveProp.GetArrayElementAtIndex (i);
			if (isCurve.boolValue) {
				purposeTransformProp.GetArrayElementAtIndex (i).objectReferenceValue = null;
				curveFoldOut[i] = EditorGUILayout.Foldout (curveFoldOut[i], ((MoveDir)i).ToString () + "(Curve)");
				if (curveFoldOut[i]) {
					for (int j = 0; j < 4; j++) {
						bezerPoints = bezerPointsProp.GetArrayElementAtIndex (i * 4 + j);
						EditorGUILayout.PropertyField (bezerPoints);
					}
					bezerStartPos = bezerStartPosProp.GetArrayElementAtIndex (i);
					bezerEndPos = bezerEndPosProp.GetArrayElementAtIndex (i);
					Vector3 tmp = bezerStartPos.vector3Value;
					EditorGUILayout.LabelField (string.Format ("START x : {0} y : {1} z : {2}", tmp.x, tmp.y, tmp.z));
					tmp = bezerEndPos.vector3Value;
					EditorGUILayout.LabelField (string.Format ("END   x : {0} y : {1} z : {2}", tmp.x, tmp.y, tmp.z));
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
			CheckTargetCornerNotOwn ();
			ModifyDuplicateElement ();
			ModifyCornerConnection ();
			ModifyFormerCornerConnection ();
			UpdateFormerInfo ();
			serializedObject.ApplyModifiedProperties ();
		}
			
	}

	private void CheckTargetCornerNotOwn(){
		for (int i = 0; i < 4; i++) {
			purposeTransform = purposeTransformProp.GetArrayElementAtIndex (i);
			if (purposeTransform.objectReferenceValue == null) {
				continue;
			} else if(purposeTransform.objectReferenceValue == corner.transform) {
				purposeTransform.objectReferenceValue = null;
				serializedObject.ApplyModifiedProperties ();
			}

		}
	}

	//TODO : implements via serializedObjet
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
					purposeTransform = purposeTransformProp.GetArrayElementAtIndex (i);
					purposeTransform.objectReferenceValue = null;
					serializedObject.ApplyModifiedProperties ();
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
			if (partnerPurposeTransform.objectReferenceValue != null) {
				Transform purposeOfPartnerTransform = partnerPurposeTransform.objectReferenceValue as Transform;
				if (purposeOfPartnerTransform == corner.transform) {
					continue;
				}
				//Debug.Log (purposeOfPartnerTransform.gameObject);
				//Debug.Log ((MoveDir)(((i + 2) % 4 + 2) % 4));
				SerializedObject obj = new SerializedObject (purposeOfPartnerTransform.gameObject.GetComponent<Corner> ());
				obj.Update ();
				SerializedProperty prop = obj.FindProperty ("purposeTransform").GetArrayElementAtIndex (i);
				prop.objectReferenceValue = null;
				obj.ApplyModifiedProperties ();
			} else {
				partnerPurposeTransform.objectReferenceValue = corner.transform;
			}
			partnerPurposeTransform.objectReferenceValue = corner.transform;
			partnerCorner.ApplyModifiedProperties ();
		}
	}

	private void ModifyFormerCornerConnection () {
		SerializedObject partnerCorner;
		SerializedProperty partnerPurposeTransformProp;
		SerializedProperty partnerPurposeTransform;
		for (int i = 0; i < 4; i++) {
			if (formerPurposeTransform[i] != corner.purposeTransform[i]) {
				if (formerPurposeTransform [i] == null) {
					continue;
				}
				partnerCorner = new SerializedObject (formerPurposeTransform [i].gameObject.GetComponent<Corner> ());
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
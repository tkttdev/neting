using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Corner))]
public class CornerEditor : Editor {

	private Transform[] formerPurposeTransform = new Transform[4];
	private Transform[] formerCurvePurpose = new Transform[4];
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
	private bool[] curveFoldOut = new bool[4];
		
	protected virtual void OnEnable(){
		corner = target as Corner;
		for (int i = 0; i < 4; i++) {
			formerPurposeTransform [i] = corner.purposeTransform [i];
			formerCurvePurpose [i] = corner.bezerPoints [i * 4 + 3];
		}
		purposeTransformProp = serializedObject.FindProperty ("purposeTransform");
		onlyEnemyProp = serializedObject.FindProperty ("onlyEnemy");
		onlyBulletProp = serializedObject.FindProperty ("onlyBullet");
		onlyForwardProp = serializedObject.FindProperty ("onlyForward");
		isCurveProp = serializedObject.FindProperty("isCurve");
		bezerPointsProp = serializedObject.FindProperty ("bezerPoints");
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
				//purposeTransformProp.GetArrayElementAtIndex (i).objectReferenceValue = null;
				curveFoldOut[i] = EditorGUILayout.Foldout (curveFoldOut[i], ((MoveDir)i).ToString () + "(Curve)");
				if (curveFoldOut[i]) {
					for (int j = 0; j < 4; j++) {
						bezerPoints = bezerPointsProp.GetArrayElementAtIndex (i * 4 + j);
						EditorGUILayout.PropertyField (bezerPoints);
					}
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
				isCurve = isCurveProp.GetArrayElementAtIndex (i);
				isCurve.boolValue = false;
				for (int j = 0; j < 4; j++) {
					bezerPoints = bezerPointsProp.GetArrayElementAtIndex (i * 4 + j);
					bezerPoints.objectReferenceValue = null;
				}
			}
		}

		serializedObject.ApplyModifiedProperties ();

		if (EditorGUI.EndChangeCheck ()) {
			RemoveOwnByTargetCorner ();
			ModifyDuplicateElement ();
			ModifyCornerConnection ();
			ModifyFormerCornerConnection ();
			ModifyCurveConnection ();
			ModifyFormerCurveConnection ();
			UpdateFormerInfo ();
			serializedObject.ApplyModifiedProperties ();
		}
	}

	private void RemoveOwnByTargetCorner(){
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
			if (corner.purposeTransform [i] == null || corner.purposeTransform [i].gameObject.GetComponent<Corner> () == null) {
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
				if (formerPurposeTransform [i] == null || formerPurposeTransform [i].gameObject.GetComponent<Corner> () == null) {
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

	private void ModifyCurveConnection () {
		SerializedObject partnerCorner;
		SerializedProperty partnerIsCurveProp;
		SerializedProperty partnerIsCurve;
		SerializedProperty partnerBezerPointsProp;
		SerializedProperty partnerBezerPoints;
		for (int i = 0; i < 4; i++) {
			if (bezerPointsProp.GetArrayElementAtIndex (i * 4 + 3).objectReferenceValue == null) {
				continue;
			}
			Corner c = corner.bezerPoints [i * 4 + 3].gameObject.GetComponent<Corner> ();
			if (c == null) {
				continue;
			}
			partnerCorner = new SerializedObject (c);
			partnerCorner.Update ();
			partnerIsCurveProp = partnerCorner.FindProperty ("isCurve");
			partnerIsCurve = partnerIsCurveProp.GetArrayElementAtIndex ((i + 2) % 4);
			partnerIsCurve.boolValue = isCurveProp.GetArrayElementAtIndex(i).boolValue;
			partnerBezerPointsProp = partnerCorner.FindProperty ("bezerPoints");
			for (int j = 0; j < 4; j++) {
				partnerBezerPoints = partnerBezerPointsProp.GetArrayElementAtIndex ((i + 2) % 4 * 4 + 3 - j);
				partnerBezerPoints.objectReferenceValue = bezerPointsProp.GetArrayElementAtIndex (i * 4 + j).objectReferenceValue;
			}
			partnerCorner.ApplyModifiedProperties ();
		}
	}

	private void ModifyFormerCurveConnection(){
		SerializedObject formerPartnerCorner;
		SerializedProperty formerPartnerIsCurveProp;
		SerializedProperty formerPartnerIsCurve;
		SerializedProperty formerPartnerBezerPointsProp;
		SerializedProperty formerPartnerBezerPoints;
		for (int i = 0; i < 4; i++) {
			if (bezerPointsProp.GetArrayElementAtIndex (i * 4 + 3).objectReferenceValue == formerCurvePurpose[i]) {
				continue;
			}
			if (formerCurvePurpose [i] == null) {
				return;
			}
			Corner c = formerCurvePurpose[i].gameObject.GetComponent<Corner> ();
			if (c == null) {
				continue;
			}
			formerPartnerCorner = new SerializedObject (c);
			formerPartnerCorner.Update ();
			formerPartnerIsCurveProp = formerPartnerCorner.FindProperty ("isCurve");
			formerPartnerIsCurve = formerPartnerIsCurveProp.GetArrayElementAtIndex ((i + 2) % 4);
			formerPartnerIsCurve.boolValue = false;
			formerPartnerBezerPointsProp = formerPartnerCorner.FindProperty ("bezerPoints");
			for (int j = 0; j < 4; j++) {
				formerPartnerBezerPoints = formerPartnerBezerPointsProp.GetArrayElementAtIndex (((i + 2) % 4) * 4 + j);
				formerPartnerBezerPoints.objectReferenceValue = null;
			}
			formerPartnerCorner.ApplyModifiedProperties ();
		}
	}

	private void UpdateFormerInfo(){
		for (int i = 0; i < 4; i++) {
			formerPurposeTransform [i] = corner.purposeTransform [i];
			formerCurvePurpose [i] = corner.bezerPoints [i * 4 + 3];
		}
	}
}
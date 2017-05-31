using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Warp))]
public class WarpEditor : CornerEditor {
	private SerializedProperty warpPurposeProp;

	private Transform beforePurpose;
	private Warp targetWarp;

	protected override void OnEnable () {
		warpPurposeProp = serializedObject.FindProperty ("warpPurpose");
		targetWarp = target as Warp;
		beforePurpose = targetWarp.warpPurpose;
		base.OnEnable ();
	}

	public override void OnInspectorGUI () {
		serializedObject.Update ();
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (warpPurposeProp);
		serializedObject.ApplyModifiedProperties ();
		base.OnInspectorGUI ();
		if (EditorGUI.EndChangeCheck ()) {
			//ModifyConnectionOfWarp ();
			//UpdateBeforeInfo ();
		}

	}

	private void ModifyConnectionOfWarp(){
		/*if (warpPurposeProp.objectReferenceValue == beforePurpose) {
			return;
		}
		SerializedObject beforePurpose = new SerializedObject (beforePurpose.gameObject.GetComponent<Warp> ());
		SerializedProperty partnerWarpPurposeProp;
		beforePurpose.Update ();
		partnerWarpPurposeProp = beforePurpose.FindProperty ("warpPurpose");
		partnerWarpPurposeProp.objectReferenceValue = null;
		beforePurpose.ApplyModifiedProperties ();

		SerializedObject newlyPurpose = new SerializedObject (targetWarp.warpPurpose.gameObject.GetComponent<Warp> ());
		newlyPurpose.Update ();
		partnerWarpPurposeProp = newlyPurpose.FindProperty ("warpPurpose");
		partnerWarpPurposeProp.objectReferenceValue = targetWarp.gameObject.transform;
		newlyPurpose.ApplyModifiedProperties ();*/
	}

	private void UpdateBeforeInfo(){
		//beforePurpose = targetWarp.warpPurpose;
	}
}

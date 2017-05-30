using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Warp))]
public class WarpEditor : CornerEditor {
	private SerializedProperty warpPurposeProp;

	protected override void OnEnable () {
		warpPurposeProp = serializedObject.FindProperty ("warpPurpose");
		base.OnEnable ();
	}

	public override void OnInspectorGUI () {
		serializedObject.Update ();
		EditorGUILayout.PropertyField (warpPurposeProp);
		serializedObject.ApplyModifiedProperties ();
		base.OnInspectorGUI ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Corner))]
public class CornerEditor : Editor {

	public override void OnInspectorGUI (){
		base.OnInspectorGUI ();
		Corner corner = target as Corner;
		if (GUILayout.Button ("SetCorner")) {
			corner.SetCorner ();
		}


		serializedObject.Update ();

		serializedObject.ApplyModifiedProperties ();

	}
}

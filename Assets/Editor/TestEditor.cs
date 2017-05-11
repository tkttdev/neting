using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor {
	//static List<Test> tests = new List<Test> ();
	//private Test test;
	//SerializedProperty fields;
	SerializedProperty xxx;

	void Enable(){
		//tests.Add ((Test)target);
		//test = (Test)target;
		//fields = serializedObject.FindProperty ("fields");
		xxx = serializedObject.FindProperty ("xxx");
	}

	void OnInspectorGUI(){
		base.OnInspectorGUI ();
		EditorGUILayout.LabelField ("a", xxx.ToString());
		//serializedObject.Update ();
		//EditorGUILayout.IntSlider (xxx, 0, 100);
		//serializedObject.ApplyModifiedProperties ();
	}
}

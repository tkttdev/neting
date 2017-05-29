using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TestEditor : EditorWindow {

	private string[] testString = new string[] {"a", "b", "c", "d", "e", "f", "g", "h", "i"};

	[MenuItem("Window/Test")]
	static void Open(){
		var window = GetWindow<TestEditor> ();
		window.maxSize = window.minSize = new Vector2 (600, 600);
	}

	int index = 0;
	Vector2 scroll;

	void OnGUI () {
		//EditorGUILayout.RectField (500f);
		EditorGUILayout.BeginHorizontal ();

		EditorGUILayout.BeginVertical (GUILayout.MinWidth (500));
		if (GUILayout.Button ("TEST", GUILayout.ExpandWidth(false))) {
			StreamWriter sw;
			sw = new StreamWriter (Application.dataPath + "/Test.csv");
			sw.WriteLine ("a, b, c");
			sw.Write ("a, b, c");
			sw.Flush();
			sw.Close();
		}
		EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginVertical ();
		scroll = EditorGUILayout.BeginScrollView(scroll);
		for(int i = 0; i < testString.Length; i++){
			bool flag = GUILayout.Toggle(index == i, testString[i], "OL Elem");
			if (flag != (index == i)) {
				index = i;
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical ();

		EditorGUILayout.EndHorizontal ();
	}
}

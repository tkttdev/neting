using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EnemySpawnEditor : EditorWindow {

	private string[] testString = new string[] {"a", "b", "c", "d", "e", "f", "g", "h", "i"};
	private static EnemyDefine enemyDefine;
	private GameObject targetEnemy;
	private int index = -1;
	private Vector2 scroll;
	private TextAsset stageCsv;
	private Editor enemyEditor;

	[MenuItem("Window/EnemySpawnEdit")]
	static void Open(){
		enemyDefine = Resources.Load ("ScriptableObjects/EnemyDefineData") as EnemyDefine;
		var window = GetWindow<EnemySpawnEditor> ();
		window.maxSize = window.minSize = new Vector2 (700, 700);
	}

	void OnGUI () {
		var curEvent = Event.current;
		stageCsv = EditorGUILayout.ObjectField ("EnemySpawnCsv", stageCsv, typeof(TextAsset), false) as TextAsset;
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

		if (enemyDefine != null) {
			for (int i = 0; i < enemyDefine.varietyNum; i++) {
				bool flag = GUILayout.Toggle (index == i, "ENEMY" + i.ToString (), "OL Elem");
				if (flag != (index == i)) {
					index = i;
					targetEnemy = Resources.Load (enemyDefine.enemy [index].PATH) as GameObject;
				}
			}
		}

		if (targetEnemy != null) {
			Texture t = targetEnemy.GetComponent<SpriteRenderer> ().sprite.texture;
			GUILayout.Box(t, GUILayout.Width(50), GUILayout.Height(50));
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical ();

		EditorGUILayout.EndHorizontal ();
	}
}

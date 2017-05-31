using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EnemySpawnEditor : EditorWindow {

	private Texture[] enemyTextures;
	private static EnemyDefine enemyDefine;
	private GameObject targetEnemy;
	private int targetEnemyIndex = 0;
	private Vector2 scroll;
	private TextAsset stageCsv;
	private Editor enemyEditor;
	private Event curEvent;
	private List<Vector2> displayEnemyPos = new List<Vector2>();
	private List<int> displayEnemyIndex = new List<int>();
	private enum EditMode : int {
		NOT_SELECT = 0,
		SELECT_OBJ = 1,
	}
	EditMode editMode = EditMode.NOT_SELECT;

	[MenuItem("Window/EnemySpawnEdit")]
	static void Open(){
		enemyDefine = Resources.Load ("ScriptableObjects/EnemyDefineData") as EnemyDefine;
		var window = GetWindow<EnemySpawnEditor> ();
		window.maxSize = window.minSize = new Vector2 (700, 700);
	}

	void OnEnable(){
		targetEnemy = Resources.Load (enemyDefine.enemy [targetEnemyIndex].PATH) as GameObject;
		enemyTextures = new Texture2D[enemyDefine.varietyNum];
	}

	void OnGUI () {
		curEvent = Event.current;
		wantsMouseMove = true;
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
		if (curEvent.type == EventType.MouseDown) {
			if (curEvent.mousePosition.x <= 490) {
				displayEnemyPos.Add (curEvent.mousePosition);
				displayEnemyIndex.Add (targetEnemyIndex);
				Repaint ();
			}
		}
		for (int i = 0; i < displayEnemyPos.Count; i++) {
			GUI.Box (new Rect (displayEnemyPos [i].x - 10, displayEnemyPos [i].y - 10, 20, 20), enemyTextures [displayEnemyIndex[i]]);
		}
		EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginVertical ();
		scroll = EditorGUILayout.BeginScrollView(scroll);

		DisplayEnemyList ();
		CheckKeyMoveAtEnemyList ();
		DisplayTargetEnemy ();


		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();
	}

	private void DisplayEnemyList(){
		if (enemyDefine != null) {
			for (int i = 0; i < enemyDefine.varietyNum; i++) {
				bool flag = GUILayout.Toggle (targetEnemyIndex == i, "ENEMY" + i.ToString (), "OL Elem");
				if (flag != (targetEnemyIndex == i)) {
					if (targetEnemyIndex == i) {
						targetEnemyIndex = - 1;
						targetEnemy = null;
					} else {
						targetEnemyIndex = i;
						targetEnemy = Resources.Load (enemyDefine.enemy [targetEnemyIndex].PATH) as GameObject;
					}
				}
			}
		}
	}

	private void CheckKeyMoveAtEnemyList(){
		if (targetEnemyIndex < 0 || targetEnemyIndex > enemyDefine.varietyNum - 1) {
			return;
		}
		if (curEvent.type == EventType.KeyDown && curEvent.keyCode == KeyCode.DownArrow) {
			targetEnemyIndex = targetEnemyIndex < enemyDefine.varietyNum - 1 ? targetEnemyIndex + 1 : targetEnemyIndex;
			targetEnemy = Resources.Load (enemyDefine.enemy [targetEnemyIndex].PATH) as GameObject;
			Repaint ();
		} else if (curEvent.type == EventType.KeyDown && curEvent.keyCode == KeyCode.UpArrow) {
			targetEnemyIndex = targetEnemyIndex > 0 ? targetEnemyIndex - 1 : targetEnemyIndex;
			targetEnemy = Resources.Load (enemyDefine.enemy [targetEnemyIndex].PATH) as GameObject;
			Repaint ();
		}
	}

	private void DisplayTargetEnemy(){
		if (targetEnemy != null) {
			if (enemyTextures [targetEnemyIndex] == null) {
				Texture t = targetEnemy.GetComponent<SpriteRenderer> ().sprite.texture;
				enemyTextures [targetEnemyIndex] = t;
			} 
			GUILayout.Box (enemyTextures [targetEnemyIndex], GUILayout.Width (30), GUILayout.Height (30));
		}
	}
}

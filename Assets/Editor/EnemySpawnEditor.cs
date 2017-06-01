using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EnemySpawnEditor : EditorWindow {

	private Texture[] enemyTextures;
	private static EnemyDefine enemyDefine;
	private GameObject placeTargetEnemy;
	private int placeTargetEnemyId = 0;
	private int replaceTargetEnemyId = 0;
	private Vector2 scroll;
	private TextAsset stageCsv;
	private Editor enemyEditor;
	private Event curEvent;
	private List<Vector2> placedEnemyPos = new List<Vector2>();
	private List<int> placedEnemyid = new List<int>();
	private enum EditMode : int {
		NONE = 0,
		PLACE = 1,
		REPLACE = 2,
	}
	EditMode editMode = EditMode.NONE;

	[MenuItem("Window/EnemySpawnEdit")]
	static void Open(){
		enemyDefine = Resources.Load ("ScriptableObjects/EnemyDefineData") as EnemyDefine;
		var window = GetWindow<EnemySpawnEditor> ();
		window.maxSize = window.minSize = new Vector2 (700, 700);
	}

	void OnEnable(){
		placeTargetEnemy = Resources.Load (enemyDefine.enemy [placeTargetEnemyId].PATH) as GameObject;
		enemyTextures = new Texture[enemyDefine.varietyNum];
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
		if (editMode == EditMode.NONE) {
		
		} else if (editMode == EditMode.PLACE) {
			if (curEvent.type == EventType.MouseMove) {
				if (curEvent.mousePosition.x <= 490) {
					DisplayEnemyAtPos (curEvent.mousePosition, placeTargetEnemyId);
					Repaint ();
				}
			}
			if (curEvent.type == EventType.MouseDown) {
				if (curEvent.mousePosition.x <= 490) {
					placedEnemyPos.Add (curEvent.mousePosition);
					placedEnemyid.Add (placeTargetEnemyId);
					Repaint ();
				}
			}
		} else if (editMode == EditMode.REPLACE) {
		
		}

		DisplayPlacedEnemy ();

		EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginVertical ();
		scroll = EditorGUILayout.BeginScrollView(scroll);

		DisplayEnemyList ();
		UpdateMoveOnEnemyList ();
		DisplayTargetEnemy ();


		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();

		editMode = SetNowEditMode ();
	}

	private void DisplayEnemyAtPos(Vector2 _pos, int _id){
		GUI.Box (new Rect (_pos.x - 10, _pos.y - 10, 20, 20), enemyTextures [_id]);
	}

	private void DisplayPlacedEnemy(){
		for (int i = 0; i < placedEnemyPos.Count; i++) {
			GUI.Box (new Rect (placedEnemyPos [i].x - 10, placedEnemyPos [i].y - 10, 20, 20), enemyTextures [placedEnemyid[i]]);
		}
	}

	private void DisplayEnemyList(){
		if (enemyDefine != null) {
			for (int i = 0; i < enemyDefine.varietyNum; i++) {
				bool flag = GUILayout.Toggle (placeTargetEnemyId == i, "ENEMY" + i.ToString (), "OL Elem");
				if (flag != (placeTargetEnemyId == i)) {
					if (placeTargetEnemyId == i) {
						placeTargetEnemyId = - 1;
						placeTargetEnemy = null;
					} else {
						placeTargetEnemyId = i;
						placeTargetEnemy = Resources.Load (enemyDefine.enemy [placeTargetEnemyId].PATH) as GameObject;
					}
				}
			}
		}
	}

	private void UpdateMoveOnEnemyList(){
		if (placeTargetEnemyId < 0 || placeTargetEnemyId > enemyDefine.varietyNum - 1) {
			return;
		}
		if (curEvent.type == EventType.KeyDown && curEvent.keyCode == KeyCode.DownArrow) {
			placeTargetEnemyId = placeTargetEnemyId < enemyDefine.varietyNum - 1 ? placeTargetEnemyId + 1 : placeTargetEnemyId;
			placeTargetEnemy = Resources.Load (enemyDefine.enemy [placeTargetEnemyId].PATH) as GameObject;
			Repaint ();
		} else if (curEvent.type == EventType.KeyDown && curEvent.keyCode == KeyCode.UpArrow) {
			placeTargetEnemyId = placeTargetEnemyId > 0 ? placeTargetEnemyId - 1 : placeTargetEnemyId;
			placeTargetEnemy = Resources.Load (enemyDefine.enemy [placeTargetEnemyId].PATH) as GameObject;
			Repaint ();
		}
	}

	private void DisplayTargetEnemy(){
		if (placeTargetEnemy != null) {
			if (enemyTextures [placeTargetEnemyId] == null) {
				Texture t = placeTargetEnemy.GetComponent<SpriteRenderer> ().sprite.texture;
				enemyTextures [placeTargetEnemyId] = t;
			} 
			GUILayout.Box (enemyTextures [placeTargetEnemyId], GUILayout.Width (30), GUILayout.Height (30));
		}
	}

	private EditMode SetNowEditMode(){
		if (editMode == EditMode.REPLACE) {
			return EditMode.REPLACE;
		}
		if (placeTargetEnemyId < 0 || placeTargetEnemyId > enemyDefine.varietyNum - 1) {
			return EditMode.NONE;
		}
		return EditMode.PLACE;
	}
}

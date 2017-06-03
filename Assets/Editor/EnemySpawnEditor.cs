using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EnemySpawnEditor : EditorWindow {

	private static List<List<int>> placedEnemySpawnLineIndex = new List<List<int>> ();
	private static List<List<int>> placedEnemyId = new List<List<int>>();
	private static List<List<float>> placedEnemySpawnTime = new List<List<float>>();

	private Texture2D[] enemyTextures;
	private Texture2D[] moveEnemyTextures;
	private bool[] useStageLine = new bool[5]{ true, true, true, true, true };
	private static EnemyDefine enemyDefine;
	private GameObject placeTargetEnemy;
	private int placeTargetEnemyId = 0;
	private int replaceTargetEnemyId = 0;
	private float enemySpawnTime = 0f;
	private int enemySpawnLineIndex = 0;
	private Vector2 enemyListScroll;
	private TextAsset stageCsv;
	private Editor enemyEditor;
	private Event curEvent;
	private float enemyPlaceAreaWidth = 500;
	private float placeEnemyTextureWidth = 20;
	private float placeEnemyTextureHeight = 20;
	private bool isEdited = false;
	private TextAsset beforeStageCsv = null;
	private int spawnVarietyNum = 1;
	private int spawnVarietyIndex = 0;
	private float editStartTime = 0f;
	private float stageSliderValue = 0f;

	private const string directoryPath = "Assets/Application/Resources/SpawnInfoCsv";

	private enum EditMode : int {
		NONE = 0,
		PLACE = 1,
		REPLACE = 2,
	}
	EditMode editMode = EditMode.NONE;
	public static EditorWindow window;
	[MenuItem("Window/EnemySpawnEdit")]
	static void Open(){
		enemyDefine = Resources.Load ("ScriptableObjects/EnemyDefineData") as EnemyDefine;
		for (int i = 0; i < 5; i++) {
			placedEnemyId.Add (new List<int> ());
			placedEnemySpawnLineIndex.Add (new List<int> ());
			placedEnemySpawnTime.Add (new List<float> ());
		}
		window = GetWindow<EnemySpawnEditor> ();
		window.maxSize = window.minSize = new Vector2 (700, 700);
	}

	void OnEnable(){
		placeTargetEnemy = Resources.Load (enemyDefine.enemy [placeTargetEnemyId].PATH) as GameObject;
		enemyTextures = new Texture2D[enemyDefine.varietyNum];
		moveEnemyTextures = new Texture2D[enemyDefine.varietyNum];
	}

	void OnGUI () {
		curEvent = Event.current;
		wantsMouseMove = true;

		stageCsv = EditorGUILayout.ObjectField ("EnemySpawnCsv", stageCsv, typeof(TextAsset), false) as TextAsset;

		/*
		GUILayout.BeginArea (new Rect (400, 400, 100, 100));
		GUILayout.Label ("a");
		GUILayout.EndArea ();
		*/

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical (GUILayout.MinWidth (enemyPlaceAreaWidth), GUILayout.MaxWidth (enemyPlaceAreaWidth));
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.BeginHorizontal ();
		spawnVarietyNum = EditorGUILayout.IntField ("Spawn Variety : ", spawnVarietyNum);
		spawnVarietyNum = Mathf.Clamp (spawnVarietyNum, 1, 5);
		editStartTime = EditorGUILayout.FloatField ("Edit Start Time : ", editStartTime);
		editStartTime = Mathf.Clamp (editStartTime, 0, 70);

		EditorGUILayout.EndHorizontal ();
		if (EditorGUI.EndChangeCheck ()) {
			spawnVarietyIndex = Mathf.Clamp (spawnVarietyIndex, 0, spawnVarietyNum - 1);
		}
		EditorGUILayout.BeginHorizontal (GUILayout.MinWidth (enemyPlaceAreaWidth));
		for(int i = 0; i < spawnVarietyNum; i++){
			string toggleName = "Ver" + (i + 1).ToString ();
			bool flag = GUILayout.Toggle (spawnVarietyIndex == i, toggleName);
			if (flag != (spawnVarietyIndex == i)) {
				spawnVarietyIndex = i;
			}
		}
		EditorGUILayout.EndHorizontal ();
		//Begin EnemyPlaceArea
		GUI.Box (new Rect (30, 65, 450, 600), "");
		GUILayout.BeginArea(new Rect (30, 65, 450, 600));
		DrawStage ();



		if (editMode == EditMode.NONE) {
			if (curEvent.type == EventType.MouseDown) {
				if (curEvent.mousePosition.x <= enemyPlaceAreaWidth - 10) {
					int index = GetPlacedEnemyListIndexAtPos (curEvent.mousePosition);
					if (index > -1) {
						replaceTargetEnemyId = placedEnemyId[spawnVarietyIndex][index];
						placedEnemyId[spawnVarietyIndex].RemoveAt (index);
						placedEnemySpawnTime[spawnVarietyIndex].RemoveAt (index);
						placedEnemySpawnLineIndex[spawnVarietyIndex].RemoveAt (index);
						editMode = EditMode.REPLACE;
						Repaint ();
					}
				}
			}
		} else if (editMode == EditMode.PLACE) {
			if (curEvent.mousePosition.x <= 490 && curEvent.mousePosition.y >= 30 && curEvent.mousePosition.y <= 580) {
				enemySpawnTime = culcSpawnTime (curEvent.mousePosition);
				enemySpawnLineIndex = culcSpawnLineIndex (curEvent.mousePosition);
				DisplayEnemyAtPos (curEvent.mousePosition, placeTargetEnemyId);
				DrawRedPointOnNearestLine (curEvent.mousePosition);
			}
			if (curEvent.type == EventType.MouseDown) {
				if (curEvent.mousePosition.x <= 490 && curEvent.mousePosition.y >= 30 && curEvent.mousePosition.y <= 580) {
					PlaceEnemy (placeTargetEnemyId, enemySpawnTime, enemySpawnLineIndex);
				}
			}
			if (curEvent.type == EventType.KeyDown && curEvent.keyCode == KeyCode.Backspace) {
				SetEditModeNone ();
			}
			if (curEvent.type == EventType.ScrollWheel) {
				MoveStageByScroll (curEvent.delta.y);
			}
		} else if (editMode == EditMode.REPLACE) {
			if (curEvent.mousePosition.x <= 490 && curEvent.mousePosition.y >= 30 && curEvent.mousePosition.y <= 580) {
				enemySpawnTime = culcSpawnTime (curEvent.mousePosition);
				enemySpawnLineIndex = culcSpawnLineIndex (curEvent.mousePosition);
				DisplayEnemyAtPos (curEvent.mousePosition, replaceTargetEnemyId);
				DrawRedPointOnNearestLine (curEvent.mousePosition);
			}
			if (curEvent.type == EventType.MouseDown && curEvent.mousePosition.y >= 30 && curEvent.mousePosition.y <= 580) {
				if (curEvent.mousePosition.x <= 490) {
					editMode = EditMode.NONE;
					PlaceEnemy (replaceTargetEnemyId, enemySpawnTime, enemySpawnLineIndex);
				}
			}
			if (curEvent.type == EventType.KeyDown && curEvent.keyCode == KeyCode.Backspace) {
				SetEditModeNone ();
			}
			if (curEvent.type == EventType.ScrollWheel) {
				MoveStageByScroll (curEvent.delta.y);
			}
		}

		DisplayPlacedEnemy ();
		GUILayout.EndArea ();
		EditorGUILayout.EndVertical ();
		//End EnemyPlaceArea

		//Begin EnemyListArea
		EditorGUILayout.BeginVertical ();
		enemyListScroll = EditorGUILayout.BeginScrollView (enemyListScroll, GUILayout.MaxHeight (300));

		DisplayEnemyList ();
		UpdateMoveOnEnemyList ();

		EditorGUILayout.EndScrollView ();
		DisplayTargetEnemy ();
		if (GUI.Button(new Rect(620,670,75,25), "SAVE")) {
		}
		EditorGUILayout.EndVertical ();
		//End EnemyListArea

		EditorGUILayout.EndHorizontal ();

		editMode = GetNowEditMode ();
	}

	private void OnFocus(){
		if (stageCsv != beforeStageCsv && isEdited) {
			Debug.Log ("編集内容が保存されていません");

		} else if(stageCsv != beforeStageCsv && !isEdited) {
			Debug.Log ("新たなステージcsvをロードします");
		}
	}

	private void OnLostFocus(){
		beforeStageCsv = stageCsv;
	}

	private void DrawStage(){
		GUILayout.BeginHorizontal ();
		for (int i = 0; i < 5; i++) {
			useStageLine[i] = GUI.Toggle (new Rect (80 * i + 63, 5, 10, 10), useStageLine[i], "");
			if (useStageLine [i]) {
				EditorGUI.DrawRect (new Rect (80 * i + 70, 30, 2, 550), Color.black);
			}
		}
		for (int i = 0; i < 7; i++) {
			EditorGUI.LabelField (new Rect (35, 572 - 550 / 6 * i, 30, 30), (i * 5 + editStartTime).ToString ());
		}
		GUILayout.EndHorizontal ();
	}


	/// <summary>
	/// if doesn't exist enemy at pos, return -1
	/// else this func return #INDEX OF PLACED ENEMY LIST#
	/// </summary>
	/// <param name="_pos">Position.</param>
	private int GetPlacedEnemyListIndexAtPos(Vector2 _pos){
		for (int i = 0; i < placedEnemyId[spawnVarietyIndex].Count; i++) {
			float x = culcDrawPosX(placedEnemySpawnLineIndex[spawnVarietyIndex][i]);
			float y = culcDrawPosY (placedEnemySpawnTime [spawnVarietyIndex][i]);
			if (_pos.x >= x - 9 && _pos.x <= x + 9 && _pos.y >= y - 9 && _pos.y <= y + 9) {
				return i;
			}
		}
		return -1;
	}

	private float culcSpawnTime(Vector2 _pos){
		float y = _pos.y;
		return ((1f - ((y - 30f)/ 550f)) * 30f + editStartTime);
	}

	private int culcSpawnLineIndex(Vector2 _pos){
		float drawX = _pos.x;
		int index = 0;
		if (drawX <= 70) {
			index = 0;
		} else if (drawX >= 390) {
			index = 4;
		} else {
			int lowIndex = (int)(drawX - 70) / 80;
			float dis = (drawX - 70) - lowIndex * 80;
			index = dis < 40 ? lowIndex : lowIndex + 1;
		}
		return index;
	}

	private float culcDrawPosY(float _time){
		return (1f - (_time - editStartTime) / 30f) * 550f + 30f;
	}

	private float culcDrawPosX(int _index){
		return _index * 80 + 70;
	}

	private void PlaceEnemy(int _id, float _spawnTime, int _lineIndex){
		//placedEnemyPos.Add (_pos);
		if (!useStageLine [_lineIndex]) {
			return;
		}
		placedEnemyId[spawnVarietyIndex].Add (_id);
		placedEnemySpawnTime[spawnVarietyIndex].Add (_spawnTime);
		placedEnemySpawnLineIndex[spawnVarietyIndex].Add (_lineIndex);
		isEdited = true;
		Repaint ();
	}

	private void DisplayEnemyAtPos(Vector2 _pos, int _id){
		GUI.Box (new Rect (_pos.x - 9, _pos.y - 9, 18, 18), enemyTextures[_id]);
		GUI.Label (new Rect (_pos.x - 15, _pos.y + 13, 30, 18), string.Format("{0:f1}", enemySpawnTime));
		Repaint ();
	}

	private void MoveStageByScroll(float _deltaY){
		editStartTime += _deltaY * 2f;
		Repaint ();
	}

	private void DrawRedPointOnNearestLine(Vector2 _pos){
		int index = culcSpawnLineIndex (_pos);
		if (!useStageLine [index]) {
			return;
		}
		float drawX = culcDrawPosX (index);
		EditorGUI.DrawRect (new Rect (drawX - 1f, _pos.y - 1f, 4, 4), Color.red);
	}

	private void DisplayPlacedEnemy(){
		for (int i = 0; i < placedEnemyId[spawnVarietyIndex].Count; i++) {
			if (placedEnemySpawnTime [spawnVarietyIndex][i] < editStartTime || !useStageLine[placedEnemySpawnLineIndex[spawnVarietyIndex][i]]) {
				continue;
			}
			float x = culcDrawPosX (placedEnemySpawnLineIndex[spawnVarietyIndex] [i]);
			float y = culcDrawPosY (placedEnemySpawnTime[spawnVarietyIndex] [i]);
			if (y >= 30f) {
				GUI.Box (new Rect (x - 9, y - 9, 18, 18), enemyTextures [placedEnemyId[spawnVarietyIndex] [i]]);
			}
		}
	}

	private void SetEditModeNone(){
		editMode = EditMode.NONE;
		replaceTargetEnemyId = -1;
		placeTargetEnemyId = -1;
		placeTargetEnemy = null;
		Repaint ();
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
						editMode = EditMode.PLACE;
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
				Texture2D t = placeTargetEnemy.GetComponent<SpriteRenderer> ().sprite.texture;
				enemyTextures [placeTargetEnemyId] = t;
			} 
			//GUILayout.Box (enemyTextures [placeTargetEnemyId], GUILayout.Width (30), GUILayout.Height (30));
			EditorGUI.DrawTextureTransparent(new Rect(550,400,50,50), enemyTextures [placeTargetEnemyId]);
		}
	}

	private EditMode GetNowEditMode(){
		if (editMode == EditMode.REPLACE) {
			return EditMode.REPLACE;
		}
		if (placeTargetEnemyId < 0 || placeTargetEnemyId > enemyDefine.varietyNum - 1) {
			return EditMode.NONE;
		}
		return EditMode.PLACE;
	}

	private bool SaveStageCsv(){
		string path = EditorUtility.SaveFilePanel ("", directoryPath, "", "csv");
		if (!string.IsNullOrEmpty(path)) {
			Debug.Log (path);
			return true;
		}
		return false;
	}
}

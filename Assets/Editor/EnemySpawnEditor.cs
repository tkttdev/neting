using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EnemySpawnEditor : EditorWindow {

	private Texture2D[] enemyTextures;
	private Texture2D[] moveEnemyTextures;
	private static EnemyDefine enemyDefine;
	private GameObject placeTargetEnemy;
	private int placeTargetEnemyId = 0;
	private int replaceTargetEnemyId = 0;
	private Vector2 enemyListScroll;
	private Vector2 stageLineScroll = new Vector2 (0f, 1200f);
	private TextAsset stageCsv;
	private Editor enemyEditor;
	private Event curEvent;
	private List<Vector2> placedEnemyPos = new List<Vector2>();
	private List<int> placedEnemyid = new List<int>();
	private float enemyPlaceAreaWidth = 500;
	private float placeEnemyTextureWidth = 20;
	private float placeEnemyTextureHeight = 20;
	private bool isEdited = false;
	private TextAsset beforeStageCsv = null;

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

		EditorGUILayout.BeginHorizontal ();

		//Begin EnemyPlaceArea
		EditorGUILayout.BeginVertical (GUILayout.MinWidth (enemyPlaceAreaWidth), GUILayout.MaxWidth (enemyPlaceAreaWidth));
		if (GUILayout.Button ("Vol1")) {
		}

		stageLineScroll = EditorGUILayout.BeginScrollView (stageLineScroll);
		DrawStageLine ();
		EditorGUILayout.EndScrollView ();


		if (editMode == EditMode.NONE) {
			if (curEvent.type == EventType.MouseDown) {
				if (curEvent.mousePosition.x <= enemyPlaceAreaWidth - 10) {
					int index = GetPlacedEnemyListIndexAtPos (curEvent.mousePosition);
					if (index > -1) {
						replaceTargetEnemyId = placedEnemyid [index];
						placedEnemyid.RemoveAt (index);
						placedEnemyPos.RemoveAt (index);
						editMode = EditMode.REPLACE;
						Repaint ();
					}
				}
			}
		} else if (editMode == EditMode.PLACE) {
			if (curEvent.mousePosition.x <= 490) {
				DisplayEnemyAtPos (curEvent.mousePosition, placeTargetEnemyId);
			}
			if (curEvent.type == EventType.MouseDown) {
				if (curEvent.mousePosition.x <= 490) {
					PlaceEnemy (curEvent.mousePosition, placeTargetEnemyId);
				}
			}
			if (curEvent.type == EventType.KeyDown && curEvent.keyCode == KeyCode.Backspace) {
				SetEditModeNone ();
			}
		} else if (editMode == EditMode.REPLACE) {
			if (curEvent.mousePosition.x <= 490) {
				DisplayEnemyAtPos (curEvent.mousePosition, replaceTargetEnemyId);
			}
			if (curEvent.type == EventType.MouseDown) {
				if (curEvent.mousePosition.x <= 490) {
					editMode = EditMode.NONE;
					PlaceEnemy (curEvent.mousePosition, replaceTargetEnemyId);
				}
			}
			if (curEvent.type == EventType.KeyDown && curEvent.keyCode == KeyCode.Backspace) {
				SetEditModeNone ();
			}
		}

		DisplayPlacedEnemy ();

		EditorGUILayout.EndVertical ();
		//End EnemyPlaceArea

		//Begin EnemyListArea
		EditorGUILayout.BeginVertical ();
		enemyListScroll = EditorGUILayout.BeginScrollView (enemyListScroll, GUILayout.MaxHeight (300));

		DisplayEnemyList ();
		UpdateMoveOnEnemyList ();

		EditorGUILayout.EndScrollView ();
		DisplayTargetEnemy ();
		if (GUILayout.Button ("SAVE")) {
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

	private void DrawStageLine(){
		for(int i = 0; i < 5; i++){
			EditorGUI.DrawRect (new Rect (80 + i * 100, 100, 2, 800), Color.black);
		}
	}


	/// <summary>
	/// if doesn't exist enemy at pos, return -1
	/// else this func return #INDEX OF PLACED ENEMY LIST#
	/// </summary>
	/// <param name="_pos">Position.</param>
	private int GetPlacedEnemyListIndexAtPos(Vector2 _pos){
		for (int i = 0; i < placedEnemyPos.Count; i++) {
			if (_pos.x >= placedEnemyPos [i].x - 10 && _pos.x <= placedEnemyPos [i].x + 10 && _pos.y >= placedEnemyPos [i].y - 10 && _pos.y <= placedEnemyPos [i].y + 10) {
				return i;
			}
		}
		return -1;
	}

	private void PlaceEnemy(Vector2 _pos, int _id){
		placedEnemyPos.Add (_pos);
		placedEnemyid.Add (_id);
		isEdited = true;
		Repaint ();
	}

	private void DisplayEnemyAtPos(Vector2 _pos, int _id){
		GUI.Box (new Rect (_pos.x - 10, _pos.y - 10, 20, 20), enemyTextures[_id]);
		Repaint ();
	}

	private void DisplayPlacedEnemy(){
		for (int i = 0; i < placedEnemyPos.Count; i++) {
			GUI.Box (new Rect (placedEnemyPos [i].x - 10, placedEnemyPos [i].y - 10, 20, 20), enemyTextures [placedEnemyid[i]]);
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

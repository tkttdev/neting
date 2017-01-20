using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(RichButton))]
public class RichButtonEditor : Editor {

	private const float minLongPressSec = 0.3f;
	private const float maxLongPressSec = 10.0f;

	private bool isWarning = false;

	private SerializedProperty longProp;
	private SerializedProperty useLongProp;

	void OnEnable(){
		//初回は何故かnullが返ってくるので回避用のコード
		longProp = serializedObject.FindProperty ("__dammy__");

		longProp = serializedObject.FindProperty ("onLongPress");
		useLongProp = serializedObject.FindProperty ("isUsedLongPress");
	}

	public override void OnInspectorGUI(){
		RichButton richButton = target as RichButton;
		base.OnInspectorGUI();

		EditorGUILayout.PropertyField (useLongProp);
		serializedObject.ApplyModifiedProperties();

		GUI.enabled = richButton.isUsedLongPress;

		EditorGUI.BeginChangeCheck ();
		richButton.longPressSec = EditorGUILayout.FloatField ("Long Press Sec", richButton.longPressSec);

		EditorGUILayout.PropertyField(longProp);
		serializedObject.ApplyModifiedProperties();


		if (EditorGUI.EndChangeCheck ()) {
			if (richButton.longPressSec < minLongPressSec || richButton.longPressSec > maxLongPressSec) {
				richButton.longPressSec = Mathf.Clamp (richButton.longPressSec, minLongPressSec, maxLongPressSec);
				isWarning = true;
			} else {
				isWarning = false;
			}
		}

		if(isWarning){
			string warning = "LongPressSecは" + minLongPressSec.ToString () + "以上" + maxLongPressSec.ToString () + "以下の範囲で設定してください．";
			EditorGUILayout.HelpBox (warning, MessageType.Warning);
		}
	}


}
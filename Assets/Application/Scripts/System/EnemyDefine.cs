using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class EnemyDefine : ScriptableObject {
	#region public_field
	public List<EnemyData> enemy = new List<EnemyData>();
	public int varietyNum = 0;
	#endregion

	#if UNITY_EDITOR
	[MenuItem("CreateScriptable/EnemyDefine")]
	static void Create(){
		var instance = CreateInstance<EnemyDefine> ();
		AssetDatabase.CreateAsset(instance, "Assets/EnemyDefineData.asset");
		AssetDatabase.Refresh();
	}
	#endif
}

[System.Serializable]
public class EnemyData {
	public float HP;
	public int DAMAGE;
	public int MONEY;
	public float SPEED;
	public string PATH;
}

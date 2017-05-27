using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDefineData : ScriptableObject {
	#region public_field
	public List<EnemyDefine> enemyDefine = new List<EnemyDefine>();
	#endregion

	[MenuItem("CreateScriptable/EnemyDefine")]
	static void Create(){
		var instance = CreateInstance<EnemyDefineData> ();
		AssetDatabase.CreateAsset(instance, "Assets/EnemyDefineData.asset");
		AssetDatabase.Refresh();
	}
}

[System.Serializable]
public class EnemyDefine {
	public int HP;
	public int DAMAGE;
	public int MONEY;
	public float SPEED;
	public string PATH;
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDefine : ScriptableObject {
	#region public_field
	public List<EnemyStatus> enemyStatus = new List<EnemyStatus>();
	#endregion

	[MenuItem("CreateScriptable/EnemyDefine")]
	static void Create(){
		var instance = CreateInstance<EnemyDefine> ();
		AssetDatabase.CreateAsset(instance, "Assets/EnemyDefine.asset");
		AssetDatabase.Refresh();
	}
}

[System.Serializable]
public class EnemyStatus {
	public int HP;
	public int DAMAGE;
	public int MONEY;
	public float SPEED;
	public string PATH;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BattleShipStatusWindow : ScriptableWizard {
	[MenuItem("Window/BattleShipStatus")]
	static void Open(){
		DisplayWizard<BattleShipStatusWindow> ("BattleShipStatus");
	}
}

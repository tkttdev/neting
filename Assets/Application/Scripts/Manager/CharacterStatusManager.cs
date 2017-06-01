using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;

public class CharacterStatusManager : SingletonBehaviour<CharacterStatusManager> {


	[SerializeField]private TextAsset StatusInfoText;
	private StatusInfo statusInfo = new StatusInfo();
	private List<StatusInfo> characterInfo = new List<StatusInfo> ();
	private string characterName = "ATLANTA";

	private List<int> level = new List<int> ();
	private List<int> attack = new List<int> ();
	private List<float> health = new List<float> ();
	private List<float> speed = new List<float> ();
	private List<int> bulletNum = new List<int> ();
	private List<float> chargeSpeed = new List<float> ();
	private List<int> money = new List<int>();

	// Use this for initialization
	protected override void Initialize() {
		base.Initialize ();
	}


	public void ParseStatusInfoText (string Name) {
		characterName = Name;
		StatusInfoText = Resources.Load ("CharacterStatus/" + characterName) as TextAsset;
		StringReader reader = new StringReader (StatusInfoText.text);


		while (reader.Peek () > -1) {
			string line = reader.ReadLine ();
			string[] values = line.Split (',');

			//変換
			level.Add (int.Parse (values [0]));
			attack.Add (int.Parse (values [1]));
			health.Add (float.Parse (values [2]));
			speed.Add (float.Parse (values [3]));
			bulletNum.Add (int.Parse (values [4]));
			chargeSpeed.Add (float.Parse (values [5]));
			money.Add (int.Parse (values [6]));

		}

		 

	}

	/*
	public void ShowDebugText(){
		Debug.Log(CharacterStatusManager.I.GetCharacterAttack (0));


	}
	*/

	public int GetCharacterAttack(int lv){
		return attack [lv - 1];
	}

	public float GetCharacterHealth(int lv){
		return health [lv - 1];
	}

	public float GetCharacterSpeed(int lv){
		return speed [lv - 1];
	}

	public int GetCharacterBulletNum(int lv){
		return bulletNum [lv - 1];
	}

	public float GetCharacterCharageSpeed(int lv){
		return chargeSpeed [lv - 1];
	}

	public int GetCharacterMoney(int charaId,int lv){
		ParseStatusInfoText (CHARACTER_DEFINE.NAME[charaId]);
		return money[lv - 1];
	}


	private class StatusInfo{
		public List<int> level = new List<int>();
		public List<int> attack = new List<int> ();
		public List<float> health = new List<float>();
		public List<float> speed = new List<float>();
		public List<int> bulletNum = new List<int>();
		public List<float> chargeSpeed = new List<float>();
		public List<int> money = new List<int>();
	}

}

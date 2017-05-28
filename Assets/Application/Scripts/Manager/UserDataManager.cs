using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class UserDataManager : SingletonBehaviour<UserDataManager> {

	private const int MAX_MONEY = 99999;
	private UserData userData;
	private string dataPath;

	public class UserData {
		public int useCharaIndex = 0;
		public int money = 0;
		public bool[] hasChara = new bool[10];
		public bool[] isClearStage = new bool[100];
		public int[] characterLevel = new int[10];

		public UserData() {
			hasChara[0] = true;
			for(int i = 1; i < 10; i++){
				hasChara[i] = false;
			}

			for(int i = 0; i < 100; i++){
				isClearStage[i] = false;
			}

			for(int i = 0; i < 10; i++){
				characterLevel[i] = 1;
			}
		}
	}

    protected override void Initialize() {
        base.Initialize();

		#if UNITY_EDITOR
		dataPath = Application.dataPath + "/Application/savedata.txt";
		#else
		dataPath = Application.persistentDataPath + "/savedata.txt";
		#endif

		userData = new UserData ();
		if (File.Exists (dataPath)) {
			LoadData ();
		} else {
			SaveData ();
		}
    }

	public int GetCharacterLevel(int _characterIndex){
		return userData.characterLevel[_characterIndex];
	}

	public void AddCharacterLevel(int _characterIndex){
		userData.characterLevel [_characterIndex] += 1;
		SaveData ();
	}

    public int GetUseCharacterIndex() {
        return userData.useCharaIndex;
    }

    public void SetUseCharacterIndex(int _characterIndex) {
		if (_characterIndex >= CHARACTER_DEFINE.characterVarietyNum) return;
        userData.useCharaIndex = _characterIndex;
		SaveData ();
    }

	public void GetCharacter(int _characterIndex){
		if (_characterIndex >= CHARACTER_DEFINE.characterVarietyNum) return;
		userData.hasChara [_characterIndex] = true;
		SaveData ();
	}

	//Debug用関数
	public void LossCharacter(int _characterIndex){
		if (_characterIndex >= CHARACTER_DEFINE.characterVarietyNum) return;
		userData.hasChara [_characterIndex] = false;
		SaveData ();
	}

	public void ResetLevel(int _characterIndex){
		userData.characterLevel [_characterIndex] = 1;
		SaveData ();
	}

	public int GetMoney(){
		return userData.money;
	}

	public void AddMoney(int _money){
		userData.money += _money;
		if (userData.money > MAX_MONEY) {
			userData.money = MAX_MONEY;
		}
		SaveData ();
	}

	public void ReduceMoney(int _money){
		userData.money -= _money;
		SaveData ();
	}

	public bool IsPermitUseCharacter(int _characterIndex){
		if (_characterIndex >= CHARACTER_DEFINE.characterVarietyNum) return false;
		return userData.hasChara [_characterIndex];
	}

	public bool IsClearStage(int _stageIndex){
		return userData.isClearStage [_stageIndex];
	}

	public void SetClearStage(int _stageIndex){
		userData.isClearStage [_stageIndex] = true;
	}

	public void SaveData (){
		string jData = JsonUtility.ToJson (userData);
		byte[] bData = Encoding.ASCII.GetBytes (jData);
		byte[] eData = EncryptData (bData);
		File.WriteAllBytes (dataPath, eData);
	}

	public void LoadData (){
		byte[] eData = File.ReadAllBytes (dataPath);
		byte[] dData = DecodeData (eData);
		string jData = Encoding.ASCII.GetString (dData);
		userData = JsonUtility.FromJson<UserData> (jData);
		CheckUpdateUserData ();
	}

	private void CheckUpdateUserData(){
		UserData tmpUserData = new UserData ();
		if (userData.isClearStage.Length != tmpUserData.isClearStage.Length) {
			for (int i = 0; i < userData.isClearStage.Length; i++) {
				tmpUserData.isClearStage [i] = userData.isClearStage [i];
			}
			userData = tmpUserData;
		} else if (userData.hasChara.Length != tmpUserData.hasChara.Length) {
			for (int i = 0; i < userData.hasChara.Length; i++) {
				tmpUserData.hasChara [i] = userData.hasChara [i];
			}
			userData = tmpUserData;
		}
	}

	private byte[] EncryptData(byte[] data){
		RijndaelManaged rijndael = new RijndaelManaged ();
		rijndael.KeySize = 128;
		rijndael.BlockSize = 128;

		string pw = "NetShootingPass";
		string salt = "SaltOfNetShooting";

		byte[] bSalt = Encoding.UTF8.GetBytes (salt);
		Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes (pw, bSalt);
		deriveBytes.IterationCount = 1000;

		rijndael.Key = deriveBytes.GetBytes (rijndael.KeySize / 8);
		rijndael.IV = deriveBytes.GetBytes (rijndael.BlockSize / 8);

		ICryptoTransform encryptor = rijndael.CreateEncryptor ();
		byte[] encryptedData = encryptor.TransformFinalBlock (data, 0, data.Length);

		encryptor.Dispose ();

		return encryptedData;
	}

	private byte[] DecodeData(byte[] data){
		RijndaelManaged rijndael = new RijndaelManaged ();
		rijndael.KeySize = 128;
		rijndael.BlockSize = 128;

		string pw = "NetShootingPass";
		string salt = "SaltOfNetShooting";

		byte[] bSalt = Encoding.UTF8.GetBytes (salt);
		Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes (pw, bSalt);
		deriveBytes.IterationCount = 1000;

		rijndael.Key = deriveBytes.GetBytes (rijndael.KeySize / 8);
		rijndael.IV = deriveBytes.GetBytes (rijndael.BlockSize / 8);

		ICryptoTransform decryptor = rijndael.CreateDecryptor ();
		byte[] decodedData = decryptor.TransformFinalBlock (data, 0, data.Length);

		decryptor.Dispose ();

		return decodedData;
	}
}

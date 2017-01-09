using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : SingletonBehaviour<UserDataManager> {

	public class UserData {
		public int useCharaIndex = 0;
		public int money = 0;
		public bool[] hasChara = new bool[10];

		public UserData() {
			hasChara[0] = true;
			for(int i = 1; i < 10; i++){
				hasChara[i] = false;
			}
		}
	}

	private UserData userData = new UserData();

    protected override void Initialize() {
        base.Initialize();
    }

    public int GetUseCharaIndex() {
        return userData.useCharaIndex;
    }

    public void SetUseCharacterIndex(int _characterIndex) {
        userData.useCharaIndex = _characterIndex;
    }
}

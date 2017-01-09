using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : SingletonBehaviour<UserDataManager> {

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

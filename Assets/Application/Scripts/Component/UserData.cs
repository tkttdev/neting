using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour {

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

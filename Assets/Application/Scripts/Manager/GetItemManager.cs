using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemManager : SingletonBehaviour<GetItemManager> {

	private int earnMoney = 0;

	public void AddEarnMoney(int _earnMoney){
		earnMoney += _earnMoney;
	}

	public int GetEarnMoney(){
		return earnMoney;
	}
}

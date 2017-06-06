using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour {

	private enum SkillType : int {
		LASER = 0,
		THUNDER = 1,
		BOMB = 2,
		GATLING = 3,
	}

	[SerializeField]private SkillType skill;
	[SerializeField]private GameObject[] skillPrefab;

	private void Start(){
		skill =  (SkillType)UserDataManager.I.GetUseCharacterIndex();
	}

	public void PushSkillButon() {
		switch (skill) {
			case SkillType.LASER:
				BattleShip.I.SetSkill(skillPrefab[0]);
				break;
			case SkillType.THUNDER:
				BattleShip.I.SetSkill(skillPrefab[1]);
				break;
			case SkillType.BOMB:
				ObjectPool.I.Instantiate(skillPrefab[2], new Vector3(0, -5, 0));
				break;
			case SkillType.GATLING:
				BattleShip.I.SetGatling();
				break;
		}
		gameObject.SetActive(false);
	}
}

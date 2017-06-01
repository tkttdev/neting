using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skill : int {
	LASER = 0,
	THUNDER = 1,
	BOMB = 2,
	GATLING = 3,
}

public class SkillButton : SingletonBehaviour<SkillButton> {

	public Skill skill;
	public GameObject[] skillPrefab;

	// Use this for initialization
	protected override void Initialize() {
		base.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PushSkillButon() {
		if (gameObject.activeSelf == true) {
			switch (skill) {
				case Skill.LASER:
					BattleShip.I.SetSkill(skillPrefab[0]);
					break;
				case Skill.THUNDER:
					BattleShip.I.SetSkill(skillPrefab[1]);
					break;
				case Skill.BOMB:
					ObjectPool.I.Instantiate(skillPrefab[2], new Vector3(0, -5, 0));
					break;
				case Skill.GATLING:
					BattleShip.I.SetGatling();
                    break;
			}
			gameObject.SetActive(false);
		}
	}
}

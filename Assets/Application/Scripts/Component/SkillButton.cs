using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skill : int {
	BOMB = 0,
	THUNDER = 1,
	GATLING = 2,
}

public class SkillButton : MonoBehaviour {

	[SerializeField] private Skill skill;
	public GameObject[] skillPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PushSkillButon() {
		if (gameObject.activeSelf == true) {
			switch (skill) {
				case Skill.BOMB:
					Instantiate(skillPrefab[0], new Vector3(0, -5, 0), transform.rotation);
					break;
				case Skill.THUNDER:
					BattleShip.I.SetSkill(skillPrefab[1]);
                    break;
				case Skill.GATLING:
					BattleShip.I.SetGatling();
                    break;
			}
			gameObject.SetActive(false);
		}
	}
}

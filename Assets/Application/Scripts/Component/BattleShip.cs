using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class BattleShip : SingletonBehaviour<BattleShip> {

	private float intervalCount = 0;
	private int useCharaIndex = 0;
	private int useCharaLv = 1;

	private float bulletInterval = 2;
	private int bulletStock = 0;
	private int maxBulletStock = 0;
	private float life = 3;
	private string charaName = "ATLANTA";

	[SerializeField] private Text bulletNum;
	[SerializeField] private TextAsset bulletSpawnerInfo;
	[SerializeField] private GameObject characterBullet;
	[SerializeField] private Corner[] bulletSpawnCorner = new Corner[5];

	private bool activeSkill;
	private bool activeGatling;
	private GameObject bulletPrefab;

	protected override void Initialize() {
		base.Initialize();
		#if UNITY_EDITOR
		if (GameObject.Find("Systems") == null) {
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
	}

	private void Start(){
		LoadSpawnPos ();
		LoadCharaStatus();
	}

	private void LoadSpawnPos(){
		StageManager.I.bulletSpawnCorner.CopyTo (bulletSpawnCorner, 0);
	}

	private void LoadCharaStatus() {
		useCharaIndex = UserDataManager.I.GetUseCharacterIndex();
		useCharaLv = UserDataManager.I.GetCharacterLevel(useCharaIndex);
		charaName = CHARACTER_DEFINE.NAME[useCharaIndex];
		CharacterStatusManager.I.ParseStatusInfoText(charaName);

		bulletInterval = CharacterStatusManager.I.GetCharacterCharageSpeed(useCharaLv);
		maxBulletStock = CharacterStatusManager.I.GetCharacterBulletNum(useCharaLv);
		life = CharacterStatusManager.I.GetCharacterHealth(useCharaLv);

		characterBullet.GetComponent<Bullet>().SetDamage(CharacterStatusManager.I.GetCharacterAttack(useCharaLv));
		characterBullet.GetComponent<Bullet>().SetSpeed(CharacterStatusManager.I.GetCharacterSpeed(useCharaLv));
		characterBullet.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Bullet/Bullet" + useCharaIndex);
		bulletPrefab = characterBullet;

		bulletStock = maxBulletStock;
		intervalCount = 0;
		UIManager.I.UpdateCharacterInfo (life, bulletStock);
    }

	void Update() {
		if (GameManager.I.CheckGameStatus (GameStatus.PLAY)) {
			if (activeSkill == false) {
				bulletNum.color = Color.white;
				if (bulletStock < maxBulletStock && intervalCount == 0) {
					intervalCount = bulletInterval;
				}

				if (intervalCount > 0.0f) {
					intervalCount -= Time.deltaTime;
					if (intervalCount <= 0.0f) {
						intervalCount = 0.0f;
						bulletStock += 1;
					}
				}
			} else {
				bulletNum.color = Color.green;
				if (bulletStock < 1) {
					bulletPrefab = characterBullet;
					bulletStock = maxBulletStock;
					activeSkill = false;
				}
			}
		}

		UIManager.I.UpdateCharacterInfo(life, bulletStock);
	}

	public void Shoot(int _entryX) {
		if (bulletStock == 0 || !GameManager.I.CheckGameStatus(GameStatus.PLAY) || bulletSpawnCorner[_entryX + 2] == null) {
			return;
		}
		if (activeGatling == false) {
			bulletStock--;
		}
		UIManager.I.UpdateCharacterInfo (life, bulletStock);
		Bullet bullet = ObjectPool.I.Instantiate (bulletPrefab, new Vector3 ((float)_entryX, -4f, 0.0f)).GetComponent<Bullet> ();
		if (bulletSpawnCorner [_entryX + 2].CheckCurve (MoveDir.UP, 1, bullet.moveMode)) {
			bullet.isCurve = true;
			bullet.bezerPoints = bulletSpawnCorner [_entryX + 2].ChangePurposeCurve (bullet.moveMode, 1, ref bullet.moveDir, ref bullet.lineId, ref bullet.onLineLength, ref bullet.lengthOfBezerSection);
		} else {
			bullet.endPos = bulletSpawnCorner [_entryX + 2].ChangePurposeStraight (bullet.moveMode, 1, ref bullet.moveDir, ref bullet.lineId, ref bullet.onLineLength);
			bullet.onLineLength -= Mathf.Abs (-4f - bulletSpawnCorner [_entryX + 2].transform.position.y); // TODO:このコード無しで初期速度を正常にするよう変更
		}
		SoundManager.I.SoundSE (SE.SHOOT);
	}

	public void TakeDamage(int _damage){
		#if UNITY_EDITOR
		if(GameManager.I.isDemo) return;
		#endif
		StartCoroutine(DamageRendering());
		life -= _damage;
		UIManager.I.UpdateCharacterInfo (life, bulletStock);
		if (life <= 0) {
			GameManager.I.SetStatuEnd ();
		}
	}

	private IEnumerator DamageRendering(){
		for (int i = 0; i < 1; i++) {
			iTween.MoveTo (Camera.main.gameObject, iTween.Hash ("x", 0.3f, "time", 0.05f, "easeType", iTween.EaseType.easeInExpo));
			yield return new WaitForSeconds (0.05f);
			iTween.MoveTo (Camera.main.gameObject, iTween.Hash ("x", -0.3f, "time", 0.1f, "easeType", iTween.EaseType.easeInExpo));
			yield return new WaitForSeconds (0.1f);
		}
		iTween.MoveTo (Camera.main.gameObject, iTween.Hash ("x", 0f, "time", 0.05f, "easeType", iTween.EaseType.easeInExpo));
		yield return new WaitForSeconds (0.05f);
	}

	public float GetLife(){
		return life;
	}

	public float bulletRate{
		get { return (bulletInterval - intervalCount) / bulletInterval; }
	}

	public void SetSkill(GameObject prefab) {
		intervalCount = 0;
		bulletStock = 5;

        bulletPrefab = prefab;
		activeSkill = true;
	}

	public void SetGatling() {
		intervalCount = 0;
		bulletStock = maxBulletStock;

		activeGatling = true;
		StartCoroutine("EndSkill");
	}

	private IEnumerator EndSkill() {
		yield return new WaitForSeconds(5.0f);

		bulletPrefab = characterBullet;
		bulletStock = maxBulletStock;
		activeGatling = false;
	}
}

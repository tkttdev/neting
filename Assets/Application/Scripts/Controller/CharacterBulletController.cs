using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBulletController : SingletonBehaviour<CharacterBulletController> {

    [SerializeField]  private int fullTime = 2;
    private int count = 0;
    private int entryX = 0;
    [SerializeField] private Text countText;
    [SerializeField] private Text stockText;
    private int useCharaIndex = 0;
    private int bulletInterval = 2;
    private int bulletStock = 0;
    private int maxBulletStock = 0;
    private GameObject charaBulletPrefab;
    [SerializeField] private bool isDemo = false;


    protected override void Initialize() {
        base.Initialize();
        count = 0;
        countText.text = "IN " + count.ToString() + "sec";
        StartCoroutine(Counter());

        #if UNITY_EDITOR
        if (GameObject.Find("Systems") == null) {
            GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
            Instantiate(obj).name = "Systems";
        }
        #endif

        //キャラ毎のstatus設定
        useCharaIndex = UserDataManager.I.GetUseCharaIndex();
        bulletInterval = CHARACTER_STATUS_DEFINE.BULLET_INTERVAL[useCharaIndex];
        maxBulletStock = CHARACTER_STATUS_DEFINE.MAX_BULLET_STOCK[useCharaIndex];
        charaBulletPrefab = Resources.Load(CHARACTER_STATUS_DEFINE.BULLET_PREFAB_PATHS[useCharaIndex]) as GameObject;
    }

    void Update() {
        stockText.text = bulletStock.ToString();
        if (bulletStock != 0 && Input.GetMouseButtonDown(0)) {
            bulletStock--;
            if (Input.mousePosition.x < Screen.width / 5.0f) {
                entryX = -2;
            } else if (Input.mousePosition.x < (Screen.width / 5.0f) * 2.0f) {
                entryX = -1;
            } else if (Input.mousePosition.x < (Screen.width / 5.0f) * 3.0f) {
                entryX = 0;
            } else if (Input.mousePosition.x < (Screen.width / 5.0f) * 4.0f) {
                entryX = 1;
            } else {
                entryX = 2;
            }
            Shoot();
        }


    }

    private void Shoot() {
        Instantiate(charaBulletPrefab, new Vector3((float)entryX, -4.0f, 0.0f), Quaternion.Euler(0, 0, 0));
    }

    public IEnumerator Counter() {
        count = bulletInterval;
        while (true) {
            if (count > 0) {
                count--;
                countText.text = "IN " + count.ToString() + "sec";
                if (count == 0) {
                    bulletStock++;
                    if(bulletStock < maxBulletStock) {
                        StartCoroutine(Counter());
                    }
                    yield break;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}

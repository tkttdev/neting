using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBulletController : SingletonBehaviour<PlayerBulletController> {

    [SerializeField]
    private int fullTime = 2;
    private int count = 0;
    private int entryX = 0;
    [SerializeField] private Text countText;
    [SerializeField] private GameObject playerBullet;

    protected override void Initialize() {
        count = 0;
        countText.text = "IN " + count.ToString() + "sec";
    }

    void Update() {
        if (count == 0 && Input.GetMouseButtonDown(0)) {
            if (Input.mousePosition.x < Screen.width / 5.0f) {
                entryX = -2;
            }
            else if (Input.mousePosition.x < (Screen.width / 5.0f) * 2.0f) {
                entryX = -1;
            }
            else if (Input.mousePosition.x < (Screen.width / 5.0f) * 3.0f) {
                entryX = 0;
            }
            else if (Input.mousePosition.x < (Screen.width / 5.0f) * 4.0f) {
                entryX = 1;
            }
            else {
                entryX = 2;
            }
            Shoot();
        }
    }

    private void Shoot() {
        Instantiate(playerBullet, new Vector3((float)entryX, -4.0f, 0.0f), Quaternion.Euler(0, 0, 0));
        count = fullTime;
        StartCoroutine("Counter");
    }

    public IEnumerator Counter() {
        while (true) {
            if (count > 0) {
                count--;
                countText.text = "IN " + count.ToString() + "sec";
                if (count == 0) {
                    yield break;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}

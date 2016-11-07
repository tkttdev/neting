using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerControllerByShimizu : SingletonBehaviour<PlayerControllerByShimizu>{

    [SerializeField] private int fullTime = 3;
    private int count = 0;
    private int entryX;
    [SerializeField] private Text countText;
    [SerializeField] private GameObject playerBullet;

    protected override void Initialize(){
        count = 0;
        countText.text = "IN " + count.ToString() + "sec";
    }

	void Update () {
        if (count == 0 && (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.T))){
            if (Input.GetKey(KeyCode.Q)) entryX = -2;
            if (Input.GetKey(KeyCode.W)) entryX = -1;
            if (Input.GetKey(KeyCode.E)) entryX = 0;
            if (Input.GetKey(KeyCode.R)) entryX = 1;
            if (Input.GetKey(KeyCode.T)) entryX = 2;
            Instantiate(playerBullet, new Vector3((float)entryX, -4.0f, 0.0f), Quaternion.Euler(0, 0, 0));
            count = fullTime;
            StartCoroutine("Counter");
        }
    }

    public IEnumerator Counter()
    {
        while (true)
        {
            if (count > 0) {
                count--;
                countText.text = "IN " + count.ToString() + "sec";
                if (count == 0){
                    yield break;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}

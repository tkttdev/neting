using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyControllerByShimizu : SingletonBehaviour<EnemyControllerByShimizu>
{
    private string[] wave;
    [SerializeField] private TextAsset textData;

     private int H;
    private int nowRow;
    [SerializeField] private int interval = 5;
    [SerializeField] private GameObject enemy;

    protected override void Initialize()
    {
        SplitString();
        H = wave.Length / 5;
        /*
        Debug.Log("H: " + H);
        for (int i=0; i<wave.Length; i++)
        {
            Debug.Log(i);
            //if((i + 1) % 5 == 0) Debug.Log("endRow");
        }
        */
        nowRow = H - 1;
        StartCoroutine("ComeWaves");
    }

    private void SplitString()
    {
        string S = textData.text;
        wave = S.Split(',');
    }

    public IEnumerator ComeWaves()
    {
        while (true)
        {
            Debug.Log(nowRow);
            for (int i=0; i<5; i++)
            {
                if(wave[nowRow*5 + i] == "1")
                {
                    Instantiate(enemy, new Vector3(i-2, 4, 0), Quaternion.Euler(0, 0, 0));
                }
            }
            nowRow--;
            //if (nowRow < 0) yield break;
            if (nowRow < 0) nowRow = H - 1;
            yield return new WaitForSeconds(interval);
        }
    }
}

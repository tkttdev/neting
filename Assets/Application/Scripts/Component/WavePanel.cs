using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavePanel : MonoBehaviour {
	
	[SerializeField] private Text waveText;

	public void Show(){
		StartCoroutine (ShowAnimation ());
	}

	public void Hide(){
		gameObject.transform.localScale = new Vector3 (0, 0.01f, 1);
		waveText.text = "";
		gameObject.SetActive (false);
	}

	private IEnumerator ShowAnimation(){
		iTween.ScaleTo (gameObject, iTween.Hash ("x", 1, "time", 1));
		yield return new WaitForSeconds (0.8f);
		iTween.ScaleTo (gameObject, iTween.Hash ("y", 1, "time", 1));
		yield return new WaitForSeconds (0.8f);
		waveText.text = string.Format ("Stage {0}\nStart", StageLevelManager.I.GetStageLevel ());
		yield return new WaitForSeconds (0.8f);
		GameManager.I.SetStatuPlay ();
		Hide ();
		yield break;
	}
}

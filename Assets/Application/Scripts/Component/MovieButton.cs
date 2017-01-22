using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieButton : MonoBehaviour {

	[SerializeField] private Button ownButton;

	void Start(){
		if (AdsManager.I.IsAbleWatch ()) {
			ownButton.onClick.AddListener (AdsManager.I.ShowRewardedAd);
		} else {
			gameObject.SetActive (false);
		}
	}
}

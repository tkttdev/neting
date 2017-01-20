using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieButton : MonoBehaviour {

	[SerializeField] private Button ownButton;

	void Start(){
		ownButton.onClick.AddListener (AdsManager.I.ShowRewardedAd);
	}
}

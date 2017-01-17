using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Advertisements;

public class AdsManager : SingletonBehaviour<AdsManager> {

	/*private bool beAbleWatch = true;
	private const string ADS_KEY = "ableWatchAdsKey";

	protected override void Initialize (){
		base.Initialize ();
		GetAdsInfo ();
	}

	public void ShowRewardedAd(){
		if (Advertisement.IsReady ("rewardedVideo")) {
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show ("rewardedVideo", options);
		}
	}

	private void HandleShowResult(ShowResult result){
		switch (result){
		case ShowResult.Finished:
			UserDataManager.I.AddMoney (1000);
			beAbleWatch = false;
			SaveAdsInfo ();
			break;
		case ShowResult.Skipped:
			break;
		case ShowResult.Failed:
			break;
		}
	}

	public void EnableWatchAds(){
		beAbleWatch = true;
		SaveAdsInfo ();
	}

	private void SaveAdsInfo(){
		PlayerPrefs.SetInt (ADS_KEY, beAbleWatch ? 1 : 0);
	}

	public bool GetAdsInfo(){
		return (beAbleWatch = (PlayerPrefs.GetInt (ADS_KEY, 1) == 1));
	}*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : SingletonBehaviour<AdsManager> {

	private bool beAbleWatch = true;
	private const string ADS_KEY = "ableWatchAdsKey";

	protected override void Initialize (){
		base.Initialize ();
		beAbleWatch = (PlayerPrefs.GetInt (ADS_KEY, 1) == 1);
	}

	public void ShowRewardedAd(){
		StageSelectUIManager.I.InactiveNotDialogComponent ();
		if (Advertisement.IsReady ("rewardedVideo")) {
			beAbleWatch = false;
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show ("rewardedVideo", options);
		}
	}

	private void HandleShowResult(ShowResult result){
		switch (result){
		case ShowResult.Finished:
			UserDataManager.I.AddMoney (1000);
			SoundManager.I.SoundSE (SE.PURCHASE);
			StageSelectUIManager.I.ShowGetMoneyDialog ();
			beAbleWatch = false;
			SaveAdsInfo ();
			break;
		case ShowResult.Skipped:
			break;
		case ShowResult.Failed:
			break;
		}
	}

	public void EnableWatch(){
		beAbleWatch = true;
		SaveAdsInfo ();
	}

	private void SaveAdsInfo(){
		PlayerPrefs.SetInt (ADS_KEY, beAbleWatch ? 1 : 0);
	}

	public bool IsAbleWatch(){
		return (beAbleWatch = (PlayerPrefs.GetInt (ADS_KEY, 1) == 1));
	}
}

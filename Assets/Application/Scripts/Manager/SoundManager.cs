using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SE : int {
	BUTTON0 = 0,
	BUTTON1 = 1,
	BUTTON2 = 2,
	PURCHASE = 3,
	SHOOT = 4,
	DEAD = 5,
	DAMAGE1 = 6,
	DAMAGE2 = 7,
	HIT = 8,
	GAME_OVER = 9,
	CLEAR = 10,
	WARP = 11,
}

public enum BGM : int {
	TITLE_BGM = 0,
	MENU_BGM = 1,
	CHARACTER_STORE_BGM = 2,
	CREDIT = 3,
	STAGE_EASY = 4,
	STAGE_NORMAL = 5,
	STAGE_HARD = 6,
	STAGE_VERY_HARD = 7,
}

public class SoundManager : SingletonBehaviour<SoundManager> {

	//0:bgm 1:se
	private AudioSource[] audioSource;

	[SerializeField] private AudioClip[] seAudioClip;
	[SerializeField] private AudioClip[] bgmAudioClip;

	protected override void Initialize (){
		base.Initialize ();
		audioSource = gameObject.GetComponents<AudioSource> ();
	}

	public void SoundBGM(BGM bgmType){
		audioSource [0].clip = bgmAudioClip [(int)bgmType];
		audioSource [0].Play ();
	}

	public void SoundSE(SE seType){
		audioSource [1].PlayOneShot (seAudioClip[(int)seType]);
	}

	public void StopBGM(){
		audioSource [0].Stop ();
	}

	public void StopSE(){
		audioSource [1].Stop ();
	}
}

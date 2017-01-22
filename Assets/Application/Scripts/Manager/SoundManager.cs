using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SE : int {
	BUTTON = 0,
	PURCHASE = 1,
	SHOOT = 2,
	DEAD = 3,
	DAMAGE = 4,
}

public enum BGM : int {
	TITLE_BGM = 0,
	MENU_BGM = 1,
	CHARACTER_STORE_BGM = 2,
	STAGE_EASY = 3,
	STAGE_NORMAL = 4,
	STAGE_HARD = 5,
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
}

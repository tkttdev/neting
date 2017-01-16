using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SE : int {

}

public enum BGM : int {

}

public class SoundManager : SingletonBehaviour<SoundManager> {

	//0:bgm 1:se
	public AudioSource[] audioSource;

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

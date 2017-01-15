﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

	[SerializeField] private Image background;
	[SerializeField] private Text tapText;

	private float backgroundBlinkTime = 0.4f;

	void Start(){
		iTween.ScaleTo (tapText.gameObject, iTween.Hash ("x", 1.2f, "y", 1.2f, "easeType", iTween.EaseType.linear, "loopType", iTween.LoopType.pingPong, "time", 1.65f));
	}

	void Update(){
		BackgroundEffect ();
	}

	public void MenuButton(){
		AppSceneManager.I.GoScene (GameSceneType.MENU_SCENE);
	}

	private void BackgroundEffect(){
		background.color = new Color (background.color.r, background.color.g, background.color.b, Mathf.PingPong (Time.time * backgroundBlinkTime, 0.7f) + 0.3f);
	}
}

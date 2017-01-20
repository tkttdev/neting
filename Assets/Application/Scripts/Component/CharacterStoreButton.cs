﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(ButtonExtention))]
public class CharacterStoreButton : MonoBehaviour {
	[SerializeField] private int charaId;

	void Start() {
		var button = GetComponent<ButtonExtention>();
		button.onClick.AddListener (() => Pressed ());
		button.onLongPress.AddListener(() => LongPressed());
	}

	public void LongPressed(){
		CharacterStoreController.I.ShowCharacterStatusPanel (charaId);
	}

	private void Pressed(){
		CharacterStoreController.I.CharacterSelectButton (charaId);
	}
}
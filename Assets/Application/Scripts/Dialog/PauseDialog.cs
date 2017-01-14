using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PauseDialog : DialogBase {

	[SerializeField]private GameObject panel;

	// Use this for initialization
	protected override void Start() {
		panel = gameObject.transform.FindChild("Panel").gameObject;

		base.Start();
		Hide();
	}

	// Update is called once per frame
	void Update() {

	}

	public override void Show() {
		base.Show();
		SetComponentsActive();
	}

	public override void Hide() {
		base.Hide();
		SetComponentsInactive();
	}

	private void SetComponentsInactive() {
		panel.SetActive(false);
	}

	private void SetComponentsActive() {
		panel.SetActive(true);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonExtention : Button {

	public UnityEvent onLongPress = new UnityEvent ();
	public float longPressIntervalSec = 1.0f;

	private float pressingSeconds = 0.0f;
	private bool isEnableLongPress = true;
	private bool isPressing = false;

	void Update(){
		if (!isPressing && isEnableLongPress) {
			pressingSeconds += Time.deltaTime;
			if (pressingSeconds >= longPressIntervalSec) {
				onLongPress.Invoke ();
				isEnableLongPress = false;
			}
		}
	}

	public override void OnPointerDown (PointerEventData eventData){
		base.OnPointerDown (eventData);
		isPressing = true;
	}

	public override void OnPointerUp (PointerEventData eventData){
		base.OnPointerUp (eventData);
		pressingSeconds = 0.0f;
		isEnableLongPress = true;
		isPressing = false;
	}
}

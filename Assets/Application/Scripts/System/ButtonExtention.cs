using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonExtention : Button{
	public UnityEvent onLongPress = new UnityEvent();
	public float longPressIntervalSeconds = 1.0f;

	private float pressingSeconds    = 0.0f;
	private bool isEnabledLongPress  = true;
	private bool isPressing          = false;
	private bool isLongPress = false;

	void Update(){
		if (isPressing && isEnabledLongPress){
			pressingSeconds += Time.deltaTime;
			if (pressingSeconds >= longPressIntervalSeconds){
				onLongPress.Invoke();
				isEnabledLongPress = false;
				isLongPress = true;
			}
		}
	}

	public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData){
		base.OnPointerDown(eventData);
		isPressing = true;
	}

	public override void OnPointerUp (UnityEngine.EventSystems.PointerEventData eventData){
		base.OnPointerUp(eventData);
		pressingSeconds = 0.0f;
		isEnabledLongPress = true;
		isPressing = false;
		isLongPress = false;
	}

	public override void OnPointerClick (PointerEventData eventData){
		if (isLongPress) {
			isLongPress = false;
			return;
		}
		base.OnPointerClick (eventData);
	}
}
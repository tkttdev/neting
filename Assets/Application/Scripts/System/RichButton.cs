using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class RichButton : Button{
	[HideInInspector] public bool isUsedLongPress = false;
	[HideInInspector] public float longPressSec = 1.0f;
	[HideInInspector] public UnityEvent onLongPress = new UnityEvent();

	private float pressingSeconds    = 0.0f;
	private bool isPressing          = false;
	private bool isLongPress 		 = false;

	void Update(){
		if (isPressing){
			pressingSeconds += Time.deltaTime;
			if (pressingSeconds >= longPressSec && isUsedLongPress){
				onLongPress.Invoke();
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
		isPressing = false;
		isLongPress = false;
	}

	public override void OnPointerClick (PointerEventData eventData){
		if (isLongPress && isUsedLongPress) {
			isLongPress = false;
			return;
		}
		base.OnPointerClick (eventData);
	}
}
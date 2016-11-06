using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogBase : MonoBehaviour {

	private CanvasGroup canvasGroup;

	protected virtual void Start () {
		canvasGroup = gameObject.GetComponent<CanvasGroup> ();
		if (canvasGroup == null) {
			canvasGroup = gameObject.AddComponent<CanvasGroup> ();
		}
		canvasGroup.alpha = 0;
	}

	public virtual void Show(){
		canvasGroup.alpha = 1;
	}

	public virtual void Hide(){
		canvasGroup.alpha = 0;
	}
}

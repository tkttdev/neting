using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

	[SerializeField] private SpriteRenderer background;
	[SerializeField] private Text tapText;

	private float backgroundBlinkTime = 0.4f;

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			AppSceneManager.I.GoScene (GameSceneType.MENU_SCENE);
		}
		BackgroundEffect ();
	}

	private void BackgroundEffect(){
		background.color = new Color (background.color.r, background.color.g, background.color.b, Mathf.PingPong (Time.time * backgroundBlinkTime, 0.7f) + 0.3f);
	}
}

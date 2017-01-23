using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

	[SerializeField] private Image background;
	[SerializeField] private Text tapText;

	private float backgroundBlinkTime = 0.4f;

	void Start(){
		SoundManager.I.SoundBGM (BGM.TITLE_BGM);
		iTween.ScaleTo (tapText.gameObject, iTween.Hash ("x", 1.0f, "y", 1.0f, "easeType", iTween.EaseType.linear, "loopType", iTween.LoopType.pingPong, "time", 1.65f));
	}

	void Update(){
		BackgroundEffect ();
	}

	public void MenuButton(){
		AppSceneManager.I.GoScene (GameSceneType.MENU_SCENE);
	}

	public void CreditButton(){
		AppSceneManager.I.GoScene (GameSceneType.CREDIT_SCENE);
	}

	private void BackgroundEffect(){
		background.color = new Color (background.color.r, background.color.g, background.color.b, Mathf.PingPong (Time.time * backgroundBlinkTime, 0.7f) + 0.3f);
	}

	private IEnumerator GoMenu(){
		
	}

	private IEnumerator GoCredit(){

	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageSelectController : MonoBehaviour {
	
	[SerializeField] private Text moneyText;
    [SerializeField] private GameObject[] buttonRoot;
	[SerializeField] private GameObject[] stageButtonRoot;


	//0~10 => 0, 11~20 => 1
	private int showStageIndex = 0;
	private int minStageIndex = 0;
	private int maxStageIndex = 1;
	private int purposeStageIndex = 0;

	private float unitX;

	[SerializeField] private GameObject[] coverPanel;
	[SerializeField] private Image characterImage;

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		if(GameObject.Find("Systems") == null){
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif

		SoundManager.I.SoundBGM (BGM.MENU_BGM);

		characterImage.sprite = Resources.Load<Sprite> (CHARACTER_DEFINE.IMAGE_RESOURCES_PATH [UserDataManager.I.GetUseCharacterIndex ()]);
		moneyText.text = string.Format ("{0}", UserDataManager.I.GetMoney ().ToString ());
		unitX = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0, 0)).x * 2.0f;
	}

	void Update(){
		moneyText.text = string.Format ("{0}", UserDataManager.I.GetMoney ().ToString ());
	}

	/// <summary>
	/// left : dir => 1, right : dir => -1
	/// </summary>
	/// <param name="_dir">Dir.</param>
	public void MoveStageButton(int _dir){
		StartCoroutine (ButtonMoveAnimation (_dir));
	}

	private IEnumerator ButtonMoveAnimation(int _dir){
		for (int i = 0; i < coverPanel.Length; i++) {
			coverPanel [i].SetActive (true);
		}
		for (int i = 0; i < buttonRoot.Length; i++) {
			iTween.ScaleTo (buttonRoot [i].gameObject, iTween.Hash ("x", 0.7f, "y", 0.7f, "time", 0.25f));
		}
		yield return new WaitForSeconds(0.2f);
		for (int i = 0; i < stageButtonRoot.Length; i++) {
			iTween.MoveBy (stageButtonRoot [i].gameObject, iTween.Hash ("x", _dir * unitX, "time", 0.65f));
		}
		yield return new WaitForSeconds (0.5f);
		for (int i = 0; i < buttonRoot.Length; i++) {
			iTween.ScaleTo (buttonRoot [i].gameObject, iTween.Hash ("x", 1.0f, "y", 1.0f, "time", 0.25f));
		}
		yield return new WaitForSeconds (0.25f);
		for (int i = 0; i < coverPanel.Length; i++) {
			coverPanel [i].SetActive (false);
		}
		yield break;
	}

	public void StageSelectButton(int stageLevel){
		if (stageLevel == 0) {
			stageLevel = 1;
		}
		StageLevelManager.I.SetStageLevel (stageLevel);
		AppSceneManager.I.GoScene(GameSceneType.GAME_SCENE);
	}

	public void CharacterButton() {
		AppSceneManager.I.GoScene(GameSceneType.CHARACTER_STORE);
	}
}

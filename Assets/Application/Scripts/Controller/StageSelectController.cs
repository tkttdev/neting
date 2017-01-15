using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageSelectController : MonoBehaviour {
	
	[SerializeField] private Text moneyText;
    [SerializeField] private GameObject[] stageButtonRoot;

	//0~10 => 0, 11~20 => 1
	private int showStageIndex = 0;
	private int minStageIndex = 0;
	private int maxStageIndex = 1;
	private int purposeStageIndex = 0;

	private float unitX;

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		if(GameObject.Find("Systems") == null){
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif

		moneyText.text = string.Format ("Money : {0}", UserDataManager.I.GetMoney ().ToString ());
		unitX = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0, 0)).x * 2.0f;
	}

    void Update () {
		if (Input.GetMouseButtonDown (0) && !EventSystem.current.IsPointerOverGameObject ()) {
			if (Input.mousePosition.x < Screen.width / 2.0f) {
				purposeStageIndex = showStageIndex - 1;
			} else {
				purposeStageIndex = showStageIndex + 1;
			}

			purposeStageIndex = Mathf.Clamp (purposeStageIndex, minStageIndex, maxStageIndex);
			if (purposeStageIndex != showStageIndex) {
				StartButtonAnimation (showStageIndex - purposeStageIndex);
				showStageIndex = purposeStageIndex;
			}
		}
	}

	private void StartButtonAnimation(int _dir){
		for (int i = 0; i < stageButtonRoot.Length; i++) {
			iTween.MoveBy (stageButtonRoot [i].gameObject, iTween.Hash ("x", _dir * unitX, "time", 1.5f));
		}
	}

	private void EndButtonAnimation(){
		
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

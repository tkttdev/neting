using UnityEngine;
using System.Collections;

public class StageSelectController : MonoBehaviour {

    [SerializeField] private GameObject stageButtonRoot;
    private bool isTouch = false;
    private float frickDist = 0.0f;
    private float unitX;
    private int displayStagePanelIndex = 0;
    private int stagePanelNum = 2;

    private Vector3 lastPos;
    private bool isMove = false;
    private int purposePanelIndex = 0;

	// Use this for initialization
	void Start () {
        unitX = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(-Screen.width / 2, 0, 0)).x, 0, 0).x;
		#if UNITY_EDITOR
		if(GameObject.Find("Systems") == null){
			GameObject obj = Resources.Load("Prefabs/Systems") as GameObject;
			Instantiate(obj).name = "Systems";
		}
		#endif
	}
    float buttonRootX = 0;
    int nearbyStagePanelIndex = 0;
    // Update is called once per frame
    void Update () {
        
        if (Input.GetMouseButtonDown(0)) {
            isMove = false;
            lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        } else if (Input.GetMouseButton(0)) {
            frickDist = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - lastPos.x;
            if (!(displayStagePanelIndex == 0 && frickDist > 0.0f) && !(displayStagePanelIndex == stagePanelNum && frickDist < 0.0f)) {
                stageButtonRoot.transform.position += new Vector3(frickDist, 0, 0);
                lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        } else if (Input.GetMouseButtonUp(0)) {
            isMove = true;
            buttonRootX = stageButtonRoot.transform.position.x;
            nearbyStagePanelIndex = (int)((buttonRootX + unitX / 2.0f) / unitX);
            if(nearbyStagePanelIndex > 1) {
                nearbyStagePanelIndex = 1;
            }
        }

        if (isMove) {
            int direction;
            if(stageButtonRoot.transform.position.x > nearbyStagePanelIndex * unitX) {
                direction = -1;
            }else {
                direction = 1;
            }

            stageButtonRoot.transform.position += new Vector3(3.0f * Time.deltaTime * direction, 0, 0);

            if(stageButtonRoot.transform.position.x >= nearbyStagePanelIndex*unitX - 0.2f && stageButtonRoot.transform.position.x <= nearbyStagePanelIndex * unitX + 0.2f) {
                displayStagePanelIndex = nearbyStagePanelIndex;
                stageButtonRoot.transform.position = new Vector3(nearbyStagePanelIndex * unitX, 0, 0);
                isMove = false;
            }
            
        }
	}

	public void StageSelectButton(int stageLevel){
		if (stageLevel == 0) {
			stageLevel = 1;
		}
		StageLevelManager.I.SetStageLevel (stageLevel);
		AppSceneManager.I.GoScene(GameSceneType.GAME_SCENE);
	}

	public void CharacterButton() {
		AppSceneManager.I.GoScene(GameSceneType.CHARACTER_SELECT_SCENE);
	}

	public void StoreButton() {
		AppSceneManager.I.GoScene(GameSceneType.STORE_SCENE);
	}
}

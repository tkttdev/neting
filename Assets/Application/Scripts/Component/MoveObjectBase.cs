using UnityEngine;
using System.Collections;

public enum MoveDir : int {
	UP = 0,
	RIGHT = 1,
	DOWN = 2,
	LEFT = 3,
}
public enum MoveMode : int {
	NORMAL = 0,
	IGNORE = 1,
}

[RequireComponent(typeof(Rigidbody2D))]
public class MoveObjectBase : MonoBehaviour {

	#region Define
	protected enum EffectMode : int {
		LOW_SPEED = 5,
		NORMAL_SPEED = 10,
		HIGH_SPEED = 15,
	}
	#endregion

	#region PubliField
	public MoveMode moveMode = MoveMode.NORMAL;
	[HideInInspector]public string lineId = "";
	[HideInInspector]public MoveDir moveDir = MoveDir.UP;
	[HideInInspector]public Transform[] bezerPoints = new Transform[4];
	[HideInInspector]public float[] lengthOfBezerSection = new float[(Corner.bezerFineness + 1)];
	[HideInInspector]public float onLineLength = 0.0f;
	[HideInInspector]public bool isCurve = false;
	[HideInInspector]public float moveT = 0.0f;
	[HideInInspector]public Vector3 startPos;
	[HideInInspector]public Vector3 endPos;
	#endregion

	#region ProtectedField
	// player to enemy : 1 enemy to player : -1
	protected int moveDesMode = 1;
	protected bool afterWarp = false;
	[Range(0.0f,100.0f)]
	[SerializeField] protected float moveSpeed = 3.0f;
	protected EffectMode effectMode = EffectMode.NORMAL_SPEED;
	#endregion

	#region PrivateField
	[SerializeField] private MoveDir initMoveDir = MoveDir.UP;
	[SerializeField] private MoveMode initMoveMode = MoveMode.NORMAL;
	[SerializeField] private CasheCornerData cornerCashe;
	#endregion


	/// <summary>
	/// Don't use this function to initialize.
	/// </summary>
	private void Awake(){
		Initialize ();
	}


	/// <summary>
	/// To use initialize this function.
	/// </summary>
	protected virtual void Initialize(){
		moveDir = initMoveDir;
		moveMode = initMoveMode;
		effectMode = EffectMode.NORMAL_SPEED;
		moveT = 0.0f;
		isCurve = false;
		startPos = transform.position;
	}

	/// <summary>
	/// If attached object move to Enemy, use this function in initialize.
	/// </summary>
	protected void SetMoveToEnemy(){
		moveDesMode = 1;
	}

	/// <summary>
	/// If attached object move to Player, use this function in initialize.
	/// </summary>
	protected void SetMoveToPlayer(){
		moveDesMode = -1;
	}

	protected virtual void ChangeMoveMode(){
		moveDesMode *= -1;
		if (moveDir == MoveDir.LEFT) {
			moveDir = MoveDir.RIGHT;
		} else if (moveDir == MoveDir.RIGHT) {
			moveDir = MoveDir.LEFT;
		}
	}

	protected virtual void FixedUpdate(){
		if (GameManager.I.CheckGameStatus (GameStatus.PLAY)) {
			if (isCurve) {
				moveT += Time.deltaTime * (moveSpeed / onLineLength);
				moveT = Mathf.Clamp (moveT, 0.0f, 1.0f);
				gameObject.transform.position = Bezer3Interpolate (bezerPoints [0].position, bezerPoints [1].position, bezerPoints [2].position, bezerPoints [3].position, moveT);
			} else {
				moveT += Time.deltaTime * (moveSpeed / onLineLength);
				moveT = Mathf.Clamp (moveT, 0.0f, 1.0f);
				gameObject.transform.position = Vector3.Lerp (startPos, endPos, moveT);
			}
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D _other){
		
		if (_other.tag == "LowSpeedZone") {
			effectMode = EffectMode.LOW_SPEED;
		} else if (_other.tag == "HighSpeedZone") {
			effectMode = EffectMode.HIGH_SPEED;
		}
			
		if (_other.tag == "LeftCorner" || _other.tag == "RightCorner" || _other.tag == "PassCorner") {
			moveT = 0.0f;
			string key = _other.GetInstanceID ().ToString () + moveDir.ToString() + moveMode.ToString() + moveDesMode.ToString();
			if (cornerCashe.straightPurposeData.ContainsKey (key)) {
				isCurve = false;
				endPos = cornerCashe.straightPurposeData [key];
				onLineLength = cornerCashe.lengthData [key];
				lineId = cornerCashe.lineIdData [key];
				moveDir = cornerCashe.moveDirData [key];
			} else if (cornerCashe.curveData.ContainsKey (key)) { 
				isCurve = true;
				bezerPoints = cornerCashe.curveData [key];
				onLineLength = cornerCashe.lengthData [key];
				lengthOfBezerSection = cornerCashe.curveSectionLengthData [key];
				lineId = cornerCashe.lineIdData [key];
				moveDir = cornerCashe.moveDirData [key];
			} else {
				Corner corner = _other.GetComponent<Corner> ();
				if (corner.CheckCurve(moveDir, moveDesMode, moveMode)) {
					isCurve = true;
					bezerPoints = corner.ChangePurposeCurve (moveMode, moveDesMode, ref moveDir, ref lineId, ref onLineLength, ref lengthOfBezerSection);
					cornerCashe.curveData.Add (key, bezerPoints);
					cornerCashe.lengthData.Add (key, onLineLength);
					cornerCashe.curveSectionLengthData.Add (key, lengthOfBezerSection);
					cornerCashe.lineIdData.Add (key, lineId);
					cornerCashe.moveDirData.Add (key, moveDir);
				} else {
					isCurve = false;
					endPos = corner.ChangePurposeStraight (moveMode, moveDesMode, ref moveDir, ref lineId, ref onLineLength);
					cornerCashe.straightPurposeData.Add (key, endPos);
					cornerCashe.lengthData.Add (key, onLineLength);
					cornerCashe.lineIdData.Add (key, lineId);
					cornerCashe.moveDirData.Add (key, moveDir);
				}
			}
			transform.position = _other.transform.position;
			startPos = _other.transform.position;
		} 

		if (_other.tag == "Warp" && moveMode != MoveMode.IGNORE) {
			if (afterWarp) {
				afterWarp = false;
				return;
			}
			SoundManager.I.SoundSE (SE.WARP);
			Warp warp = _other.GetComponent<Warp> ();
			gameObject.transform.position = warp.warpPos;
			lineId = warp.afterWarpLineId;
			afterWarp = true;
		}
	}

	private Vector3 Bezer3Interpolate (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t){
		int k=0;
		float tt;
		for (int i = 0; i < Corner.bezerFineness; i++,k++) {
			if (lengthOfBezerSection [i] <= t && t <= lengthOfBezerSection [i + 1]) {
				break;
			}
		}
		tt = (t - lengthOfBezerSection [k]) / (lengthOfBezerSection [k + 1] - lengthOfBezerSection [k]);
		tt = (k + tt) * 1.0f / Corner.bezerFineness;
		tt = Mathf.Clamp (tt, 0f, 1f);
		return Bezer3 (p0, p1, p2, p3,tt);
	}

	private Vector3 Bezer3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		var oneMinusT = 1f - t;
		return oneMinusT * oneMinusT * oneMinusT * p0 +
			3f * oneMinusT * oneMinusT * t * p1 +
			3f * oneMinusT * t * t * p2 +
			t * t * t * p3;
	}

	protected virtual void OnTriggerExit2D(Collider2D _other){
		bool isCorner = (_other.tag == "LeftCorner" || _other.tag == "RightCorner" || _other.tag == "PassCorner" || _other.tag == "CurveCorner");
		if (isCorner) {
			//isInCorner = false;
		}

		if (_other.tag == "LowSpeedZone" || _other.tag == "HighSpeedZone") {
			effectMode = EffectMode.NORMAL_SPEED;
		}
	}
}

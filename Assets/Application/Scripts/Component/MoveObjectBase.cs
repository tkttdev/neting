﻿using UnityEngine;
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
	[HideInInspector]public Vector2 slope = new Vector2(0.0f, 1f);
	[HideInInspector]public Transform[] bezerPoints = new Transform[4];
	[HideInInspector]public float[] lengthOfBezerSection = new float[(Corner.bezerFineness + 1)];
	[HideInInspector]public float onCurveLength = 0.0f;
	[HideInInspector]public bool isCurve = false;
	[HideInInspector]public float bezerT = 0.0f;
	//For Copy Flag TODO:IMPREMENT COPY WITHOUT THIS FLAG
	//[HideInInspector] public bool isInCorner = false;
	#endregion

	#region ProtectedField
	// player to enemy : 1 enemy to player : -1
	protected int moveDesMode = 1;
	protected bool afterWarp = false;
	[Range(1.0f,50.0f)]
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
		bezerT = 0.0f;
		isCurve = false;
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
				bezerT += Time.deltaTime * (moveSpeed / onCurveLength);
				bezerT = Mathf.Clamp (bezerT, 0.0f, 1.0f);
				gameObject.transform.position = Bezer3Interpolate (bezerPoints [0].position, bezerPoints [1].position, bezerPoints [2].position, bezerPoints [3].position, bezerT);
			} else {
				gameObject.transform.position += (Vector3)slope * (int)effectMode * 0.1f * Time.deltaTime * moveSpeed;
			}
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D _other){
		
		if (_other.tag == "LowSpeedZone") {
			effectMode = EffectMode.LOW_SPEED;
		} else if (_other.tag == "HighSpeedZone") {
			effectMode = EffectMode.HIGH_SPEED;
		}
			
		if ((_other.tag == "LeftCorner" || _other.tag == "RightCorner" || _other.tag == "PassCorner" || (moveMode == MoveMode.IGNORE && _other.tag == "PassCorner"))) {
			string key = _other.GetInstanceID ().ToString () + moveDir.ToString() + moveMode.ToString() + moveDesMode.ToString();
			if (cornerCashe.slopeData.ContainsKey (key)) {
				isCurve = false;
				slope = cornerCashe.slopeData [key];
				lineId = cornerCashe.lineIdData [key];
				moveDir = cornerCashe.moveDirData [key];
			} else if (cornerCashe.curveData.ContainsKey (key)) { 
				isCurve = true;
				bezerT = 0.0f;
				bezerPoints = cornerCashe.curveData [key];
				onCurveLength = cornerCashe.curveLengthData [key];
				lengthOfBezerSection = cornerCashe.curveSectionLengthData [key];
				lineId = cornerCashe.lineIdData [key];
				moveDir = cornerCashe.moveDirData [key];
			} else {
				Corner corner = _other.GetComponent<Corner> ();
				if (corner.CheckCurve(moveDir, moveDesMode, moveMode)) {
					bezerT = 0.0f;
					isCurve = true;
					bezerPoints = corner.ChangePurposeCurve (ref moveDir, moveDesMode, ref lineId, ref onCurveLength, ref lengthOfBezerSection, moveMode);
					cornerCashe.curveData.Add (key, bezerPoints);
					cornerCashe.curveLengthData.Add (key, onCurveLength);
					cornerCashe.curveSectionLengthData.Add (key, lengthOfBezerSection);
					cornerCashe.lineIdData.Add (key, lineId);
					cornerCashe.moveDirData.Add (key, moveDir);
				} else {
					isCurve = false;
					slope = corner.ChangePurposeStraight (ref moveDir, moveDesMode, ref lineId, moveMode);
					cornerCashe.slopeData.Add (key, slope);
					cornerCashe.lineIdData.Add (key, lineId);
					cornerCashe.moveDirData.Add (key, moveDir);
				}
			}
			transform.position = _other.transform.position;
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

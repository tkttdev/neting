using UnityEngine;
using System.Collections;

public enum MoveDir : int {
	UP = 0,
	RIGHT = 1,
	DOWN = 2,
	LEFT = 3,
}

[RequireComponent(typeof(Rigidbody2D))]
public class MoveObjectBase : MonoBehaviour {

	#region Define
	protected enum MoveMode : int {
		NORMAL = 0,
		IGNORE = 1,
	}

	protected enum EffectMode : int {
		LOW_SPEED = 5,
		NORMAL_SPEED = 10,
		HIGH_SPEED = 15,
	}
	#endregion

	#region PubliField
	public string lineId = "";
	public MoveDir moveDir = MoveDir.UP;
	public Vector2 slope = new Vector2(0.0f, 1f);
	//For Copy Flag TODO:IMPREMENT COPY WITHOUT THIS FLAG
	[HideInInspector] public bool isInCorner = false;
	#endregion

	#region ProtectedField
	// player to enemy : 1 enemy to player : -1
	protected int moveDesMode = 1;
	protected bool afterWarp = false;
	[Range(1.0f,12.0f)]
	[SerializeField] protected float moveSpeed = 3.0f;
	protected MoveMode moveMode = MoveMode.NORMAL;
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
		gameObject.transform.position += (Vector3)slope * (int)effectMode * 0.1f * Time.deltaTime;
	}

	protected virtual void OnTriggerEnter2D(Collider2D _other){
		if (_other.tag == "LowSpeedZone") {
			effectMode = EffectMode.LOW_SPEED;
		} else if (_other.tag == "HighSpeedZone") {
			effectMode = EffectMode.HIGH_SPEED;
		}
			
		if ((_other.tag == "LeftCorner" || _other.tag == "RightCorner" || _other.tag == "PassCorner" || (moveMode == MoveMode.IGNORE && _other.tag == "PassCorner")) && !isInCorner) {
			string key = _other.GetInstanceID ().ToString () + moveDir.ToString();
			if (cornerCashe.slopeData.ContainsKey (key)) {
				slope = cornerCashe.slopeData [key];
				lineId = cornerCashe.lineIdData [key];
				moveDir = cornerCashe.moveDirData [key];
			} else {
				Corner corner = _other.GetComponent<Corner> ();
				slope = corner.ChangePurpose (ref moveDir, moveDesMode, ref lineId);
				cornerCashe.slopeData.Add (key, slope);
				cornerCashe.lineIdData.Add (key, lineId);
				cornerCashe.moveDirData.Add (key, moveDir);
			}
			//Corner corner = _other.GetComponent<Corner> ();
			//slope = corner.ChangePurpose (ref moveDir, moveDesMode, ref lineId);
			//cornerCashe.slopeData.Add (key, slope);
			//cornerCashe.lineIdData.Add (key, lineId);
			transform.position = _other.transform.position;
		} else if (_other.tag == "CurveCorner") {
			string key = _other.GetInstanceID ().ToString () + moveDir.ToString();
			transform.position = _other.transform.position;
		}

		if (_other.tag == "Warp") {
			if (afterWarp) {
				afterWarp = false;
				return;
			}
			SoundManager.I.SoundSE (SE.WARP);
			gameObject.transform.position = _other.GetComponent<Warp> ().warpPos;
			afterWarp = true;
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D _other){
		bool isCorner = (_other.tag == "LeftCorner" || _other.tag == "RightCorner" || _other.tag == "PassCorner" || _other.tag == "CurveCorner");
		if (isCorner) {
			isInCorner = false;
		}

		if (_other.tag == "LowSpeedZone" || _other.tag == "HighSpeedZone") {
			effectMode = EffectMode.NORMAL_SPEED;
		}
	}
}

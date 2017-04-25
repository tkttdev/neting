using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveObjectBase : MonoBehaviour {

	// player to enemy : 1 enemy to player : -1
	protected int moveDesMode = 1;
	[Range(1.0f,12.0f)]
	[SerializeField] protected float moveSpeed = 3.0f;
	public int lineLayer = 0;

	protected string tag;

	public enum MoveDir : int {
		FORWARD = 0,
		LEFT = 1,
		RIGHT = 2,
		UP = 3,
		DOWN = 4,
	}

	protected enum MoveMode : int {
		NORMAL = 0,
		IGNORE = 1,
	}

	protected enum EffectMode : int {
		LOW_SPEED = 5,
		NORMAL = 10,
		HIGH_SPEED = 15,
	}

	public MoveDir moveDir = MoveDir.FORWARD;
	protected MoveMode moveMode = MoveMode.NORMAL;
	protected EffectMode effectMode = EffectMode.NORMAL;

	[SerializeField] private MoveDir initMoveDir = MoveDir.FORWARD;
	[SerializeField] private MoveMode initMoveMode = MoveMode.NORMAL;

	//For Copy Flag TODO:IMPREMENT COPY WITHOUT THIS FLAG
	[HideInInspector] public bool isInCorner = false;

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
		effectMode = EffectMode.NORMAL;
		tag = gameObject.tag;
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
			switch (moveDir) {
			case MoveDir.FORWARD:
				gameObject.transform.position += new Vector3 (0, moveDesMode * (int)effectMode * 0.1f * moveSpeed * Time.fixedDeltaTime, 0);
				break;
			case MoveDir.LEFT:
				gameObject.transform.position += new Vector3 (-moveSpeed * (int)effectMode * 0.1f * Time.fixedDeltaTime, 0, 0);
				break;
			case MoveDir.RIGHT:
				gameObject.transform.position += new Vector3 (moveSpeed * (int)effectMode * 0.1f * Time.fixedDeltaTime, 0, 0);
				break;
			case MoveDir.UP:
				gameObject.transform.position += new Vector3 (0, (int)effectMode * 0.1f * moveSpeed * Time.fixedDeltaTime, 0);
				break;
			case MoveDir.DOWN:
				gameObject.transform.position += new Vector3 (0, -(int)effectMode * 0.1f * moveSpeed * Time.fixedDeltaTime, 0);
				break;
			}
		}
	}

	protected bool afterWarp = false;

	protected virtual void OnTriggerEnter2D(Collider2D _other){
		if (_other.tag == "LowSpeedZone") {
			effectMode = EffectMode.LOW_SPEED;
		} else if (_other.tag == "HighSpeedZone") {
			effectMode = EffectMode.HIGH_SPEED;
		}

		if (moveMode == MoveMode.IGNORE) {
			return;
		} else if (_other.tag == "LeftCorner" && !isInCorner) {
			//isInCorner = true;
			gameObject.transform.position = _other.transform.position;
			if (moveDir == MoveDir.FORWARD || moveDir == MoveDir.DOWN || moveDir == MoveDir.UP) {
				moveDir = MoveDir.LEFT;
			} else {
				moveDir = MoveDir.FORWARD;
			}
		} else if (_other.tag == "RightCorner" && !isInCorner) {
			//isInCorner = true;
			gameObject.transform.position = _other.transform.position;
			if (moveDir == MoveDir.FORWARD || moveDir == MoveDir.DOWN || moveDir == MoveDir.UP) {
				moveDir = MoveDir.RIGHT;
			} else {
				moveDir = MoveDir.FORWARD;
			}
		} else if (_other.tag == "UpLeftCorner" && !isInCorner) {
			//isInCorner = true;
			gameObject.transform.position = _other.transform.position;
			if (moveDir == MoveDir.RIGHT) {
				moveDir = MoveDir.UP;	
			} else {
				moveDir = MoveDir.LEFT; 
			}
		} else if (_other.tag == "UpRightCorner" && !isInCorner) {
			//isInCorner = true;
			gameObject.transform.position = _other.transform.position;
			if (moveDir == MoveDir.LEFT) {
				moveDir = MoveDir.UP;	
			} else {
				moveDir = MoveDir.RIGHT; 
			}
		} else if (_other.tag == "DownLeftCorner" && !isInCorner) {
			//isInCorner = true;
			gameObject.transform.position = _other.transform.position;
			if (moveDir == MoveDir.RIGHT) {
				moveDir = MoveDir.DOWN;	
			} else {
				moveDir = MoveDir.LEFT; 
			}
		} else if (_other.tag == "DownRightCorner" && !isInCorner) {
			//isInCorner = true;
			gameObject.transform.position = _other.transform.position;
			if (moveDir == MoveDir.LEFT) {
				moveDir = MoveDir.DOWN;	
			} else {
				moveDir = MoveDir.RIGHT; 
			}
		} else if (_other.tag == "LeftEnemyTunnel" && tag == "Enemy" && !isInCorner) { 
			//isInCorner = true;
			gameObject.transform.position = _other.transform.position;
			if (moveDir == MoveDir.FORWARD || moveDir == MoveDir.DOWN || moveDir == MoveDir.UP) {
				moveDir = MoveDir.LEFT;
			} else {
				moveDir = MoveDir.FORWARD;
			}
		} else if (_other.tag == "RightEnemyTunnel" && tag == "Enemy" && !isInCorner) {
			//isInCorner = true;
			gameObject.transform.position = _other.transform.position;
			if (moveDir == MoveDir.FORWARD || moveDir == MoveDir.DOWN || moveDir == MoveDir.UP) {
				moveDir = MoveDir.RIGHT;
			} else {
				moveDir = MoveDir.FORWARD;
			}
		} else if (_other.tag == "LeftPlayerTunnel" && tag == "PlayerBullet" && !isInCorner) {
			//isInCorner = true;
			gameObject.transform.position = _other.transform.position;
			if (moveDir == MoveDir.FORWARD || moveDir == MoveDir.DOWN || moveDir == MoveDir.UP) {
				moveDir = MoveDir.LEFT;
			} else {
				moveDir = MoveDir.FORWARD;
			}
		} else if (_other.tag == "RightPlayerTunnel" && tag == "PlayerBullet" && !isInCorner) {
			//isInCorner = true;
			gameObject.transform.position = _other.transform.position;
			if (moveDir == MoveDir.FORWARD || moveDir == MoveDir.DOWN || moveDir == MoveDir.UP) {
				moveDir = MoveDir.RIGHT;
			} else {
				moveDir = MoveDir.FORWARD;
			}
		} else if (_other.tag == "ForwardCorner" && !isInCorner) { 
			//isInCorner = true;
			gameObject.transform.position = _other.transform.position;
			moveDir = MoveDir.FORWARD;
		} else if (_other.tag == "Warp") {
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
		bool isCorner = (_other.tag == "LeftCorner" || _other.tag == "RightCorner" || _other.tag == "UpLeftCorner" || _other.tag == "UpRightCorner" || _other.tag == "DownLeftCorner" || _other.tag == "DownRightCorner" || _other.tag == "ForwardCorner");
		bool isTunnel = (_other.tag == "LeftEnemyTunnel" || _other.tag == "RightEnemyTunnel" || _other.tag == "LeftPlayerTunnel" || _other.tag == "RightPlayerTunnel");
		if (isCorner || isTunnel) {
			isInCorner = false;
		}

		if (_other.tag == "LowSpeedZone" || _other.tag == "HighSpeedZone") {
			effectMode = EffectMode.NORMAL;
		}
	}


}

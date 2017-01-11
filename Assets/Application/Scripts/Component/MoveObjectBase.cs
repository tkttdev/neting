using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveObjectBase : MonoBehaviour {

	// player to enemy : 1 enemy to player : -1
	protected int moveDesMode = 1;
	[Range(2.0f,20.0f)]
	[SerializeField] protected float moveSpeed = 3.0f;

	public enum MoveDir : int {
		FORWARD = 0,
		LEFT = 1,
		RIGHT = 2,
	}

	protected enum MoveMode : int {
		NORMAL = 0,
		IGNORE = 1,
	}

	public MoveDir moveDir = MoveDir.FORWARD;
	protected MoveMode moveMode = MoveMode.NORMAL;

	[SerializeField] private MoveDir initMoveDir = MoveDir.FORWARD;
	[SerializeField] private MoveMode initMoveMode = MoveMode.NORMAL;

	[HideInInspector] public bool isTriggerEnter2D = false;

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

	protected virtual void Update(){
		switch (moveDir) {
		case MoveDir.FORWARD :
			gameObject.transform.position += new Vector3 (0, moveDesMode * moveSpeed * Time.deltaTime, 0);
			break;
		case MoveDir.LEFT:
			gameObject.transform.position += new Vector3 (-moveSpeed * Time.deltaTime, 0, 0);
			break;
		case MoveDir.RIGHT:
			gameObject.transform.position += new Vector3 (moveSpeed * Time.deltaTime, 0, 0);
			break;
		}
	}

	bool afterWarp = false;

	protected virtual void OnTriggerEnter2D(Collider2D _other){
		if (moveMode == MoveMode.IGNORE || isTriggerEnter2D) {
			return;
		} else if (_other.tag == "LeftCorner") {
			gameObject.transform.position = _other.transform.position;
			if (moveDir == MoveDir.FORWARD) {
				moveDir = MoveDir.LEFT;
			} else {
				moveDir = MoveDir.FORWARD;
			}
		} else if (_other.tag == "RightCorner") {
			gameObject.transform.position = _other.transform.position;
			if (moveDir == MoveDir.FORWARD) {
				moveDir = MoveDir.RIGHT;
			} else {
				moveDir = MoveDir.FORWARD;
			}
		} else if (_other.tag == "Warp") {
			if (afterWarp) {
				afterWarp = false;
				return;
			}
			gameObject.transform.position = _other.GetComponent<Warp> ().warpPos;
			afterWarp = true;
		}

		isTriggerEnter2D = true;

	}

	protected virtual void OnTriggerExit2D(){
		isTriggerEnter2D = false;
	}


}

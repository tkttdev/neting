using UnityEngine;
using System.Collections;

public class PlayerBullet : MoveObjectBase {

	protected override void Initialize (){
		base.Initialize ();
		SetMoveToEnemy ();
	}

	protected override void Update (){
		base.Update ();
	}
}

using UnityEngine;
using System.Collections;

public class Enemy : MoveObjectBase {

	protected override void Initialize (){
		base.Initialize ();
		SetMoveToPlayer ();
	}

	protected override void Update (){
		base.Update ();
	}
}

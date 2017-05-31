using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : Corner {

	#region public_field
	public static List<int> warpObjectsKey = new List<int> ();
	public Transform warpPurpose;
	[HideInInspector] public Vector3 warpPurposePos;
	#endregion

	protected override void Awake () {
		warpPurposePos = warpPurpose.position;
		base.Awake ();
	}

	public override Vector3 ChangePurposeStraight (MoveMode _moveMode, int _moveDesMode, ref MoveDir _moveDir, ref string _lineId, ref float _lineLength) {
		return base.ChangePurposeStraight (_moveMode, _moveDesMode, ref _moveDir, ref _lineId, ref _lineLength);
	}

	public override Transform[] ChangePurposeCurve (MoveMode _moveMode, int _moveDesMode, ref MoveDir _moveDir, ref string _lineId, ref float _lineLength, ref float[] _lengthOfBezerSection) {
		return base.ChangePurposeCurve (_moveMode, _moveDesMode, ref _moveDir, ref _lineId, ref _lineLength, ref _lengthOfBezerSection);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlphaChange : EnemyEffectBase {

	private enum ChangeMode : int {
		HIGH_TO_LOW = -1,
		LOW_TO_HIGH = 1,
	}
		
	[SerializeField]private ChangeMode changeMode = ChangeMode.HIGH_TO_LOW;
	[SerializeField] private float changeRateParsec;
	[SerializeField] private bool isLoop = false;
	private bool isChange = true;
	private float enemyAlpha;
	private SpriteRenderer spriteRenderer;

	private void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
	}

	public override void MoveEffect (){
		if (isChange) {
			enemyAlpha = spriteRenderer.color.a;
			enemyAlpha += (int)changeMode * changeRateParsec * Time.deltaTime;

			switch (changeMode) {
			case ChangeMode.HIGH_TO_LOW:
				if (enemyAlpha <= 0.0f) {
					enemyAlpha = 0.0f;
					if (isLoop) {
						changeMode = ChangeMode.LOW_TO_HIGH;
					} else {
						isChange = false;
					}
				}
				break;
			case ChangeMode.LOW_TO_HIGH:
				if (enemyAlpha >= 1.0f) {
					enemyAlpha = 1.0f;
					if (isLoop) {
						changeMode = ChangeMode.HIGH_TO_LOW;
					} else {
						isChange = false;
					}
				}
				break;
			}

			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, enemyAlpha);
		}
	}
}

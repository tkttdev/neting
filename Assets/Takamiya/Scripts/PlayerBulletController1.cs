using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// pos -2,-1,,,,2
/// </summary>

public class PlayerBulletController1 : SingletonBehaviour<PlayerBulletController1>{

	[SerializeField] private int fullTime = 2;
	private int count = 0;
	private int entryX = 0;
	private float playerX = 0;
	[SerializeField] private Text countText;
	[SerializeField] private GameObject playerBullet;

	protected override void Initialize(){
		count = 0;
		countText.text = "IN " + count.ToString() + "sec";
		playerX = gameObject.transform.position.x;
	}

	void Update () {
		
		if (count == 0 && Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast ((Vector2)ray.origin, (Vector2)ray.direction, 100);
			if (hit.collider) {
				return;
			} else {
				if (Input.mousePosition.x < Screen.width / 2.0f) {
					if (playerX > -2) {
						playerX -= 1.0f;
					}
				} else {
					if (playerX < 2) {
						playerX += 1.0f;
					}
				}
				gameObject.transform.position = new Vector3 (playerX, gameObject.transform.position.y, 0);
			}
		}
	}

	public void Shoot(){
		Instantiate (playerBullet, new Vector3 (playerX, -3.0f, 0.0f), Quaternion.Euler (0, 0, 0));
	}
}

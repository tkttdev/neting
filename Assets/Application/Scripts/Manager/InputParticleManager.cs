﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputParticleManager : MonoBehaviour {

	[SerializeField] private GameObject particlePrefab;
	private ParticleSystem[] particlePool = new ParticleSystem[10];

	private int nextParticleIndex = 0;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {

			Vector3 generateParticlePos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10));
			Vector3 localScale = new Vector3 (Camera.main.orthographicSize / 5.0f, Camera.main.orthographicSize / 5.0f, Camera.main.orthographicSize / 5.0f);

			if (particlePool [nextParticleIndex] == null) {
				GameObject prtcl = Instantiate (particlePrefab, generateParticlePos, Quaternion.identity);
				prtcl.transform.parent = gameObject.transform;
				prtcl.transform.localScale = localScale;
				particlePool [nextParticleIndex] = prtcl.GetComponent<ParticleSystem> ();
			} else {
				particlePool [nextParticleIndex].transform.position = generateParticlePos;
				particlePool [nextParticleIndex].transform.localScale = localScale;
				particlePool [nextParticleIndex].Play ();
			}
			nextParticleIndex = (nextParticleIndex + 1) % 10;
		}
	}
}

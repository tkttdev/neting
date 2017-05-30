using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour {

	[SerializeField] private GameObject locus;

	private int i;
	private bool warp;
	private Vector3 zero;
	private GameObject[] prefab = new GameObject[100];

	// Use this for initialization
	void Start() {
		i = 0;
		zero = gameObject.transform.position;
		StartCoroutine("EndThunder");
	}

	// Update is called once per frame
	void Update() {

	}

	void OnEnable() {
		Start();
	}

	void OnTriggerEnter2D(Collider2D _other) {
		if ((_other.tag == "LeftCorner" || _other.tag == "RightCorner" || _other.tag == "PassCorner" || _other.tag == "Warp" || _other.tag == "DestroyZone")) {
			Vector3 one = gameObject.transform.position;
			Vector3 dif = one - zero;
			float dis = dif.magnitude;
			float angle = Vector3.Angle(Vector3.up, dif);
			if (dif.x > 0) {
				angle *= -1;
			}

			if (warp == false) {
				prefab[i] = ObjectPool.I.Instantiate(locus, zero) as GameObject;
				prefab[i].transform.localScale = new Vector3(0.05f, dis / 1.45f, 0);
				prefab[i].transform.Rotate(new Vector3(0, 0, angle));
			}

			if (_other.tag == "Warp" && warp == false) {
				warp = true;
			} else {
				warp = false;
			}

			i++;
			zero = gameObject.transform.position;
		}
	}

	public IEnumerator EndThunder() {
		yield return new WaitForSeconds(2.0f);

		for (int j = 0; j < i; j++) {
			prefab[j].transform.rotation = Quaternion.Euler(0, 0, 0);
			ObjectPool.I.Release(prefab[j]);
		}

		ObjectPool.I.Release(gameObject);
	}
}

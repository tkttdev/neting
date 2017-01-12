using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBase : MonoBehaviour {

	private int pri_id = 1;
	public int pub_id = 1;

	// Use this for initialization
	void Start () {
		pri_id = 2;
		pub_id = 2;

		Instantiate (gameObject);

		Debug.Log (pri_id);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

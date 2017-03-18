using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : SingletonBehaviour<ObjectPool> {

	private Dictionary<int, List<GameObject>> pooledObjects = new Dictionary<int,List<GameObject>>();
	//private List<GameObject> pool = new List<GameObject> ();

	public GameObject Instantiate(GameObject _gameObjectPrefab, Vector3 _position){
		int key = _gameObjectPrefab.GetInstanceID ();

		if (!pooledObjects.ContainsKey (key)) {
			pooledObjects.Add (key, new List<GameObject> ());
		}

		List<GameObject> pool = pooledObjects [key];

		for (int i = 0; i < pool.Count; i++) {
			if (!pool [i].activeInHierarchy) {
				pool [i].transform.position = _position;
				pool [i].SetActive (true);
				return pool [i];
			}
		}

		GameObject obj = Instantiate (_gameObjectPrefab, _position, Quaternion.identity);
		pool.Add (obj);
		return obj;
	}

	public void Release(GameObject _gameObject){
		_gameObject.SetActive (false);
	}

}

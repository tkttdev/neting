using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	GameObject bulletPrefab;

	public BulletController(string _bulletPrefabPath){
		bulletPrefab = Resources.Load (_bulletPrefabPath) as GameObject;
	}

	ObjectPool objectPool = new ObjectPool();

	public void Shoot(Vector3 _position){
		GameObject obj = objectPool.Instantiate (bulletPrefab, _position);
	}
}

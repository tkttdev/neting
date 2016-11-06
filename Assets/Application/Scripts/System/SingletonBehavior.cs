using UnityEngine;
using System;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
	protected static T instance;
	private bool isInitialized = false;

	public static T I {
		get {
			if (instance == null) {
				instance = (T)FindObjectOfType (typeof(T));

				if (instance == null) {
					Debug.LogWarning (typeof(T) + "is nothing");
				}
			}

			return instance;
		}
	}

	/// <summary>
	/// 初期化
	/// </summary>
	protected virtual void Initialize(){}

	protected virtual void Awake (){
		if (!CheckInstance () || !isInitialized) {
			this.Initialize();
			this.isInitialized = true;
		}
	}

	protected bool CheckInstance ()
	{
		if (I != null && this == I) {
			return true;
		}
		Destroy(gameObject);
		return false;
	}

	virtual protected void OnDestroy() {
		if (instance != null) {
			instance = null;
		}
	}
}

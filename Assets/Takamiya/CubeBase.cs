using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBase : MonoBehaviour {

	void Start(){
		A a = new A();
		A b = (A)new B();

		//a.test ();
		b.test ();

		//B b_ = b as B;
		//b_.test ();
	}

	public class A{
		public virtual void test(){
			Debug.Log ("This is a A");
		}
	}

	public class B : A{
		public override void test () {
			base.test ();
			Debug.Log("This is a B");
		}
	}
}

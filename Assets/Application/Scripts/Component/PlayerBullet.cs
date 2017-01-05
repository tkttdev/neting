using UnityEngine;
using System.Collections;

public class PlayerBullet : MoveObjectBase {

    [SerializeField] private int damage = 100;

	protected override void Initialize (){
		base.Initialize ();
		SetMoveToEnemy ();
	}

	protected override void Update (){
		base.Update ();
	}

    protected override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
        if(other.tag == "Enemy") {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

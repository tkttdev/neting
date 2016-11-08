using UnityEngine;
using System.Collections;

public class PlayerBulleByShimizu : MoveObjectBase
{

    protected override void Initialize(){
        base.Initialize();
        SetMoveToEnemy();
    }

    protected override void Update(){
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D other){
        base.OnTriggerEnter2D(other);
        if (other.tag == "Enemy")
        {
            GameObject.Destroy(gameObject);
        }
    }

}
 
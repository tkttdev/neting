using UnityEngine;
using System.Collections;

public class EnemyByShimizu : MoveObjectBase{

    protected override void Initialize()
    {
        base.Initialize();
        SetMoveToPlayer();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag == "RightCorner" || other.tag == "LeftCorner")
        {
            this.GetComponent<Transform>().position = other.GetComponent<Transform>().position;
        }
        if (other.tag == "PlayerBullet")
        {
            GameObject.Destroy(gameObject);
        }
    }
}

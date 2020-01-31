using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CommonEnemy : Entity
{
    public LayerMask targetLayer; // 대상 레이어


    public override void OnDamage(int damage, Vector2 hitPoint)
    {
        Vector2 direction = hitPoint - rigid.position;
        direction.Normalize();

        rigid.AddForce(direction * 2, ForceMode2D.Impulse);

        base.OnDamage(damage, hitPoint);
    }
}

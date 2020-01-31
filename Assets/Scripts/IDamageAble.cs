using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamageAble
{
    // damage : 데미지 크기
    // hitPoint : 공격자의 위치
    void OnDamage(int damage, Vector2 hitPoint);
}

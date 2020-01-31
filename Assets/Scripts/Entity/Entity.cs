using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageAble
{
    public int hp, damage;
    public float moveSpeed, jumpPower;
    public float timeBetAttack; // 공격 간격
    public LayerMask platformLayer, wallLayer; // 밟을 수 있는 레이어, 점프로 통과할 수 없는 레이어

    public bool canOnDamage; // 피해를 받을 수 있는지 여부
    public bool dead; // 죽음 여부

    protected Animator anim;
    protected Rigidbody2D rigid;


    protected void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }


    // 외부에서 Entity를 공격하는 데 사용하는 함수
    // 상속받은 클래스는 재정의해서 사용
    public virtual void OnDamage(int damage, Vector2 hitPoint)
    {
        hp -= damage;

        if(hp <= 0 && !anim.GetBool("Dead"))
        {
            anim.SetTrigger("Die");
            anim.SetBool("Dead", true);
            dead = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rigid;
    CollisionRayCheck collisionCheck;
    PlayerStatus playerStatus;
    SpriteRenderer spriteRenderer;

    PlayerChain playerChain;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        collisionCheck = GetComponent<CollisionRayCheck>();
        playerStatus = GetComponent<PlayerStatus>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerChain = transform.Find("PlayerChain").GetComponent<PlayerChain>();

        anim.SetBool("CanRoll", true);
    }


    private void Update()
    {
        UpdateAnimatorParameters();
    }

    
    private void FixedUpdate()
    {
        // 현재 animator의 State가 이동 가능 상태인지 확인
        if (anim.GetCurrentAnimatorStateInfo(0).tagHash.Equals(Animator.StringToHash("CanMove")))
        {
            Move();
        }
    }


    public void DoRoll()
    {
        StartCoroutine("RollCooldownUpdate");
    }


    IEnumerator RollCooldownUpdate()
    {
        anim.SetBool("CanRoll", false);
        yield return new WaitForSeconds(playerStatus.rollCooldown);
        anim.SetBool("CanRoll", true);
    }


    public void DoShotChain()
    {
        playerChain.DoShotChain();
    }


    void Move()
    {
        BoxCollider2D box2d = GetComponent<BoxCollider2D>();
        Bounds bounds = box2d.bounds;

        Vector2 rayOrigin = Vector2.zero;
        Vector2 moveDirection = new Vector2(GameManager.horizontal, 0);

        // betweenPlayerAndWall의 가능한 최대 크기는 moveSpeed * Time.fixedDeltaTime과 같음
        float betweenPlayerAndWall = playerStatus.moveSpeed * Time.fixedDeltaTime;
        bool canMove = true;

        rayOrigin.x = GameManager.horizontal > 0 ? bounds.max.x : bounds.min.x;
        rayOrigin.y = bounds.min.y;

        // Ray를 이동방향으로 collider 크기에 맞춰 발사
        for (int i = 0; i <= 4; i++)
        {
            Vector2 newRayOrigin = new Vector2(rayOrigin.x, rayOrigin.y + bounds.size.y * 0.25f * i);
            RaycastHit2D hit = Physics2D.Raycast(newRayOrigin, moveDirection, Mathf.Abs(GameManager.horizontal) * playerStatus.moveSpeed * Time.fixedDeltaTime, playerStatus.wallLayer);
            Debug.DrawRay(newRayOrigin, moveDirection * playerStatus.moveSpeed * Time.fixedDeltaTime, Color.red);

            // Ray 충돌 시
            if (hit)
            {
                // 제일 가까운 벽과의 거리 구하기
                betweenPlayerAndWall = betweenPlayerAndWall > hit.distance ? hit.distance : betweenPlayerAndWall;
                canMove = false;
            }
        }

        if (canMove)
            rigid.position += new Vector2(GameManager.horizontal * playerStatus.moveSpeed * Time.fixedDeltaTime, 0);
        else if(Mathf.Abs(moveDirection.x) * betweenPlayerAndWall >= 0.015f)
            rigid.position += new Vector2(moveDirection.x * betweenPlayerAndWall, 0);

        // 이동 방향으로 스프라이트 뒤집기
        if (GameManager.horizontal > 0)
            spriteRenderer.flipX = true;
        else if (GameManager.horizontal < 0)
            spriteRenderer.flipX = false;

    }


    void UpdateAnimatorParameters()
    {
        anim.SetBool("IsGround", collisionCheck.collisions.below);
        anim.SetBool("CollisionLeft", collisionCheck.collisions.left);
        anim.SetBool("CollisionRight", collisionCheck.collisions.right);
        anim.SetBool("Move", GameManager.horizontal != 0);

        anim.SetFloat("VelocityY", rigid.velocity.y);
        anim.SetFloat("MoveSpeed", Mathf.Abs(GameManager.horizontal));
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStates
{
    public class PlayerRoll : PlayerStateBase
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
           
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 점프 입력 시
            if (Input.GetKeyDown(KeyCode.W))
            {
                animator.SetTrigger("Jump");
            }

            Move(animator);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<PlayerControl>().DoRoll();
            animator.ResetTrigger("Roll");
        }

        void Move(Animator anim)
        {
            PlayerStatus playerStatus = anim.GetComponent<PlayerStatus>();
            Rigidbody2D rigid = anim.GetComponent<Rigidbody2D>();
            SpriteRenderer spriteRenderer = anim.GetComponent<SpriteRenderer>();

            BoxCollider2D box2d = anim.GetComponent<BoxCollider2D>();
            Bounds bounds = box2d.bounds;

            Vector2 rayOrigin = Vector2.zero;
            Vector2 moveDirection = new Vector2(spriteRenderer.flipX ? 1 : -1, 0);

            float multiply = 2f; // 이동 배속

            // betweenPlayerAndWall의 가능한 최대 크기는 moveSpeed * multiply * Time.deltaTime과 같음
            float betweenPlayerAndWall = playerStatus.moveSpeed * multiply * Time.deltaTime;
            bool canMove = true;

            rayOrigin.x = moveDirection.x > 0 ? bounds.max.x : bounds.min.x;
            rayOrigin.y = bounds.min.y;

            // Ray를 이동방향으로 collider 크기에 맞춰 발사
            for (int i = 0; i <= 4; i++)
            {
                Vector2 newRayOrigin = new Vector2(rayOrigin.x, rayOrigin.y + bounds.size.y * 0.25f * i);
                RaycastHit2D hit = Physics2D.Raycast(newRayOrigin, moveDirection, Mathf.Abs(moveDirection.x) * playerStatus.moveSpeed * multiply * Time.deltaTime, playerStatus.wallLayer);
                Debug.DrawRay(newRayOrigin, moveDirection * playerStatus.moveSpeed * multiply * Time.deltaTime, Color.red);

                // Ray 충돌 시
                if (hit)
                {
                    // 제일 가까운 벽과의 거리 구하기
                    betweenPlayerAndWall = betweenPlayerAndWall > hit.distance ? hit.distance : betweenPlayerAndWall;
                    canMove = false;
                }
            }

            if (canMove)
                rigid.position += new Vector2(moveDirection.x * playerStatus.moveSpeed * multiply * Time.deltaTime, 0);
            else if (Mathf.Abs(moveDirection.x) * betweenPlayerAndWall >= 0.015f)
                rigid.position += new Vector2(moveDirection.x * betweenPlayerAndWall, 0);
        }

    }
}

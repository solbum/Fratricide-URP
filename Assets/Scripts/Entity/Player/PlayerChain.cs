using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChain : MonoBehaviour
{
    LineRenderer lineRenderer;

    Rigidbody2D rigid;
    PlayerStatus playerStatus;
    SpriteRenderer spriteRenderer;
    Animator anim;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        rigid = transform.parent.GetComponent<Rigidbody2D>();
        playerStatus = transform.parent.GetComponent<PlayerStatus>();
        spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        anim = transform.parent.GetComponent<Animator>();

        lineRenderer.enabled = false; // LineRenderer 비활성화
    }

    public void DoShotChain()
    {
        StartCoroutine("ShotChain");
    }

    // Chain 발사 코루틴
    IEnumerator ShotChain()
    {
        Vector2 direction = GameManager.mousePosition - rigid.position; // Chain 발사 방향 벡터
        direction.Normalize(); // 벡터 정규화

        bool isHit = false; // 충돌 여부 검사

        lineRenderer.enabled = true; // LineRenderer 활성화

        anim.SetTrigger("ShotChain");

        // FixedUpdate는 초당 60번
        for (int i = 0; i <= 60 * playerStatus.chainDuration; i++)
        {
            // 발사 방향으로 스프라이트 방향 뒤집기
            if (direction.x > 0)
                spriteRenderer.flipX = true;
            else if (direction.x < 0)
                spriteRenderer.flipX = false;

            float length = i * playerStatus.chainMaxLength / (60 * playerStatus.chainDuration);

            // Chain의 Line 생성
            lineRenderer.SetPosition(0, rigid.position);
            lineRenderer.SetPosition(1, rigid.position + direction * length);
            // LineRenderer 충돌 체크
            RaycastHit2D hit = Physics2D.Raycast(rigid.position, direction, length, playerStatus.canChainLayer);

            yield return new WaitForFixedUpdate();

            // Chain 충돌 시
            if (hit)
            {
                StartCoroutine(MoveChain(hit.point));
                isHit = true;
                break;
            }
        }

        // Chain이 충돌 하지 않았으면
        if (!isHit)
        {
            lineRenderer.enabled = false; // LineRenderer 비활성화
            anim.SetTrigger("NotHitShotChain");
        }
    }

    // Chain 적중 시 이동 코루틴
    IEnumerator MoveChain(Vector2 point)
    {
        anim.SetTrigger("MoveChain");

        Vector2 velocity = Vector2.zero; // smoothdamp의 참조 변경을 위한 벡터

        Vector2 direction = point - rigid.position; // 방향 벡터
        direction.Normalize(); // 벡터 정규화

        lineRenderer.SetPosition(1, point); // 도달 지점에 Chain 연결
        point.y += 1f; // 이동 보정 값 설정, 실제 이동 위치는 +1만큼 더 위쪽으로 이동하게 됨

        // 이동 방향으로 스프라이트 방향 뒤집기
        if (direction.x > 0)
            spriteRenderer.flipX = true;
        else if (direction.x < 0)
            spriteRenderer.flipX = false;

        // 점프나 낙하로 상태 전이 시 종료
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
        {
            lineRenderer.SetPosition(0, rigid.position); // 플레이어 위치에 Chain이 지속시간동안 유지되도록 함

            float speedRatio = (direction.y + 1) * 0.5f; // 방향 벡터 -1 ~ 1 사이의 비율

            // 목표 위치로 감속 이동
            rigid.position = Vector2.SmoothDamp(rigid.position, point, ref velocity, Mathf.Lerp(playerStatus.minChainMoveSpeed, playerStatus.maxChainMoveSpeed, speedRatio));
            rigid.velocity = Vector2.zero; // 작용 힘 초기화

            yield return new WaitForFixedUpdate();
        }

        lineRenderer.enabled = false; // LineRenderer 비활성화
    }
}

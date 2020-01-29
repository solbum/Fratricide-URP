﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Entity
{
    // 내부 컴포넌트
    Rigidbody2D rigid;

    public LayerMask canChainLayer; // Chain 적중 가능 레이어

    public float timeToJumpApex; // 최고 위치까지 점프로 올라가는데 걸리는 시간
    public float chainMaxLength; // PlayerChain 발사 최고 길이
    public float chainDuration; // PlayerChain 발사 지속 시간
    public float minChainMoveSpeed, maxChainMoveSpeed; // MoveChain 이동 최소, 최대 속도 (각도에 따른 속도 결정)
    public float rollCooldown; // 구르기 재사용 대기시간

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();   
    }

    void Update()
    {
        rigid.gravityScale = (2 * jumpPower) / Mathf.Pow(timeToJumpApex, 2);
    }
}

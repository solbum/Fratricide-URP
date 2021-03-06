﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStates
{
    public class PlayerRun : PlayerStateBase
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("Roll");

            // 점프 입력 시
            if (Input.GetKeyDown(KeyCode.W))
            {
                animator.SetTrigger("Jump");
            }

            // 구르기 입력 시
            if (Input.GetKeyDown(KeyCode.S))
            {
                animator.SetTrigger("Roll");
            }

            // ShotChain 입력 시
            if (Input.GetMouseButtonUp(1))
            {
                animator.GetComponent<PlayerControl>().DoShotChain();
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }
    }
}

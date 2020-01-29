using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStates
{
    public class PlayerJump : PlayerStateBase
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Rigidbody2D rigid = animator.GetComponent<Rigidbody2D>();
            PlayerStatus playerStatus = animator.GetComponent<PlayerStatus>();

            rigid.velocity = Vector2.zero;
            rigid.AddForce(Vector2.up * playerStatus.jumpPower, ForceMode2D.Impulse);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
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

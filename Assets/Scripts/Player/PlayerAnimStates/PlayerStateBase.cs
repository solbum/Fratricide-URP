using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateBase : StateMachineBehaviour
{
    private PlayerControl playerControl;

    public PlayerControl GetPlayerControl(Animator animator)
    {
        if(playerControl == null)
        {
            playerControl = animator.GetComponent<PlayerControl>();
        }

        return playerControl;
    }
}

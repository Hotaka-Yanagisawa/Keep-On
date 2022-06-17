using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool("knockback", false);
        animator.SetBool("standup", true);
    }
}

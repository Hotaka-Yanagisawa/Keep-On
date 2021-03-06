/////////////////////////////////////////////////////////////////////////
// ì¬ú 2021/04/02
// ì¬Ò öò¿M
/////////////////////////////////////////////////////////////////////////
// XVú
//
// 2021/04/02 ì¬Jn
//            ³UV½Éì¬iUÍÍ³¢j
// 
//
//
//////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using State = StateMachine<PBO>.State;
public partial class PBO
{
    // Ò@óÔ
    private class PBOStateWindAtk : State
    {
        int actionCnt;
        protected override void OnEnter(State prevState)
        {
            Debug.Log("³U");
            Owner.rb.velocity = Vector3.zero;
            Owner.animator.SetFloat("speed", 0);
            actionCnt = 60;

            Owner.rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }

        protected override void OnFixedUpdate()
        {
            actionCnt--;

            if (actionCnt <= 30)
            {
                if (actionCnt == 30)
                {
                    //U»èðOnÉ·é
                    Owner.sphereCollider.enabled = true;
                }
            }

            if (actionCnt < 1)
            {
                StateMachine.Dispatch((int)Event.Timeout);
            }
        }
        protected override void OnExit(State nextState)
        {
            Owner.sphereCollider.enabled = false;
        }
    }
}
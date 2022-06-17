/////////////////////////////////////////////////////////////////////////
// 作成日 2021/04/02
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/04/02 作成開始
//            攻撃後の後隙
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
    // 待機状態
    private class PBOStateAtkDelay : State
    {
        int actionCnt;
        protected override void OnEnter(State prevState)
        {
            Debug.Log(prevState + "から来た");
            //bool a = prevState is PBOStateAttacking;
            Owner.rb.velocity = Vector3.zero;
            Owner.animator.SetFloat("speed", 0);
            actionCnt = 60;
        }

        protected override void OnFixedUpdate()
        {
            actionCnt--;

            if (actionCnt < 1)
            {
                StateMachine.Dispatch((int)Event.Timeout);
            }
        }
        protected override void OnExit(State nextState)
        {
            
        }
    }
}
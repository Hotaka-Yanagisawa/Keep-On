/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/28
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/03/028 作成開始
//            
// 
//
//
//////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homare
{
using State = StateMachine<Boss>.State;
    public partial class Boss
    {
        // 待機状態
        private class BossStateWaiting : State
        {
            float transitionCnt;
            protected override void OnEnter(State prevState)
            {
                Debug.Log("待機");
                transitionCnt = 1;
                Owner.rb.velocity = Vector3.zero;               
            }

            protected override void OnFixedUpdate()
            {
                Owner.rb.velocity = Vector3.zero;

                //if (!Owner.animator.GetBool("Change"))
                //{
                transitionCnt -= Time.deltaTime;
                //}
                if (!Owner.animator.GetBool("Change"))
                {
                    StateMachine.Dispatch((int)Event.Attack);
                    //StateMachine.Dispatch((int)Event.Timeout);
                }
            }
        }
    }
}
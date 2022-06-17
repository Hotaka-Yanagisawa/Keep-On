/////////////////////////////////////////////////////////////////////////
// 作成日 2021/04/02
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/04/02 作成開始
//            BossScript関係の汎用的な行動AIをコピペ
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
    private class PBOStateWaiting : State
    {
        int transitionCnt;
        protected override void OnEnter(State prevState)
        {
            Debug.Log("待機");
            
            transitionCnt = 60;
            Owner.rb.velocity = Vector3.zero;
            Owner.animator.SetFloat("speed", 0);
            //Owner.animator.SetBool("Attack", true);
        }

        protected override void OnFixedUpdate()
        {

            transitionCnt--;
            if (transitionCnt <= 0)
            {
                StateMachine.Dispatch((int)Event.Timeout);
            }
        }
    }
}
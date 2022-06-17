/////////////////////////////////////////////////////////////////////////
// 作成日 2021/04/02
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/04/02 作成開始
//            風圧攻撃新たに作成（攻撃力は無い）
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
    private class PBOStateWindAtk : State
    {
        int actionCnt;
        protected override void OnEnter(State prevState)
        {
            Debug.Log("風圧攻撃");
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
                    //攻撃判定をOnにする
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
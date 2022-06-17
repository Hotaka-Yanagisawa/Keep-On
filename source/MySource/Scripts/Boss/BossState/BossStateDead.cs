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
        // 死亡状態
        private class BossStateDead : State
        {
            float DeadCnt;
            protected override void OnEnter(State prevState)
            {
                Debug.Log("ボス倒した");
                DeadCnt = 7;
                Owner.rb.velocity = Vector3.zero;
                Owner.animator.SetFloat("Speed", 0);
                Owner.animator.SetBool("Exist", true);
            }

            protected override void OnFixedUpdate()
            {
                Owner.rb.velocity = Vector3.zero;
                DeadCnt -= Time.deltaTime;
                if (DeadCnt <= 0)
                {
                    Owner.transform.parent.gameObject.SetActive(false);
                    //Destroy(Owner.gameObject);
                }
            }
        }
    }
}
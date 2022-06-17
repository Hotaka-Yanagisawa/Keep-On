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
        // 移動状態
        private class BossStateMoving : State
        {
            int transitionCnt;
            Vector3 velocity;
            protected override void OnEnter(State prevState)
            {
                Debug.Log("移動");
                transitionCnt = 60;
                velocity = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
                Owner.animator.SetBool("Attack", false);
                Owner.animator.SetFloat("speed", velocity.magnitude);

            }

            protected override void OnFixedUpdate()
            {
                // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
                velocity = velocity.normalized * Owner.moveSpeed;

                // いずれかの方向に移動している場合
                if (velocity.magnitude > 0)
                {
                    // 敵の角度の更新
                    // Slerp:現在の向き、向きたい方向、向かせるスピード
                    // LookRotation(向きたい方向):
                    Owner.transform.rotation =
                        Quaternion.Slerp(Owner.transform.rotation,
                        Quaternion.LookRotation(velocity),
                        Owner.applySpeed);

                    Owner.rb.velocity = velocity;
                }

                //移動制限
                Vector3 moveDistance = Owner.transform.position - Owner.firstPos;
                if (Owner.moveRange < moveDistance.magnitude)
                {
                    StateMachine.Dispatch((int)Event.Timeout);
                    return;
                }

                //次の遷移用の処理
                transitionCnt--;
                if (transitionCnt <= 0)
                {
                    StateMachine.Dispatch((int)Event.Timeout);
                }
            }
        }
    }
}
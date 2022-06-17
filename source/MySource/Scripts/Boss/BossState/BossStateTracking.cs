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
        // 追跡状態
        private class BossStateTracking : State
        {
            Vector3 velocity;
            int actionCnt;
            protected override void OnEnter(State prevState)
            {
                Debug.Log("追跡");
                Owner.rb.velocity = Vector3.zero;
                actionCnt = 120;
                //Owner.animator.SetBool("Attack", true);
            }

            protected override void OnFixedUpdate()
            {
                float Angle = Mathf.Atan2(Owner.player.transform.position.z - Owner.transform.position.z,
                     Owner.player.transform.position.x - Owner.transform.position.x);

                velocity.x = Mathf.Cos(Angle);
                velocity.z = Mathf.Sin(Angle);
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

                    Owner.animator.SetFloat("speed", velocity.magnitude);

                    Owner.rb.velocity = velocity;
                }

                actionCnt--;
                if (actionCnt < 1)
                {
                    StateMachine.Dispatch((int)Event.Lunge);
                }
            }
        }
    }
}
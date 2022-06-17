/////////////////////////////////////////////////////////////////////////
// 作成日 2021/04/02
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/04/02 作成開始
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
        // 突進追跡状態
        private class BossStateLungeTracking : State
        {
            Vector3 velocity;
            float Angle;
            int actionCnt;
            protected override void OnEnter(State prevState)
            {
                Debug.Log("突進攻撃");
                Owner.rb.velocity = Vector3.zero;
                //Owner.animator.SetFloat("speed", 0);
                actionCnt = 30;
                //進むベクトルを設定
                Angle = Mathf.Atan2(Owner.player.transform.position.z - Owner.transform.position.z,
          Owner.player.transform.position.x - Owner.transform.position.x);
                velocity.x = Mathf.Cos(Angle);
                velocity.z = Mathf.Sin(Angle);
                //// 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
                velocity = velocity.normalized * Owner.moveSpeed * 3;
                //Owner.animator.SetBool("Attack", true);
            }

            protected override void OnFixedUpdate()
            {
                // 敵の角度の更新
                // Slerp:現在の向き、向きたい方向、向かせるスピード
                // LookRotation(向きたい方向):
                Owner.transform.rotation =
                    Quaternion.Slerp(Owner.transform.rotation,
                    Quaternion.LookRotation(velocity),
                    1);

                Owner.rb.velocity = velocity;

                actionCnt--;
                if (actionCnt < 1)
                {
                    StateMachine.Dispatch((int)Event.Timeout);
                }
            }
        }
    }
}
/////////////////////////////////////////////////////////////////////////
// 作成日 2021/05/10
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/05/10 作成開始
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
        // 攻撃状態
        private class ReachAttacking : State
        {
            Vector3 velocity;
            protected override void OnEnter(State prevState)
            {
                Owner.rb.velocity = Vector3.zero;
                Owner.animator.SetFloat("speed", 0);
                //Owner.animator.SetBool("Attack", true);
            }

            protected override void OnFixedUpdate()
            {




                #region うんこ
                //float Angle = Mathf.Atan2(Owner.player.transform.position.z - Owner.transform.position.z,
                //     Owner.player.transform.position.x - Owner.transform.position.x);

                //velocity.x = Mathf.Cos(Angle);
                //velocity.z = Mathf.Sin(Angle);
                ////// 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
                ////velocity = velocity.normalized * Owner.moveSpeed;


                //// 敵の角度の更新
                //// Slerp:現在の向き、向きたい方向、向かせるスピード
                //// LookRotation(向きたい方向):
                //Owner.transform.rotation =
                //    Quaternion.Slerp(Owner.transform.rotation,
                //    Quaternion.LookRotation(velocity),
                //    Owner.applySpeed);
                ////if (Input.GetMouseButtonDown(0))
                ////{
                ////    Owner.animator.SetBool("Attack", true);
                ////}
                #endregion

            }
        }
    }
}
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
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using State = StateMachine<PBO>.State;
public partial class PBO
{
    // 待機状態
    private class PBOStateAttacking : State
    {
        Vector3 velocity;
        protected override void OnEnter(State prevState)
        {
            Debug.Log("攻撃!!!!!!!!!!!!!!!!!!!!!");
             
            Owner.rb.velocity = Vector3.zero;
            Owner.animator.SetFloat("speed", 0);
            Owner.animator.SetBool("Attack", true);
            Owner.behavior.OnEnter();
        }

        protected override void OnFixedUpdate()
        {
            float Angle = Mathf.Atan2(Owner.player.transform.position.z - Owner.transform.position.z,
                 Owner.player.transform.position.x - Owner.transform.position.x);

            velocity.x = Mathf.Cos(Angle);
            velocity.z = Mathf.Sin(Angle);
            //// 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
            //velocity = velocity.normalized * Owner.moveSpeed;


            // 敵の角度の更新
            // Slerp:現在の向き、向きたい方向、向かせるスピード
            // LookRotation(向きたい方向):
            Owner.transform.rotation =
                Quaternion.Slerp(Owner.transform.rotation,
                Quaternion.LookRotation(velocity),
                Owner.applySpeed);

            Owner.behavior.OnUpdate();
        }
    }
}
/////////////////////////////////////////////////////////////////////////
// 作成日 2021/04/02
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/04/02 作成開始
//            突進攻撃新たに作成（攻撃力は無い）
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
    private class PBOStateLungeAtk : State
    {
        Vector3 velocity;
        float Angle;
        int actionCnt;
        protected override void OnEnter(State prevState)
        {
            Debug.Log("突進攻撃");
            Owner.rb.velocity = Vector3.zero;
            Owner.animator.SetFloat("speed", 0);
            actionCnt = 90;
            //進むベクトルを設定
            Angle = Mathf.Atan2(Owner.player.transform.position.z - Owner.transform.position.z,
                                Owner.player.transform.position.x - Owner.transform.position.x);
            velocity.x = Mathf.Cos(Angle);
            velocity.z = Mathf.Sin(Angle);
            //// 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
            velocity = velocity.normalized * Owner.moveSpeed * 8;
        }

        protected override void OnFixedUpdate()
        {
            actionCnt--;

            if (actionCnt <= 45)
            {
                if (actionCnt == 45)
                {
                    //攻撃判定をOnにする
                    Owner.boxCollider.enabled = true;
                }
                // 敵の角度の更新
                // Slerp:現在の向き、向きたい方向、向かせるスピード
                // LookRotation(向きたい方向):
                Owner.transform.rotation =
                Quaternion.Slerp(Owner.transform.rotation,
                Quaternion.LookRotation(velocity),
                1);

                Owner.rb.velocity = velocity;
            }

            if (actionCnt < 1)
            {
                StateMachine.Dispatch((int)Event.Timeout);
            }
        }

        protected override void OnExit(State nextState)
        {
            //進むベクトルを設定
            Angle = Mathf.Atan2(Owner.player.transform.position.z - Owner.transform.position.z,
                                Owner.player.transform.position.x - Owner.transform.position.x);
            velocity.x = Mathf.Cos(Angle);
            velocity.z = Mathf.Sin(Angle);

            // 敵の角度の更新
            // Slerp:現在の向き、向きたい方向、向かせるスピード
            // LookRotation(向きたい方向):
            Owner.transform.rotation =
            Quaternion.Slerp(Owner.transform.rotation,
            Quaternion.LookRotation(velocity),
            1);
            Owner.boxCollider.enabled = false;
        }
    }
}
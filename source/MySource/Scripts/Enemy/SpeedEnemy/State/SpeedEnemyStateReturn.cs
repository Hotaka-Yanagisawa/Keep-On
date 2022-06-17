//////////////////////////////
// SpeedEnemyStateReturn.cs
//----------------------------
// 作成日:2021/4/25 
// 作成者:久田律貴
//----------------------------
// 更新日時・内容
//  ・スクリプト作成
//
//
//////////////////////////////
using UnityEngine;
using Homare;

namespace Hisada
{
    public partial class SpeedEnemy
    {
        /// <summary>
        /// 帰還状態
        /// </summary>
        class StateReturn : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:元の状態へ");

                //Debug.Log(owner.creater.EnemyNum);
            }

            public override void OnFixedUpdate(Enemy owner)
            {
                float Angle = Mathf.Atan2(owner.firstPos.z - owner.transform.position.z,
                                owner.firstPos.x - owner.transform.position.x);

                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));
                //owner.velocity.x = Mathf.Cos(Angle);
                //owner.velocity.z = Mathf.Sin(Angle);
                // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;

                // いずれかの方向に移動している場合
                if (owner.rb.velocity.magnitude > 0)
                {
                    // 敵の角度の更新
                    // Slerp:現在の向き、向きたい方向、向かせるスピード
                    // LookRotation(向きたい方向):
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.enemyStatus.applySpeed);

                    //owner.rb.velocity = owner.velocity;
                }

                //移動制限
                Vector3 moveDistance = owner.transform.position - owner.firstPos;
                if (owner.enemyStatus.moveRange * 0.5f > moveDistance.magnitude)
                {
                    owner.ChangeState(stateWaiting);
                }
            }
        }
    }
}

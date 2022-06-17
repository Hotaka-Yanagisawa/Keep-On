//////////////////////////////
// SpeedEnemyStateTracking.cs
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
        public class StateTracking : EnemyStateBase
        {
            // 攻撃適正距離
            private const float attackDistance = 6f;

            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:追跡");
            }

            public override void OnFixedUpdate(Enemy owner)
            {
                float Angle = Mathf.Atan2(player.transform.position.z - owner.transform.position.z,
                                player.transform.position.x - owner.transform.position.x);

                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));

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
                }

                // プレイヤーとの距離
                float playerDistance = Vector3.Distance(player.transform.position, owner.transform.position);
                
                // プレイヤーへの攻撃適正距離になったとき
                if(attackDistance - owner.enemyStatus.moveSpeed * 0.5 < playerDistance &&
                   playerDistance < attackDistance + owner.enemyStatus.moveSpeed * 0.5)
                {
                    owner.ChangeState(stateAIM);
                }

            }
        }
    }
}

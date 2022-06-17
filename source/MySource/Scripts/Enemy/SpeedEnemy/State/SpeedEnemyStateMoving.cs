//////////////////////////////
// SpeedEnemyStateMoving.cs
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
        /// 移動状態
        /// </summary>
        class StateMoving : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:移動");

                owner.actionCnt = 60;
                owner.rb.velocity = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
            }

            public override void OnFixedUpdate(Enemy owner)
            {
                // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;

                // いずれかの方向に移動している場合
                if (owner.rb.velocity.magnitude > 0)
                {
                    // 敵の角度の更新
                    // Slerp:現在の向き、向きたい方向、向かせるスピード
                    // LookRotation(向きたい方向)
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.enemyStatus.applySpeed);

                    owner.rb.velocity = owner.rb.velocity;                }

                //移動制限
                Vector3 moveDistance = owner.transform.position - owner.firstPos;
                if (owner.enemyStatus.moveRange < moveDistance.magnitude)
                {
                    //owner.ChangeState(stateReturn);
                    //return;
                }

                //次の遷移用の処理
                owner.actionCnt--;
                if (owner.actionCnt <= 0)
                {
                    owner.ChangeState(stateWaiting);
                }
            }

            public override void OnExit(Enemy owner, EnemyStateBase nextState)
            {
                owner.animator.SetBool("isWait", true);
            }
        }
    }
}

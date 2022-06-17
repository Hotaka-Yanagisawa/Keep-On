#region ヘッダコメント
// ReachEnemyStateReturn.cs
// 範囲型雑魚敵の帰還状態クラスS
//
// 2021/04/25 : 三上優斗
#endregion


using UnityEngine;
using Homare;


namespace Mikami
{
    public partial class ReachEnemy
    {
        private class StateReturn : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                owner.rb.velocity = Vector3.zero;
            }

            public override void OnUpdate(Enemy owner)
            {
                // リスポーン地点への移動
                float Angle = Mathf.Atan2(owner.firstPos.z - owner.transform.position.z,
                                owner.firstPos.x - owner.transform.position.x);
                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;

                // 移動方向へ振り向かせる
                if (owner.rb.velocity.magnitude > 0)
                {
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.enemyStatus.applySpeed);
                }

                // リスポーン地点まで戻ってきたら待機    
                Vector3 moveDistance = owner.transform.position - owner.firstPos;
                if (owner.enemyStatus.moveRange * 0.5f > moveDistance.magnitude)
                {
                    owner.ChangeState(stateIdle);
                }
            }
        }
    }
}
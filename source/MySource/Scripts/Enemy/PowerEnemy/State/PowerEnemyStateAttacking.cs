/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/18
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/03/18 作成開始
//            攻撃状態の処理を行う
// 
// 2021/04/22 再構築by三上
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;


namespace Homare
{
    public partial class PowerEnemy
    {
        /// <summary>
        /// 攻撃状態
        /// </summary>
        class StateAttacking : EnemyStateBase
        {
            // Vector3 playerDistance;
            //float actionCnt;
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("攻撃");
                owner.rb.velocity = Vector3.zero;

                owner.animator.SetFloat("Speed", 0);
                owner.animator.SetBool("Attack", true);
                //owner.animator.SetBool("AttackEnd", false);
                //actionCnt = owner.attackTime;
            }

            public override void OnFixedUpdate(Enemy owner)
            {
                Vector3 playerPos = player.transform.position;

                playerPos.y = owner.transform.position.y;

                // 敵の角度の更新
                // Slerp:現在の向き、向きたい方向、向かせるスピード
                // LookRotation(向きたい方向):
                owner.transform.rotation =
                    Quaternion.Slerp(owner.transform.rotation,
                    Quaternion.LookRotation(playerPos - owner.transform.position),
                    owner.enemyStatus.applySpeed);

                PowerEnemy powerEnemy = owner as PowerEnemy;
                powerEnemy.attackBehavior.OnUpdate();
            }

            //public override void OnExit(Enemy owner, EnemyStateBase nextState)
            //{
            //    owner.animator.SetBool("Attack", false);
            //    owner.animator.SetBool("AttackEnd", true);
            //}
        }
    }
}
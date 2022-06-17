#region ヘッダコメント
// ReachEnemyStateAim.cs
// 範囲型雑魚敵の照準状態クラス
//
// 2021/04/26 : 三上優斗
#endregion


using UnityEngine;
using Homare;


namespace Mikami
{
    public partial class ReachEnemy
    {
        private class StateAim : EnemyStateBase
        {
            private const int AIM_TIME_COUNT = 600;

            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                owner.rb.velocity = Vector3.zero;
                owner.actionCnt = AIM_TIME_COUNT;
                ReachEnemy reachEnemy = owner as ReachEnemy;
                //reachEnemy.aimEffectL?.SetBool("Fire", true);
                reachEnemy.aimEffectL.enabled = true;
                reachEnemy.aimEffectL?.SetFloat("Alpha", 1f);
                //reachEnemy.aimEffectR?.SetBool("Fire", true);
                reachEnemy.aimEffectR.enabled = true;
                reachEnemy.aimEffectR?.SetFloat("Alpha", 1f);
            }

            public override void OnUpdate(Enemy owner)
            {
                // ここに照準線をプレイヤーに合わせ続ける処理
                Vector3 playerPos = player.transform.position;
                playerPos.y = owner.transform.position.y;
                //owner.transform.LookAt(playerPos);

                owner.transform.rotation = 
                    Quaternion.Slerp(owner.transform.rotation,
                    Quaternion.LookRotation(Vector3.Normalize(playerPos - owner.transform.position)),
                    owner.enemyStatus.applySpeed);

                if (--owner.actionCnt <= 0)
                    owner.ChangeState(stateBattleMove);


                RaycastHit hit;
                Physics.Raycast(owner.transform.position, owner.transform.forward, out hit);

                // 照準がプレイヤーに合わさったら射撃
                if (owner.actionCnt < 400 && hit.transform.tag == "Player")
                    owner.ChangeState(stateShot);
            }

            public override void OnExit(Enemy owner, EnemyStateBase nextState)
            {
                ReachEnemy reachEnemy = owner as ReachEnemy;
                //reachEnemy.aimEffectL?.SetBool("Fire", false);
                reachEnemy.aimEffectL.enabled = false;
                reachEnemy.aimEffectL?.SetFloat("Alpha", 0f);
                //reachEnemy.aimEffectR?.SetBool("Fire", false);
                reachEnemy.aimEffectL.enabled = false;
                reachEnemy.aimEffectR?.SetFloat("Alpha", 0f);
            }
        }
    }
}
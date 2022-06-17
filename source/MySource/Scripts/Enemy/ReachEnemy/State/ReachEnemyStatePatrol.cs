#region ÉwÉbÉ_ÉRÉÅÉìÉg
// RangeEnemyStatePatrol.cs
// îÕàÕå^éGãõìGÇÃèÑâÒèÛë‘ÉNÉâÉX
//
// 2021/04/25 : éOè„óDìl
#endregion


using UnityEngine;
using Homare;


namespace Mikami
{
    public partial class ReachEnemy
    {
        private class StatePatrol : EnemyStateBase
        {
            private const int PATROL_TIME_COUNT = 120;

            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                owner.actionCnt = PATROL_TIME_COUNT;

                // à⁄ìÆï˚å¸ÇÉâÉìÉ_ÉÄÇ…ê›íË
                owner.rb.velocity =Vector3.Normalize(new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f)));
                //owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);
            }

            public override void OnUpdate(Enemy owner)
            {
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;

                // êiçsï˚å¸Ç…ëÃÇêUÇËå¸Ç©ÇπÇÈ
                if (owner.rb.velocity.magnitude > 0)
                {
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.enemyStatus.applySpeed);
                }

                // à⁄ìÆêßå¿
                Vector3 moveDistance = owner.transform.position - owner.firstPos;
                if (owner.enemyStatus.moveRange < moveDistance.magnitude)
                {
                    //owner.ChangeState(stateReturn);
                    //return;
                }

                if (owner.actionCnt-- <= 0)
                {
                    int random = Random.Range(0, 101);

                    // 40%Ç≈ë“ã@ÅA60%Ç≈èÑâÒÇåpë±
                    if (random < 40)
                        owner.ChangeState(stateIdle);
                    else
                        OnEnter(owner, statePatrol);
                }
            }
        }
    }
}
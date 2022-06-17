#region �w�b�_�R�����g
// RangeEnemyStateBattleMove.cs
// �͈͌^�G���G�̐퓬���ړ���ԃN���X
//
// 2021/04/26 : �O��D�l
#endregion


using UnityEngine;
using Homare;


namespace Mikami
{
    public partial class ReachEnemy
    {
        private class StateBattleMove : EnemyStateBase
        {
            private const float attackDistance = 12f;
            private const int MOVE_TIME_COUNT = 60;

            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                owner.actionCnt = MOVE_TIME_COUNT;
                
                // �v���C���[�̕��֌���
                Vector3 playerPos = player.transform.position;
                playerPos.y = owner.transform.position.y;
                owner.transform.LookAt(playerPos);

                //owner.transform.rotation =
                //    Quaternion.Slerp(owner.transform.rotation,
                //    Quaternion.LookRotation(Vector3.Normalize(playerPos - owner.transform.position)),
                //    owner.enemyStatus.applySpeed);

                float playerDistance = Vector3.Distance(player.transform.position, owner.transform.position);
                RaycastHit hit;
                Physics.Raycast(owner.transform.position, owner.transform.forward, out hit);

                // �������̑I��
                if (hit.transform.tag == "Player" || playerDistance > attackDistance + 2f || playerDistance < attackDistance - 2f)
                {
                    if (attackDistance < playerDistance)
                        owner.rb.velocity = owner.transform.forward;//new Vector3(Mathf.Cos(angleToPlayer), owner.rb.velocity.y, Mathf.Sin(angleToPlayer));
                    else
                        owner.rb.velocity = -owner.transform.forward;//-new Vector3(Mathf.Cos(angleToPlayer), owner.rb.velocity.y, Mathf.Sin(angleToPlayer));
                }
                else
                {
                    if (Random.Range(0, 101) < 70)
                    {
                        owner.rb.velocity = owner.transform.right;
                        //owner.actionCnt *= 3;
                    }
                    else
                    {
                        owner.rb.velocity = -owner.transform.right;
                        //owner.actionCnt *= 2;
                    }
                }


                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;
                
            }

            public override void OnUpdate(Enemy owner)
            {
                ReachEnemy reachEnemy = owner as ReachEnemy;
                --reachEnemy.shotCoolCnt;
                if (reachEnemy.shotCoolCnt > 0)
                    return;
                else if (reachEnemy.shotCoolCnt == 0)
                    OnEnter(owner, stateBattleMove);

                float playerDistance = Vector3.Distance(player.transform.position, owner.transform.position);
                RaycastHit hit;
                Physics.Raycast(owner.transform.position, owner.transform.forward, out hit);
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;

                // �v���C���[�̕����ɐU���������
                Vector3 playerPos = player.transform.position;
                playerPos.y = owner.transform.position.y;
                //owner.transform.LookAt(playerPos);
                
                owner.transform.rotation =
                    Quaternion.Slerp(owner.transform.rotation,
                    Quaternion.LookRotation(Vector3.Normalize(playerPos - owner.transform.position)),
                    owner.enemyStatus.applySpeed);

                // �U���K���������ː����󂯂Ă���ΏƏ��J�n
                if (//hit.transform.tag == "Player" &&
                    reachEnemy.shotCoolCnt <= 0 &&
                    attackDistance - owner.enemyStatus.moveSpeed * 0.2f < playerDistance &&
                    playerDistance < attackDistance + owner.enemyStatus.moveSpeed * 0.2f)
                {
                    owner.ChangeState(stateAim);
                }

                if (--owner.actionCnt < 0)
                    OnEnter(owner, null);
            }

        }
    }
}
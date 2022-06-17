#region �w�b�_�R�����g
// RangeEnemyStatePatrol.cs
// �͈͌^�G���G�̏����ԃN���X
//
// 2021/04/25 : �O��D�l
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

                // �ړ������������_���ɐݒ�
                owner.rb.velocity =Vector3.Normalize(new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f)));
                //owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);
            }

            public override void OnUpdate(Enemy owner)
            {
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;

                // �i�s�����ɑ̂�U���������
                if (owner.rb.velocity.magnitude > 0)
                {
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.enemyStatus.applySpeed);
                }

                // �ړ�����
                Vector3 moveDistance = owner.transform.position - owner.firstPos;
                if (owner.enemyStatus.moveRange < moveDistance.magnitude)
                {
                    //owner.ChangeState(stateReturn);
                    //return;
                }

                if (owner.actionCnt-- <= 0)
                {
                    int random = Random.Range(0, 101);

                    // 40%�őҋ@�A60%�ŏ�����p��
                    if (random < 40)
                        owner.ChangeState(stateIdle);
                    else
                        OnEnter(owner, statePatrol);
                }
            }
        }
    }
}
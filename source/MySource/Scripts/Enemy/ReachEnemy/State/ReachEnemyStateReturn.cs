#region �w�b�_�R�����g
// ReachEnemyStateReturn.cs
// �͈͌^�G���G�̋A�ҏ�ԃN���XS
//
// 2021/04/25 : �O��D�l
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
                // ���X�|�[���n�_�ւ̈ړ�
                float Angle = Mathf.Atan2(owner.firstPos.z - owner.transform.position.z,
                                owner.firstPos.x - owner.transform.position.x);
                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;

                // �ړ������֐U���������
                if (owner.rb.velocity.magnitude > 0)
                {
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.enemyStatus.applySpeed);
                }

                // ���X�|�[���n�_�܂Ŗ߂��Ă�����ҋ@    
                Vector3 moveDistance = owner.transform.position - owner.firstPos;
                if (owner.enemyStatus.moveRange * 0.5f > moveDistance.magnitude)
                {
                    owner.ChangeState(stateIdle);
                }
            }
        }
    }
}
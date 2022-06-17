/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/18
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/03/18 �쐬�J�n
//            �ړ�(�p�j)��Ԃ̏������s��
//      03/26 �p�j�͈͂ɐ�����݂���
//      03/27 �p�j�͈͂��o����Ԃňړ���ԂɂȂ����ꍇ�A�ҏ�ԂɑJ�ڂ���悤�ɂ���
//
// 2021/04/22 �č\�zby�O��
//
//////////////////////////////////////////////////////////////////////////
using UnityEngine;


namespace Homare
{
    public partial class PowerEnemy
    {
        /// <summary>
        /// �ړ����
        /// </summary>
        class StateMoving : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("�ړ�");

                owner.actionCnt = 1;
                owner.rb.velocity = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);
            }

            public override void OnFixedUpdate(Enemy owner)
            {
                // ���x�x�N�g���̒�����1�b��moveSpeed�����i�ނ悤�ɒ������܂�
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;

                // �����ꂩ�̕����Ɉړ����Ă���ꍇ
                if (owner.rb.velocity.magnitude > 0)
                {
                    // �G�̊p�x�̍X�V
                    // Slerp:���݂̌����A�������������A��������X�s�[�h
                    // LookRotation(������������):
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.enemyStatus.applySpeed);

                    owner.rb.velocity = owner.rb.velocity;
                }

                //�ړ�����
                Vector3 moveDistance = owner.transform.position - owner.firstPos;
                if (owner.enemyStatus.moveRange < moveDistance.magnitude)
                {
                    //owner.ChangeState(stateReturn);
                    //return;
                }

                //���̑J�ڗp�̏���
                owner.actionCnt-=Time.deltaTime;
                if (owner.actionCnt <= 0)
                {
                    owner.ChangeState(stateWaiting);
                }
            }

            public override void OnExit(Enemy owner, EnemyStateBase nextState)
            {
                owner.rb.velocity = Vector3.zero;
                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);
            }
        }
    }
}
//////////////////////////////
// SpeedEnemyStateMoving.cs
//----------------------------
// �쐬��:2021/4/25 
// �쐬��:�v�c���M
//----------------------------
// �X�V�����E���e
//  �E�X�N���v�g�쐬
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
        /// �ړ����
        /// </summary>
        class StateMoving : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:�ړ�");

                owner.actionCnt = 60;
                owner.rb.velocity = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
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
                    // LookRotation(������������)
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.enemyStatus.applySpeed);

                    owner.rb.velocity = owner.rb.velocity;                }

                //�ړ�����
                Vector3 moveDistance = owner.transform.position - owner.firstPos;
                if (owner.enemyStatus.moveRange < moveDistance.magnitude)
                {
                    //owner.ChangeState(stateReturn);
                    //return;
                }

                //���̑J�ڗp�̏���
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

/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/28
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/03/028 �쐬�J�n
//            
// 
//
//
//////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homare
{
using State = StateMachine<Boss>.State;
    public partial class Boss
    {
        // �ړ����
        private class BossStateMoving : State
        {
            int transitionCnt;
            Vector3 velocity;
            protected override void OnEnter(State prevState)
            {
                Debug.Log("�ړ�");
                transitionCnt = 60;
                velocity = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
                Owner.animator.SetBool("Attack", false);
                Owner.animator.SetFloat("speed", velocity.magnitude);

            }

            protected override void OnFixedUpdate()
            {
                // ���x�x�N�g���̒�����1�b��moveSpeed�����i�ނ悤�ɒ������܂�
                velocity = velocity.normalized * Owner.moveSpeed;

                // �����ꂩ�̕����Ɉړ����Ă���ꍇ
                if (velocity.magnitude > 0)
                {
                    // �G�̊p�x�̍X�V
                    // Slerp:���݂̌����A�������������A��������X�s�[�h
                    // LookRotation(������������):
                    Owner.transform.rotation =
                        Quaternion.Slerp(Owner.transform.rotation,
                        Quaternion.LookRotation(velocity),
                        Owner.applySpeed);

                    Owner.rb.velocity = velocity;
                }

                //�ړ�����
                Vector3 moveDistance = Owner.transform.position - Owner.firstPos;
                if (Owner.moveRange < moveDistance.magnitude)
                {
                    StateMachine.Dispatch((int)Event.Timeout);
                    return;
                }

                //���̑J�ڗp�̏���
                transitionCnt--;
                if (transitionCnt <= 0)
                {
                    StateMachine.Dispatch((int)Event.Timeout);
                }
            }
        }
    }
}
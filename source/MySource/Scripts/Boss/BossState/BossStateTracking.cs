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
        // �ǐՏ��
        private class BossStateTracking : State
        {
            Vector3 velocity;
            int actionCnt;
            protected override void OnEnter(State prevState)
            {
                Debug.Log("�ǐ�");
                Owner.rb.velocity = Vector3.zero;
                actionCnt = 120;
                //Owner.animator.SetBool("Attack", true);
            }

            protected override void OnFixedUpdate()
            {
                float Angle = Mathf.Atan2(Owner.player.transform.position.z - Owner.transform.position.z,
                     Owner.player.transform.position.x - Owner.transform.position.x);

                velocity.x = Mathf.Cos(Angle);
                velocity.z = Mathf.Sin(Angle);
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

                    Owner.animator.SetFloat("speed", velocity.magnitude);

                    Owner.rb.velocity = velocity;
                }

                actionCnt--;
                if (actionCnt < 1)
                {
                    StateMachine.Dispatch((int)Event.Lunge);
                }
            }
        }
    }
}
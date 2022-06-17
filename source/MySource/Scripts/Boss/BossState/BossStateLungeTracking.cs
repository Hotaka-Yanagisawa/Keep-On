/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/04/02
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/04/02 �쐬�J�n
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
        // �ːi�ǐՏ��
        private class BossStateLungeTracking : State
        {
            Vector3 velocity;
            float Angle;
            int actionCnt;
            protected override void OnEnter(State prevState)
            {
                Debug.Log("�ːi�U��");
                Owner.rb.velocity = Vector3.zero;
                //Owner.animator.SetFloat("speed", 0);
                actionCnt = 30;
                //�i�ރx�N�g����ݒ�
                Angle = Mathf.Atan2(Owner.player.transform.position.z - Owner.transform.position.z,
          Owner.player.transform.position.x - Owner.transform.position.x);
                velocity.x = Mathf.Cos(Angle);
                velocity.z = Mathf.Sin(Angle);
                //// ���x�x�N�g���̒�����1�b��moveSpeed�����i�ނ悤�ɒ������܂�
                velocity = velocity.normalized * Owner.moveSpeed * 3;
                //Owner.animator.SetBool("Attack", true);
            }

            protected override void OnFixedUpdate()
            {
                // �G�̊p�x�̍X�V
                // Slerp:���݂̌����A�������������A��������X�s�[�h
                // LookRotation(������������):
                Owner.transform.rotation =
                    Quaternion.Slerp(Owner.transform.rotation,
                    Quaternion.LookRotation(velocity),
                    1);

                Owner.rb.velocity = velocity;

                actionCnt--;
                if (actionCnt < 1)
                {
                    StateMachine.Dispatch((int)Event.Timeout);
                }
            }
        }
    }
}
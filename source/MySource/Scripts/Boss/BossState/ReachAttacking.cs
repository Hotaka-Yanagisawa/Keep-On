/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/05/10
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/05/10 �쐬�J�n
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
        // �U�����
        private class ReachAttacking : State
        {
            Vector3 velocity;
            protected override void OnEnter(State prevState)
            {
                Owner.rb.velocity = Vector3.zero;
                Owner.animator.SetFloat("speed", 0);
                //Owner.animator.SetBool("Attack", true);
            }

            protected override void OnFixedUpdate()
            {




                #region ����
                //float Angle = Mathf.Atan2(Owner.player.transform.position.z - Owner.transform.position.z,
                //     Owner.player.transform.position.x - Owner.transform.position.x);

                //velocity.x = Mathf.Cos(Angle);
                //velocity.z = Mathf.Sin(Angle);
                ////// ���x�x�N�g���̒�����1�b��moveSpeed�����i�ނ悤�ɒ������܂�
                ////velocity = velocity.normalized * Owner.moveSpeed;


                //// �G�̊p�x�̍X�V
                //// Slerp:���݂̌����A�������������A��������X�s�[�h
                //// LookRotation(������������):
                //Owner.transform.rotation =
                //    Quaternion.Slerp(Owner.transform.rotation,
                //    Quaternion.LookRotation(velocity),
                //    Owner.applySpeed);
                ////if (Input.GetMouseButtonDown(0))
                ////{
                ////    Owner.animator.SetBool("Attack", true);
                ////}
                #endregion

            }
        }
    }
}
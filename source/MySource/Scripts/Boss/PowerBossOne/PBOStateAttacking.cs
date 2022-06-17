/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/04/02
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/04/02 �쐬�J�n
//            BossScript�֌W�̔ėp�I�ȍs��AI���R�s�y
// 
//
//
//////////////////////////////////////////////////////////////////////////
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using State = StateMachine<PBO>.State;
public partial class PBO
{
    // �ҋ@���
    private class PBOStateAttacking : State
    {
        Vector3 velocity;
        protected override void OnEnter(State prevState)
        {
            Debug.Log("�U��!!!!!!!!!!!!!!!!!!!!!");
             
            Owner.rb.velocity = Vector3.zero;
            Owner.animator.SetFloat("speed", 0);
            Owner.animator.SetBool("Attack", true);
            Owner.behavior.OnEnter();
        }

        protected override void OnFixedUpdate()
        {
            float Angle = Mathf.Atan2(Owner.player.transform.position.z - Owner.transform.position.z,
                 Owner.player.transform.position.x - Owner.transform.position.x);

            velocity.x = Mathf.Cos(Angle);
            velocity.z = Mathf.Sin(Angle);
            //// ���x�x�N�g���̒�����1�b��moveSpeed�����i�ނ悤�ɒ������܂�
            //velocity = velocity.normalized * Owner.moveSpeed;


            // �G�̊p�x�̍X�V
            // Slerp:���݂̌����A�������������A��������X�s�[�h
            // LookRotation(������������):
            Owner.transform.rotation =
                Quaternion.Slerp(Owner.transform.rotation,
                Quaternion.LookRotation(velocity),
                Owner.applySpeed);

            Owner.behavior.OnUpdate();
        }
    }
}
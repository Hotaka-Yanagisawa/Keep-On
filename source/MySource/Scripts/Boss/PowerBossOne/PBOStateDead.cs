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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using State = StateMachine<PBO>.State;
public partial class PBO
{
    // �ҋ@���
    private class PBOStateDead : State
    {
        int DeadCnt;
        protected override void OnEnter(State prevState)
        {
            Debug.Log("�{�X�|����");
            
            DeadCnt = 60;
            Owner.rb.velocity = Vector3.zero;
            Owner.animator.SetFloat("speed", 0);
        }

        protected override void OnFixedUpdate()
        {
            DeadCnt--;
            if (DeadCnt <= 0)
            {
                Destroy(Owner.gameObject);
            }
        }
    }
}
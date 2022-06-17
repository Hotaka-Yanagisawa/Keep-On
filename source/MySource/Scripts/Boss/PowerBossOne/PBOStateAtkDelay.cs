/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/04/02
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/04/02 �쐬�J�n
//            �U����̌㌄
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
    private class PBOStateAtkDelay : State
    {
        int actionCnt;
        protected override void OnEnter(State prevState)
        {
            Debug.Log(prevState + "���痈��");
            //bool a = prevState is PBOStateAttacking;
            Owner.rb.velocity = Vector3.zero;
            Owner.animator.SetFloat("speed", 0);
            actionCnt = 60;
        }

        protected override void OnFixedUpdate()
        {
            actionCnt--;

            if (actionCnt < 1)
            {
                StateMachine.Dispatch((int)Event.Timeout);
            }
        }
        protected override void OnExit(State nextState)
        {
            
        }
    }
}
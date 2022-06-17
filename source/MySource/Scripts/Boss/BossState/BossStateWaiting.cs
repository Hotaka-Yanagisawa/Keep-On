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
        // �ҋ@���
        private class BossStateWaiting : State
        {
            float transitionCnt;
            protected override void OnEnter(State prevState)
            {
                Debug.Log("�ҋ@");
                transitionCnt = 1;
                Owner.rb.velocity = Vector3.zero;               
            }

            protected override void OnFixedUpdate()
            {
                Owner.rb.velocity = Vector3.zero;

                //if (!Owner.animator.GetBool("Change"))
                //{
                transitionCnt -= Time.deltaTime;
                //}
                if (!Owner.animator.GetBool("Change"))
                {
                    StateMachine.Dispatch((int)Event.Attack);
                    //StateMachine.Dispatch((int)Event.Timeout);
                }
            }
        }
    }
}
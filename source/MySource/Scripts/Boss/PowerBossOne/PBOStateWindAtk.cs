/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/04/02
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/04/02 �쐬�J�n
//            �����U���V���ɍ쐬�i�U���͖͂����j
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
    private class PBOStateWindAtk : State
    {
        int actionCnt;
        protected override void OnEnter(State prevState)
        {
            Debug.Log("�����U��");
            Owner.rb.velocity = Vector3.zero;
            Owner.animator.SetFloat("speed", 0);
            actionCnt = 60;

            Owner.rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }

        protected override void OnFixedUpdate()
        {
            actionCnt--;

            if (actionCnt <= 30)
            {
                if (actionCnt == 30)
                {
                    //�U�������On�ɂ���
                    Owner.sphereCollider.enabled = true;
                }
            }

            if (actionCnt < 1)
            {
                StateMachine.Dispatch((int)Event.Timeout);
            }
        }
        protected override void OnExit(State nextState)
        {
            Owner.sphereCollider.enabled = false;
        }
    }
}
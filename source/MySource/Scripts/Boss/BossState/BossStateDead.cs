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
        // ���S���
        private class BossStateDead : State
        {
            float DeadCnt;
            protected override void OnEnter(State prevState)
            {
                Debug.Log("�{�X�|����");
                DeadCnt = 7;
                Owner.rb.velocity = Vector3.zero;
                Owner.animator.SetFloat("Speed", 0);
                Owner.animator.SetBool("Exist", true);
            }

            protected override void OnFixedUpdate()
            {
                Owner.rb.velocity = Vector3.zero;
                DeadCnt -= Time.deltaTime;
                if (DeadCnt <= 0)
                {
                    Owner.transform.parent.gameObject.SetActive(false);
                    //Destroy(Owner.gameObject);
                }
            }
        }
    }
}
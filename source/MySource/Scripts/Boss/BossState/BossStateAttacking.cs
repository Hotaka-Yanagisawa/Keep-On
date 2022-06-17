/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/28
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/03/28 �쐬�J�n
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
        private class BossStateAttacking : State
        {
            Vector3 velocity;
            protected override void OnEnter(State prevState)
            {
                Owner.rb.velocity = Vector3.zero;
                Owner.animator.SetFloat("Speed", 0);
            }

            protected override void OnFixedUpdate()
            {
                
                switch (Owner.type)
                {
                    case BossType.Reach:
                        Owner.ReachCurState?.OnFixedUpdate(Owner);
                        //Owner.reachBehavior?.OnUpdate();
                        break;
                    case BossType.Speed:
                        Owner.SpeedCurState?.OnFixedUpdate(Owner);
                        //Owner.speedBehavior?.OnUpdate();
                        break;
                    case BossType.Power:
                        Owner.PowerCurState?.OnFixedUpdate(Owner);
                        //Owner.powerBehavior?.OnUpdate();
                        break;
                }
                if (Owner.isDown)
                {
                    StateMachine.Dispatch((int)Event.Down);
                    Owner.isAtk = false;
                }
            }
        }
    }
}
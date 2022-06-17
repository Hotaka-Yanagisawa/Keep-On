/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/28
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/03/028 作成開始
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
        // 待機状態
        private class BossStateDown : State
        {
            float transitionCnt;
            protected override void OnEnter(State prevState)
            {
                transitionCnt = Owner.downTime;
                Owner.rb.velocity = Vector3.zero;
                Owner.animator.SetBool("Down", true);
                Owner.animator.SetBool("Attack", false);
                Owner.bossWeapon.bladeCollider.enabled = false;
                Owner.bossWeapon.handleCollider.enabled = false;
                Owner.SetWind(false, 40);
                //Owner.SetArc(false);
                Owner.SetExtinction(false);
                Owner.bossAction = BossAction.Non;
                Owner.fallDownAtk.SetActive(false);
                Owner.fallDownAtk.GetComponent<FallDownAtk>().enabled = false;
                Owner.downCollider.enabled = true;
            }

            protected override void OnFixedUpdate()
            {
                transitionCnt -= Time.deltaTime;
                Owner.rb.velocity = Vector3.zero;

                if (transitionCnt <= 0)
                {
                    //Owner.isDown = false;
                    Owner.animator.SetBool("Down", false);
                    switch (Owner.type)
                    {
                        case BossType.Reach:
                            Owner.ChangeStateMachine(BossType.Speed);
                            Owner.styleHolder.style = Owner.reachStyle;
                            break;
                        case BossType.Speed:
                            Owner.ChangeStateMachine(BossType.Power);
                            Owner.styleHolder.style = Owner.speedStyle;

                            break;
                        case BossType.Power:
                            Owner.ChangeStateMachine(BossType.Reach);
                            Owner.styleHolder.style = Owner.powerStyle;

                            break;
                    }
                }
            }
            protected override void OnExit(State nextState)
            {
                
                
            }

            //if (!Owner.animator.GetBool("Change"))
            //{
            //    //StateMachine.Dispatch((int)Event.Attack);

            //}
        }
        void DeleteCol()
        {
            downCollider.enabled = false;
        }

        void StartDown()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        }

        void EndDown()
        {
            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        }
    }
}
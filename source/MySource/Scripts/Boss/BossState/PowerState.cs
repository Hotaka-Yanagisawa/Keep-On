//////////////////////////////////////////////////
// 制作者：柳沢帆貴
// 手製behaviorツリーが使いにくいのでステートに変更
//
//
//
//
//
//
///////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Homare
{
    public partial class Boss
    {
        //プレイヤーを追跡する
        public class MoveState : BossStateBase
        {
            float actionCnt;
            public override void OnEnter(Boss owner, BossStateBase prevState)
            {
                owner.animator.SetBool("Attack", false);
                //owner.animator.SetBool("Turn", false);
                owner.bossAction = BossAction.Non;
                owner.searchArea.enabled = true;
                actionCnt = 1;
            }


            public override void OnFixedUpdate(Boss owner)
            {
                actionCnt -= Time.deltaTime;
                float Angle = Mathf.Atan2(owner.player.transform.position.z - owner.transform.position.z,
                 owner.player.transform.position.x - owner.transform.position.x);

                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));
                //owner.velocity.x = Mathf.Cos(Angle);
                //owner.velocity.z = Mathf.Sin(Angle);
                // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
                owner.rb.velocity = owner.rb.velocity.normalized * owner.moveSpeed;

                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);

                // いずれかの方向に移動している場合
                if (owner.rb.velocity.magnitude > 0)
                {
                    // 敵の角度の更新
                    // Slerp:現在の向き、向きたい方向、向かせるスピード
                    // LookRotation(向きたい方向):
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.applySpeed);
                }
                if (owner.isAtk && actionCnt <= 0)
                {
                   owner.ChangePowerState(atkState);
                }
            }

        }


        public class AtkState : BossStateBase
        {
            public override void OnEnter(Boss owner, BossStateBase prevState)
            {
                owner.isAtk = false;
                //owner.animator.SetInteger("AtkType", 0);
                owner.animator.SetInteger("AtkType", Random.Range(0, 3));
                owner.animator.SetBool("Attack", true);
                owner.animator.SetFloat("Speed", 0);
                owner.rb.velocity = Vector3.zero;
                //owner.bossWeapon.bladeCollider.enabled = true;
                //owner.bossWeapon.handleCollider.enabled = true;
                owner.searchArea.enabled = false;
                owner.bossAction = BossAction.Atk;
                owner.atkCnt--;

            }


            public override void OnFixedUpdate(Boss owner)
            {
                owner.rb.velocity = Vector3.zero;

                if (owner.animator.GetInteger("AtkType") == 2)
                {
                    if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f)
                    {
                        AxisAlignment(owner, 0.5f);
                    }
                }
                else
                {
                    if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f)
                    {
                        AxisAlignment(owner, owner.applySpeed);
                    }
                }

                if (owner.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
                {
                    if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                    {
 

                        if (owner.atkCnt <= 0)
                        {
                            owner.animator.SetBool("Attack", false);
                            owner.ChangePowerState(moveState);
                            owner.bossWeapon.bladeCollider.enabled = false;
                            owner.bossWeapon.handleCollider.enabled = false;
                            owner.ChangeStateMachine(BossType.Reach);
                        }
                        else
                        {
                            owner.ChangePowerState(moveState);
                            owner.searchArea.enabled = true;

                        }
                        //owner.ChangePowerState(moveState);

                    }
                }
            }

            void AxisAlignment(Boss owner, float applySpeed)
            {

                float Angle = Mathf.Atan2(owner.player.transform.position.z - owner.transform.position.z,
                 owner.player.transform.position.x - owner.transform.position.x);

                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));
                //owner.velocity.x = Mathf.Cos(Angle);
                //owner.velocity.z = Mathf.Sin(Angle);
                // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
                owner.rb.velocity = owner.rb.velocity.normalized * owner.moveSpeed;
                // 敵の角度の更新
                // Slerp:現在の向き、向きたい方向、向かせるスピード
                // LookRotation(向きたい方向):
                owner.transform.rotation =
                Quaternion.Slerp(owner.transform.rotation,
                Quaternion.LookRotation(owner.rb.velocity),
                applySpeed);
                owner.rb.velocity = Vector3.zero;
            }
        }

        private BossStateBase PowerCurState;
        private static readonly AtkState atkState = new AtkState();
        private static readonly MoveState moveState = new MoveState();

        /// <summary>
        ///     ステートの変更
        /// </summary>
        /// <param name="nextState"></param>
        public void ChangePowerState(BossStateBase nextState)
        {
            if (nextState != PowerCurState)
            {
                PowerCurState.OnExit(this, nextState);
                nextState.OnEnter(this, PowerCurState);
                PowerCurState = nextState;
            }
        }
    }
}
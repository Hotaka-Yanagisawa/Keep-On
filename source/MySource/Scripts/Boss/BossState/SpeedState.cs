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
        //プレイヤーから離れるジャンプ
        public class JumpMoveState : BossStateBase
        {
            float actionCnt;
            public override void OnEnter(Boss owner, BossStateBase prevState)
            {
                owner.animator.SetBool("Attack", false);
                actionCnt = 1;
            }
            public override void OnFixedUpdate(Boss owner)
            {

                float Angle = Mathf.Atan2(owner.player.transform.position.z - owner.transform.position.z,
                 owner.player.transform.position.x - owner.transform.position.x);

                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));

                // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
                owner.rb.velocity = owner.rb.velocity.normalized * owner.moveSpeed;

                owner.animator.SetFloat("Speed", 0);


                // 敵の角度の更新
                // Slerp:現在の向き、向きたい方向、向かせるスピード
                // LookRotation(向きたい方向):
                owner.transform.rotation =
                    Quaternion.Slerp(owner.transform.rotation,
                    Quaternion.LookRotation(owner.rb.velocity),
                    1);


                owner.rb.velocity = Vector3.zero;
                actionCnt -= Time.deltaTime;
                if(actionCnt <= 0)         owner.ChangeSpeedState(jumpAtkState);

            }
        }

        public class JumpAtkState : BossStateBase
        {
            float actionCnt;
            
            public override void OnEnter(Boss owner, BossStateBase prevState)
            {
                owner.animator.SetInteger("AtkType", 0);
                owner.animator.SetBool("Attack", true);
                owner.animator.SetFloat("Speed", 0);

                owner.isAtk = false;
                actionCnt = 0;
                owner.bossAction = BossAction.Fly;
            }
            public override void OnFixedUpdate(Boss owner)
            {
                if (actionCnt < 0)
                {
                    actionCnt = 0;
                    //SetArc(true);
                    owner.animator.SetInteger("AtkType", 0);
                    owner.animator.SetBool("Attack", true);
                }
                actionCnt += Time.deltaTime;

                if (actionCnt > 5)
                {
                    if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                    {
                        actionCnt = -1;
                        owner.bossAction = BossAction.Non;

                        owner.animator.SetBool("Attack", false);

                        owner.atkCnt--;

                        if (owner.atkCnt <= 0)
                        {
                            owner.ChangeStateMachine(BossType.Power);
                        }
                        else
                        {
                            owner.ChangeSpeedState(jumpMoveState);
                        }
                    }
                }
            }
        }
        void SetArc(bool atkKind)
        {
            if (atkKind)
            {
                arc.enabled = true;
                arc.endPos = player.transform.position;
            }
            else
            {
                arc.enabled = false;
            }
        }

        void OnArc()
        {
            arc.enabled = true;
            arc.endPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            //bossAction = BossAction.Fly;
        }

        void OnFootCol()
        {
            fallDownAtk.SetActive(true);
            fallDownAtk.GetComponent<FallDownAtk>().enabled = true;
        }

        
        private BossStateBase SpeedCurState;
        private static readonly JumpAtkState jumpAtkState = new JumpAtkState();
        private static readonly JumpMoveState jumpMoveState = new JumpMoveState();

        /// <summary>
        ///     ステートの変更
        /// </summary>
        /// <param name="nextState"></param>
        public void ChangeSpeedState(BossStateBase nextState)
        {
            if (nextState != SpeedCurState)
            {
                SpeedCurState.OnExit(this, nextState);
                nextState.OnEnter(this, SpeedCurState);
                SpeedCurState = nextState;
            }
        }
    }
}
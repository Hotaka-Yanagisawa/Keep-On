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
        //フィールド中央に移動する
        public class CenterMoveState : BossStateBase
        {
            public override void OnEnter(Boss owner, BossStateBase prevState)
            {
                owner.animator.SetBool("Attack", false);  
                owner.bossAction = BossAction.Non;
                owner.searchArea.enabled = true;

            }
            public override void OnFixedUpdate(Boss owner)
            {
                //if (owner.animator.GetBool("Stamp")) return;
                //原点への角度
                float Angle = Mathf.Atan2(-owner.transform.position.z,-owner.transform.position.x);
                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));              
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
                //中央に近づいたら次のステートへ
                if (Vector3.Distance(owner.transform.position, Vector3.zero) < 1)
                {
                    owner.ChangeReachState(bombingState);
                }

            }
        }

        public class BombingState : BossStateBase
        {
            //float actionCnt;
            public override void OnEnter(Boss owner, BossStateBase prevState)
            {
                owner.rb.velocity = Vector3.zero;
                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);
                owner.animator.SetInteger("AtkType", 0);
                owner.animator.SetBool("Attack", true);
                owner.isAtk = false;
                //actionCnt = 0;
            }
            public override void OnFixedUpdate(Boss owner)
            {
                owner.rb.velocity = Vector3.zero;
                //関係あるモーションかどうか
                //if (owner.animator.GetCurrentAnimatorStateInfo(0).IsTag("extinction"))
                //{
                //まだ攻撃フラグが立ってなく攻撃モーション中なら
                //if (owner.animator.GetInteger("AtkType") == 1 && !owner.isAtk)
                //{
                //    //攻撃開始、攻撃フラグ立てる
                //    owner.SetWind(true, 25);
                //    owner.SetExtinction(true);
                //    owner.isAtk = true;
                //    owner.bossAction = BossAction.Atk;
                //    actionCnt = 0;
                //}
                //攻撃中ならカウントを進める
                //else if (owner.isAtk) actionCnt += Time.deltaTime;
                //カウントが進んだらendモーションへ移行
                //if (actionCnt > 5) owner.animator.SetInteger("AtkType", 2);
                //endモーションで攻撃フラグが立っていたら終了処理へ
                //if (owner.animator.GetInteger("AtkType") == 2 && owner.isAtk)
                //{
                //owner.bossAction = BossAction.Non;
                ////モーションを変更
                //owner.animator.SetBool("Attack", false);
                ////攻撃終了
                //owner.SetExtinction(false);
                //owner.SetWind(false, 40);
                ////例外値に
                //actionCnt = -1;
                ////仮チェンジ下に別のあり
                //owner.ChangeStateMachine(BossType.Speed);
                //}

                //}
            }
            
        }

        public class LaserState : BossStateBase
        {
            public override void OnEnter(Boss owner, BossStateBase prevState)
            {
                //owner.animator.SetInteger("AtkType", Random.Range(0, 3));
                owner.animator.SetBool("Attack", true);
                owner.isAtk = false;
                owner.rb.velocity = Vector3.zero;
                owner.bossAction = BossAction.Atk;
            }


            public override void OnFixedUpdate(Boss owner)
            {
                //攻撃モーション中プレイヤーにゆっくり軸合わせ
                if (owner.animator.GetInteger("AtkType") == 2)
                {
                    if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f)
                    {
                        TurnAround(owner);
                    }
                }


                //終了処理要調整の必要あり
                if (owner.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
                {
                    if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                    {
                        owner.animator.SetBool("Attack", false);
                        //owner.ChangeState(moveState);
                    }
                }
            }

            public override void OnExit(Boss owner, BossStateBase nextState)
            {
                //owner.atkCnt--;
                //if (owner.atkCnt <= 0)
                //{
                //    owner.ChangeStateMachine(BossType.Speed);
                //}
            }
            //振り向き
            void TurnAround(Boss owner)
            {

                float Angle = Mathf.Atan2(owner.player.transform.position.z - owner.transform.position.z,
                 owner.player.transform.position.x - owner.transform.position.x);

                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));
                owner.rb.velocity = owner.rb.velocity.normalized * owner.moveSpeed;
                // 敵の角度の更新
                // Slerp:現在の向き、向きたい方向、向かせるスピード
                // LookRotation(向きたい方向):
                owner.transform.rotation =
                Quaternion.Slerp(owner.transform.rotation,
                Quaternion.LookRotation(owner.rb.velocity),
                0.1f);
                owner.rb.velocity = Vector3.zero;
            }
        }

        private BossStateBase ReachCurState;
        private static readonly CenterMoveState centerMoveState = new CenterMoveState();
        private static readonly BombingState bombingState = new BombingState();
        private static readonly LaserState laserState = new LaserState();

        /// <summary>
        ///     ステートの変更
        /// </summary>
        /// <param name="nextState"></param>
        public void ChangeReachState(BossStateBase nextState)
        {
            if (nextState != ReachCurState)
            {
                ReachCurState.OnExit(this, nextState);
                nextState.OnEnter(this, ReachCurState);
                ReachCurState = nextState;
            }
        }
        public void SetWind(bool active, float Power)
        {
            if (!manage.isResult)
            {
                wind.SetActive(active);
                wind.GetComponent<Wind>().windPower = Power;
            }
            if (active)
            {
                vEffect.Play();
            }
            else
            {
                vEffect.Stop();
            }
        }
        //リザルト時のみ起動
        public void EndWind()
        {
            if (manage.isResult)
            {
                wind.SetActive(false);
            }
        }

        void SetExtinction(bool exist)
        {
            //extinction.enabled = exist;
            divisionExtinction.enabled = exist;
        }

        public void SetBomb(bool exist)
        {
            if (exist)
            {
                //SetWind(true, 15);
                SetExtinction(true);
                isAtk = true;
                bossAction = BossAction.Atk;
            }
            else
            {
                isAtk = false;
                bossAction = BossAction.Non;
                //モーションを変更
                animator.SetBool("Attack", false);
                //攻撃終了
                SetExtinction(false);
                //SetWind(false, 10);
                //例外値に
                //actionCnt = -1;
                //仮チェンジ下に別のあり
                ChangeStateMachine(BossType.Speed);
            }
        }


    }
}
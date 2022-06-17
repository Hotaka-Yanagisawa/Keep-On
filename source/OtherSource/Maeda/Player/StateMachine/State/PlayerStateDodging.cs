using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateDodging
//	プレイヤーのステップ状態
// 作成日時	:2021/03/23
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/23   ステップのスクリプトをひとつにまとめました   	
//==================================================================================
#endregion

namespace Maeda
{
    public partial class Player
    {

        float consumption;
        public class StateDodging : PlayerStateBase
        {
            Vector3 vec;
            
            
            #region オーバーライド関数

            /// <summary>
            /// このステートに入った時の処理  
            /// </summary>
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                owner.isStep = true;
                owner.frameCnt = 0;                             // 初期化
               
                
                Debug.Log(owner.baseParam.style.style);

                #region 素体
                if (owner.baseParam.style.style == Style.E_Style.NORMAL)
                {
                    owner.consumption = owner.baseParam.maxSp / owner.normal.maxStep; // スタミナ消費量の計算

                    owner.OnCommonEnterFnc();
                }
                #endregion

                #region 火力型
                if (owner.baseParam.style.style == Style.E_Style.POWER)
                {
                    owner.consumption = owner.baseParam.maxSp / owner.power.maxStep;

                    owner.OnCommonEnterFnc();
                }
                #endregion

                #region 機動型
                if (owner.baseParam.style.style == Style.E_Style.MOBILITY)
                {
                    owner.consumption = owner.baseParam.maxSp / owner.mobility.maxStep;
                    owner.OnCommonEnterFnc();
                }
                #endregion

                #region 範囲型
                if (owner.baseParam.style.style == Style.E_Style.REACH)
                {
                    owner.consumption = owner.baseParam.maxSp / owner.reach.maxStep;

                    owner.OnCommonEnterFnc();
                }
                #endregion
            }


            /// <summary>
            /// このステートの間処理する
            /// </summary>
            public override void OnUpdate(Player owner)
            {
                owner.frameCnt++;                                   // フレームカウントを増やす
                //落下スピードの調節
                owner.rigidBody.velocity = new Vector3(owner.rigidBody.velocity.x, owner.rigidBody.velocity.y * 0.8f, owner.rigidBody.velocity.z);

                // フレームカウントが設定したカウントより多くなったら処理を行う
                #region 素体
                if (owner.baseParam.style.style == Style.E_Style.NORMAL)
                {
                    if (owner.frameCnt >= owner.normal.stepRange * 0.7f) owner.rigidBody.useGravity = true;
                    // フレームカウントが設定したカウントより多くなったら処理を行う
                    if (owner.frameCnt >= owner.normal.stepRange)
                    {
                        owner.OnCommonUpdateFnc();
                    }
                }
                #endregion

                #region 火力型
                else if (owner.baseParam.style.style == Style.E_Style.POWER)
                {
                    if (owner.frameCnt >= owner.power.stepRange * 0.7f) owner.rigidBody.useGravity = true;
                    // フレームカウントが設定したカウントより多くなったら処理を行う
                    if (owner.frameCnt >= owner.power.stepRange)
                    {
                        owner.OnCommonUpdateFnc();
                    }
                }
                #endregion

                #region 機動型
                else if (owner.baseParam.style.style == Style.E_Style.MOBILITY)
                {
                    if (owner.frameCnt >= owner.mobility.stepRange * 0.7f) owner.rigidBody.useGravity = true;
                    // フレームカウントが設定したカウントより多くなったら処理を行う
                    if (owner.frameCnt >= owner.mobility.stepRange)
                    {
                        owner.OnCommonUpdateFnc();
                    }
                }
                #endregion

                #region 範囲型
                else if (owner.baseParam.style.style == Style.E_Style.REACH)
                {
                    //これ必要なので
                    if (owner.frameCnt >= owner.reach.stepRange * 0.7f) owner.rigidBody.useGravity = true;

                    if(owner.frameCnt >= owner.reach.stepRange)
                    {
                        owner.OnCommonUpdateFnc();
                    }
                   

                }
                #endregion
            }

            /// <summary>
            /// このステートから抜けるときの処理
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="nextState"></param>
            public override void OnExit(Player owner, PlayerStateBase nextState)
            {
                owner.isStep = false;
                owner.rigidBody.useGravity = true;
                owner.animator.SetBool("step", false);           
            }
            #endregion
        }


        #region プライベート関数
        private void OnCommonEnterFnc()
        {
            // スタミナのチェックとインターバルのチェック
            if (baseParam.currentSp >= consumption && stepIntervalTime <= 0 && okStep)
            {
                baseParam.currentSp -= consumption;                 // スタミナの消費 

                if(baseParam.currentSp< consumption)
                okStep = false;

                stepIntervalTime = baseParam.stepInterval;          // ステップのインターバルの設定

                animator.SetBool("step", true);

                OnStep();                                           // ステップの処理               
            }
            else
            {
                ChangeState(stateMoving);
            }
        }


        private void OnCommonUpdateFnc()
        {
            if (baseParam.currentSp < consumption)         // ここ確認
            {
                ChangeState(stateMoving);                   // 硬直状態へ
            }
            else if (animator.GetBool("jump"))
            {
                animator.SetInteger("jumpType", 1);
                ChangeState(stateFalldown);
            }
            else if (!animator.GetBool("jump"))
            {
                ChangeState(stateMoving);
            }
            
        }


        private void OnStep()
        {
            //rigidBody.useGravity = false;
            //rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
            rigidBody.velocity = Vector3.zero;
            rigidBody.AddForce(moveForward.normalized * 15f, ForceMode.Impulse);
            PlaySE("SE_Step");
        }
        #endregion
    }
}


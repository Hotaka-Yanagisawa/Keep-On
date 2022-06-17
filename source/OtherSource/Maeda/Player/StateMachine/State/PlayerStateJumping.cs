using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateJumping
//	プレイヤーのジャンプ状態
// 作成日時	:2021/03/20
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/20   ジャンプの実装	
//==================================================================================
#endregion

/// <summary>
/// ジャンプ中の処理
/// 空中のストレイフ(空中で移動)が多少できる状態
/// </summary>
namespace Maeda
{
    public partial class Player
    {
        public class StateJumping : PlayerStateBase
        {
            //float oldposY =0f;
            #region オーバーライド関数
            /// <summary>
            /// 状態遷移した最初に呼ばれる関数     Init()と同じ
            /// </summary>
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                if (!owner.animator.GetBool("jump"))
                {
                    #region 共通の処理                 
                    //owner.animator.SetBool("jump", true);
                    owner.animator.SetInteger("jumpType", 0);
                    owner.animator.Play("JumpStart");
                    #endregion
                }
                else
                { 
                    owner.animator.SetInteger("jumpType", 1);
                    owner.ChangeState(stateFalldown);
                }
            }


            /// <summary>
            /// この状態の時繰り返し呼ばれる関数    Update()と同じ
            /// </summary>
            public override void OnUpdate(Player owner)
            {
                owner.Moving();                               
            }
            #endregion
        }

        #region プライベート関数
        /// <summary>
        /// 移動関数　Moving(Player owner)
        /// 一定の入力値を超えたら走りの速度になる
        /// また超えなかったら、歩きの速度になる
        /// コントローラのスティックの入力をX軸とY軸の成分に分けている
        ///         上下→Y軸   上：+値    下：－値
        ///         左右→X軸   右：+値    左：－値       
        /// </summary>
        /// <param name="owner"></param>
        private void Moving()
        {
            rigidBody.velocity = moveForward * baseParam.moveDash + new Vector3(0, rigidBody.velocity.y, 0);        
        }

        private void Jumping()
        {
            animator.SetBool("jump", true);
            if (animator.GetBool("jump"))
            {
                #region 素体
                if (baseParam.style.style == Style.E_Style.NORMAL)
                {
                    //Debug.Log("素体のジャンプしました");
                    rigidBody.AddForce(Vector3.up * normal.jumpPower, ForceMode.Impulse);
                }
                #endregion

                #region 火力型
                if (baseParam.style.style == Style.E_Style.POWER)
                {
                    Debug.Log("火力型のジャンプしました");
                    rigidBody.AddForce(Vector3.up * power.jumpPower, ForceMode.Impulse);
                }
                #endregion

                #region 機動型
                if (baseParam.style.style == Style.E_Style.MOBILITY)
                {
                    Debug.Log("機動型のジャンプしました");
                    rigidBody.AddForce(Vector3.up * mobility.jumpPower, ForceMode.Impulse);
                }
                #endregion

                #region 範囲型
                if (baseParam.style.style == Style.E_Style.REACH)
                {
                    Debug.Log("範囲型のジャンプしました");
                    rigidBody.AddForce(Vector3.up * reach.jumpPower, ForceMode.Impulse);
                }
                #endregion

                ChangeState(stateFalldown);
            }
        }

        private void InJumpStart()
        {
            animator.SetBool("jump", true);
        }
        #endregion
    }
}

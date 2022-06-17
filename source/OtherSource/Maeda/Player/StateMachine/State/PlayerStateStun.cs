using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateStun
//	プレイヤーの右へ回避する状態
// 作成日時	:2021/03/23
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/23   プレイヤーの硬直の実装     以下の状態の遷移先
//                                          ジャンプ後、ステップ後
//==================================================================================
#endregion

    /// <summary>
    /// プレイヤーの硬直の処理
    /// 硬直はステップのスタミナ切れのみとする
    /// </summary>

namespace Maeda
{
    public partial class Player
    {
        public class StateStun : PlayerStateBase
        {
            #region オーバーライド関数
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                
                owner.stunCnt = 0;
                owner.rigidBody.velocity = Vector3.zero;
                owner.rigidBody.angularVelocity = Vector3.zero;
                owner.animator.SetBool("landing", false);
            }


            public override void OnUpdate(Player owner)
            {
                owner.stunCnt++;

                if (owner.stunCnt >= owner.baseParam.stepStun)
                {
                    if (owner.animator.GetBool("jump"))
                    {
                        //Debug.Log("前田yaa");
                        owner.ChangeState(stateJumping);
                    }
                    else
                        owner.ChangeState(stateStanding);
                }
               
            }

            public override void OnExit(Player owner, PlayerStateBase nextState)
            {
              
                owner.animator.SetBool("step", false);
            }
            #endregion
        }
    }
}


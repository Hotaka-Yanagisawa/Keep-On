using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateStanding
//	プレイヤーの立ち状態　
// 作成日時	:2021/03/19
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/19	立ち状態から行える遷移(移動、ジャンプ)を追加 
// 2021/03/23   立ち状態から移動状態へスムーズに行くための応急処置を追加
//              これだとステートマシンとしてはよくないのでまた対策を寝る必要がある
//==================================================================================
#endregion


/// <summary>
/// 立ち状態の処理
/// この状態から遷移できる状態は以下の通り
/// ・立ち状態
/// ・移動状態
/// ・ジャンプ状態
/// </summary>
namespace Maeda
{
    public partial class Player
    {
        public class StateStanding : PlayerStateBase
        {
            #region オーバーライド関数

            /// <summary>
            /// 状態遷移した最初に呼ばれる関数     Init()と同じ
            /// </summary>
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                Debug.Log("立ち状態");
                owner.animator.SetBool("jump", false);
                owner.rigidBody.velocity = Vector3.zero;
                //owner.rigidBody.angularVelocity = Vector3.zero;
                //owner.animator.SetFloat("speed", 0f,Time.deltaTime);
            }


            /// <summary>
            /// この状態の時繰り返し呼ばれる関数    Update()と同じ
            /// </summary>
            public override void OnUpdate(Player owner)
            {
                owner.animator.SetFloat("speed", 0f);
                owner.Velocity = owner.input.Player.Move.ReadValue<Vector2>();
                #region 入力による分岐
                // 移動入力の有無
                if (owner.oldPos != owner.transform.position || owner.input.Player.Move.triggered)               
                {
                    owner.ChangeState(stateMoving);     // 移動状態へ
                }

                // ジャンプ入力の有無
                else if (owner.input.Player.Jump.triggered)          
                {
                    owner.ChangeState(stateJumping);    // ジャンプ状態へ
                }

                // 攻撃入力の有無
                else if (owner.input.Player.Attack.triggered)
                {
                    owner.ChangeState(stateAttack);     // 攻撃状態へ
                }
                #endregion
            }
            #endregion
        }
    }
}

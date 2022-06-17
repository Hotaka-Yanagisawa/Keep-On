using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateMoving
//	プレイヤーの移動状態
// 作成日時	:2021/03/20
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/20   移動の処理を実装
//==================================================================================
#endregion

/// <summary>
/// 移動状態の処理
/// この状態から遷移できる状態は以下の通り
/// ・移動状態
/// ・ジャンプ状態
/// ・全方向の回避状態
/// ・立ち状態
/// </summary>
namespace Maeda
{
    public partial class Player
    {
        public class StateMoving : PlayerStateBase
        {

            #region オーバーライド関数


            /// <summary>
            /// 状態遷移した最初に呼ばれる関数     Init()と同じ
            /// </summary>
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                //Debug.Log("移動中");
                //owner.isJumping = false;
            }


            /// <summary>
            /// この状態の時繰り返し呼ばれる関数    Update()と同じ
            /// </summary>
            public override void OnUpdate(Player owner)
            {
               
                // インプットシステムからの入力の値を受け取る
                owner.Velocity = owner.input.Player.Move.ReadValue<Vector2>();

                // 前回のポジションの更新
                owner.oldPos = owner.transform.position;

                // 移動処理
               
                Moving(owner);

                #region 入力による分岐

                // ジャンプ入力はあったか
                if (owner.input.Player.Jump.triggered && !owner.animator.GetBool("jump"))
                    owner.ChangeState(stateJumping);                    // ジャンプ状態へ
                 
                // ステップ入力はあったか
                else if (owner.input.Player.Dodge.triggered && !owner.animator.GetBool("step"))
                    owner.ChangeState(stateDodging);                    // ステップ状態へ

                // 攻撃入力はあったか
                else if (owner.input.Player.Attack.triggered)
                    owner.ChangeState(stateAttack);                     // 攻撃状態へ

                // 前回のポジションと移動後のポジションは一緒か
                else if (owner.input.Player.Move.ReadValue<Vector2>() == Vector2.zero || owner.rigidBody.velocity == Vector3.zero)
                    owner.ChangeState(stateStanding);                   // 立ち状態へ


                #endregion
            }


            public override void OnFixUpdate(Player owner)
            {
               // Moving(owner);
            }


            public override void OnExit(Player owner, PlayerStateBase nextState)
            {
                
            }

            #endregion

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
            private void Moving(Player owner)
            {
                // 入力値の絶対値が一定値(stickBoundaryValue)を超えてる→走り
                if (Mathf.Abs(owner.Velocity.x) >= owner.baseParam.stickBoundaryValue || Mathf.Abs(owner.Velocity.y) >= owner.baseParam.stickBoundaryValue)
                {
                    //Debug.Log("走ってる");
                    owner.rigidBody.velocity = owner.moveForward * owner.baseParam.moveDash + new Vector3(0, owner.rigidBody.velocity.y, 0) * Time.deltaTime;
                }
                // 入力値の絶対値が一定値(stickBoundaryValue)を超えていない→歩き
                else
                {
                    //Debug.Log("歩いてる");
                    owner.rigidBody.velocity = owner.moveForward * owner.baseParam.moveWalk + new Vector3(0, owner.rigidBody.velocity.y, 0) * Time.deltaTime;
                }
            }

            #endregion
        }
    }
}



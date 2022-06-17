using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateAttack
//	プレイヤーの攻撃状態
// 作成日時	:2021/04/01
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/04/01   アニメーションに伴い攻撃の状態遷移を追加
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
        public class StateAttack : PlayerStateBase
        {
            int attacktype;
            #region オーバーライド関数

            /// <summary>
            /// 状態遷移した最初に呼ばれる関数     Init()と同じ
            /// </summary>
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                attacktype = 0;
                owner.animator.SetTrigger("attack");
               
            }


            /// <summary>
            /// この状態の時繰り返し呼ばれる関数    Update()と同じ
            /// </summary>
            public override void OnUpdate(Player owner)
            {
                //owner.rigidBody.velocity = Vector3.zero;
                if(owner.input.Player.Attack.triggered)
                {
                    attacktype++;
                }
                else if(!owner.input.Player.Attack.triggered && owner.oldPos == owner.transform.position)
                {
                    owner.ChangeState(stateStanding);
                }



                #region 入力の分岐
                // 移動入力の有無
                if (owner.oldPos != owner.transform.position || owner.input.Player.Move.triggered)
                {

                    owner.ChangeState(stateMoving);     // 移動状態へ
                }
                #endregion
            }

            public override void OnExit(Player owner, PlayerStateBase nextState)
            {
                owner.animator.ResetTrigger("attack");
            }

            #endregion
        }
    }
}

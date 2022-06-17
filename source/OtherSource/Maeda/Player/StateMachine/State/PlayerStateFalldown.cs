using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateDead
// プレイヤーの落下状態
// 作成日時	:2021/03/20
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/19   落下のアニメーション
//==================================================================================
#endregion

/// <summary>
/// 死亡時の処理
/// 何かあれば随時更新する
/// </summary>
namespace Maeda
{
    public partial class Player
    {
        public class StateFalldown : PlayerStateBase
        {
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                owner.animator.SetBool("falldown", true);
            }

            public override void OnUpdate(Player owner)
            {
                #region 入力による分岐


                //#region 素体
                //if (owner.baseParam.style.style == Style.E_Style.NORMAL)
                //{
                //    if (owner.input.Player.Dodge.triggered)
                //        owner.ChangeState(stateDodging);
                //}
                //#endregion

                //#region 火力型
                //if (owner.baseParam.style.style == Style.E_Style.POWER)
                //{

                //    if (owner.input.Player.Dodge.triggered)
                //        owner.ChangeState(stateDodging);
                //}
                //#endregion

                //#region 機動型
                //if (owner.baseParam.style.style == Style.E_Style.MOBILITY)
                //{

                //    if (owner.input.Player.Dodge.triggered)
                //        owner.ChangeState(stateDodging);
                //}
                //#endregion

                //#region 範囲型
                //if (owner.baseParam.style.style == Style.E_Style.REACH)
                //{
                //    if (owner.input.Player.Dodge.triggered)
                //        owner.ChangeState(stateDodging);
                //}
                //#endregion


                #endregion

                //if (!owner.animator.GetBool("falldown"))
                //{
                //    owner.ChangeState(stateLanding);
                //}
            }

            public override void OnExit(Player owner, PlayerStateBase nextState)
            {
                owner.animator.SetBool("falldown", false);
            }
        }
    }
}
 
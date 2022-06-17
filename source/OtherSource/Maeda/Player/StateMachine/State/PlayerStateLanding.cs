using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateLanding
// プレイヤーの落下状態
// 作成日時	:2021/03/20
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/19   着地のアニメーション
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
        public class StateLanding : PlayerStateBase
        {
            float landCnt = 0.5f;
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                owner.animator.SetBool("landing", true);
            }

            public override void OnUpdate(Player owner)
            {
                landCnt -= Time.deltaTime;


                if (!owner.animator.GetBool("landing"))
                {
                    owner.ChangeState(stateStanding);
                }

                if(landCnt <= 0f)
                {
                    owner.animator.SetBool("landing", false);
                    landCnt = 0.5f;
                    owner.ChangeState(stateStanding);
                }
            }

          
        }

        private void OnLandingEnd()
        {
            animator.SetBool("landing", false);
        }
    }
}

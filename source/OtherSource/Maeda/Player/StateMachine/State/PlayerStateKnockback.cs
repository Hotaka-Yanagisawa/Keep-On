using UnityEngine;
using Ohira;

#region HeaderComent
//==================================================================================
// PlayerStateKnockback
// プレイヤーのノックバック状態
// 作成日時	:2021/04/19
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/04/19   ノックバックの実装
//==================================================================================
#endregion

/// <summary>
/// ノックバック時の処理
/// </summary>
namespace Maeda
{
    public partial class Player
    {
        public class StateKnockBack : PlayerStateBase
        {
            #region プライベート変数
            //float knockbackTime =0.2395f;
            #endregion

            #region オーバーライド関数
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                owner.PlaySE("SE_Damage");
                if (!owner.animator.GetBool("knockback"))
                {
                    owner.rigidBody.velocity = new Vector3(0, owner.rigidBody.velocity.y, 0);
                    // owner.animator.SetInteger("knockbackType", 1);
                    owner.animator.SetBool("knockback", true);
                    // owner.SetInvinc(2.5f);

                    CameraController.Instance.ShakeCamera(5f, 3f, 1f);
                }
              
            }

            public override void OnUpdate(Player owner)
            {

                if (!owner.animator.GetBool("knockback") && !owner.animator.GetBool("standup"))
                {
                    owner.ChangeState(stateStanding);
                }

                owner.rigidBody.velocity = new Vector3(0, owner.rigidBody.velocity.y, 0);
            }

            public override void OnFixUpdate(Player owner)
            {
              
            }

            public override void OnExit(Player owner, PlayerStateBase nextState)
            {

                //knockbackTime = 0.2395f;
            }

            #endregion
        }
        #region プライベート関数
        private void KnockBackEnd()
        {
            if (animator.GetInteger("knockbackType") == 1)
                animator.SetBool("standup", true);

            animator.SetBool("knockback", false);
        }

        private void StandUpEnd()
        {
            animator.SetBool("standup", false);
            ChangeState(stateStanding);
        }
    
        #endregion
    }
}

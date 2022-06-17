using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateDead
// プレイヤーの死亡状態
// 作成日時	:2021/03/20
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/19   特に決まっていないので中身を空にしておく
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
        public class StateDead : PlayerStateBase
        {
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                // Destroy(owner.gameObject);
                //owner.gameObject.SetActive(false);
               // owner.animator.Play("")
                owner.rigidBody.velocity = Vector3.zero;
                owner.manage.isPlayerDead = true;
                owner.sotai.SetActive(false);
            }
        }
    }
}

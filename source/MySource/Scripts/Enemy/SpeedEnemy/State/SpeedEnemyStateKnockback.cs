//////////////////////////////
// SpeedEnemyStateKnockback.cs
//----------------------------
// 作成日:2021/4/25 
// 作成者:久田律貴
//----------------------------
// 更新日時・内容
//  ・スクリプト作成
//
//
//////////////////////////////
using UnityEngine;
using Homare;

namespace Hisada
{
    public partial class SpeedEnemy
    {
        /// <summary>
        /// ノックバック状態
        /// </summary>
        class StateKnockback : EnemyStateBase
        {
            int Cnt = 60;
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:ノックバック");

            }

            public override void OnUpdate(Enemy owner)
            {
                Cnt--;
                if (Cnt <= 0)
                {
                    owner.ChangeState(stateWaiting);
                }
            }
        }
    }
}

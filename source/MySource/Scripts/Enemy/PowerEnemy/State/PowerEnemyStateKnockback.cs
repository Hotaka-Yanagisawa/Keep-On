/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/18
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/03/18 作成開始
//            ノックバック時の処理を行う
// 
// 2021/04/22 再構築by三上
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;


namespace Homare
{
    public partial class PowerEnemy
    {
        /// <summary>
        /// ノックバック状態
        /// </summary>
        class StateKnockback : EnemyStateBase
        {
            int Cnt = 60;
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("ノックバック");

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
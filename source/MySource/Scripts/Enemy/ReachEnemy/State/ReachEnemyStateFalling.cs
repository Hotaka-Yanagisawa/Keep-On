/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/18
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/03/18 作成開始
//            待機状態の処理を行う
// 
// 2021/04/22 再構築by三上
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;
using Homare;


namespace Mikami
{
    public partial class ReachEnemy
    {
        /// <summary>
        /// 待機状態
        /// </summary>
        private class StateFalling : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                owner.rb.velocity = Vector3.zero;
                owner.GetComponent<EffectOperate>().CreateEffect(1, owner.transform.position, 1.7f);

                owner.rb.constraints = RigidbodyConstraints.FreezeRotationX |
                       RigidbodyConstraints.FreezeRotationY |
                       RigidbodyConstraints.FreezeRotationZ;
                //owner.animator.SetFloat("Speed", 0);

            }
        }
    }
}
//////////////////////////////
// SpeedEnemyStateFalling.cs
//----------------------------
// 作成日:2021/5/7 
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
            }


            //public override void OnFixedUpdate(Enemy owner)
            //{
            //    base.OnFixedUpdate(owner);
            //}
        }
    }
}

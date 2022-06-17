//////////////////////////////
// SpeedEnemyStateWaiting.cs
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
        /// 待機状態
        /// </summary>
        private class StateWaiting : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:待機");

                owner.actionCnt = 300;
                owner.rb.velocity = Vector3.zero;
                owner.rb.constraints = RigidbodyConstraints.FreezeRotationX |
                       RigidbodyConstraints.FreezeRotationY |
                       RigidbodyConstraints.FreezeRotationZ |
                       RigidbodyConstraints.FreezePositionY;

            }

            public override void OnUpdate(Enemy owner)
            {
                owner.rb.velocity = Vector3.zero;

                owner.actionCnt--;
                if (owner.actionCnt <= 0)
                {
                    owner.animator.SetBool("isWait", false);
                    owner.ChangeState(stateMoving);
                }
            }

        }
    }
}
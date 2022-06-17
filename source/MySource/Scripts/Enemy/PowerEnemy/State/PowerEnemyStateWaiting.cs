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


namespace Homare
{
    public partial class PowerEnemy
    {
        /// <summary>
        /// 待機状態
        /// </summary>
        private class StateWaiting : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("待機");

                owner.actionCnt = 2;
                owner.rb.velocity = Vector3.zero;
                owner.rb.constraints = RigidbodyConstraints.FreezeRotationX |
                                       RigidbodyConstraints.FreezeRotationY |
                                       RigidbodyConstraints.FreezeRotationZ |
                                       RigidbodyConstraints.FreezePositionY;
                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);
            }
            public override void OnUpdate(Enemy owner)
            {
                owner.rb.velocity = Vector3.zero;

                owner.actionCnt -= Time.deltaTime;
                if (owner.actionCnt <= 0)
                {
                    owner.ChangeState(stateMoving);
                }
            }
        }
    }
}
/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/18
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/03/18 作成開始
//            エネミー死亡時の処理を行う
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
        /// 死亡状態
        /// </summary>
        public class StateDead : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("死亡");
                owner.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                // 動作停止
                owner.actionCnt = 90;
                owner.GetComponent<EffectOperate>().CreateEffect(0, owner.transform.position, 1.7f);
                owner.rb.velocity = Vector3.zero;
                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);

            }

            public override void OnFixedUpdate(Enemy owner)
            {
                owner.actionCnt--;
                if (owner.actionCnt <= 0)
                {
                    Debug.Log("消えます");
                    //owner.ChangeState(stateWaiting);
                    owner.gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    owner.transform.position = new Vector3(0, 20, 0);

                    //消す
                    owner.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}
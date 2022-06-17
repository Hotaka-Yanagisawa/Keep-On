//////////////////////////////
// SpeedEnemyStateDead.cs
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
        /// 死亡状態
        /// </summary>
        public class StateDead : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:死亡");
                owner.animator.ResetTrigger("Found");
                owner.animator.SetBool("isPunch", false);
                owner.animator.SetBool("isWait", true);

                //owner.GetComponent<Animator>().SetBool("Dead", true);

                // 動作停止
                owner.actionCnt = 90;
                //owner.animator.SetBool("Dead", true);
                owner.GetComponent<EffectOperate>().CreateEffect(0, owner.transform.position, 1.7f);
                owner.rb.velocity = Vector3.zero;
            }

            public override void OnFixedUpdate(Enemy owner)
            {
                owner.rb.velocity = Vector3.zero;

                owner.actionCnt--;
                if (owner.actionCnt <= 0)
                {
                    Debug.Log("Speed:消滅");
                    //owner.creater.EnemyNum--;
                    //owner.transform.parent.gameObject.SetActive(false);
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

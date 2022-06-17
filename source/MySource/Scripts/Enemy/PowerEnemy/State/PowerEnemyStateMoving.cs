/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/18
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/03/18 作成開始
//            移動(徘徊)状態の処理を行う
//      03/26 徘徊範囲に制限を設ける
//      03/27 徘徊範囲を出た状態で移動状態になった場合帰還状態に遷移するようにした
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
        /// 移動状態
        /// </summary>
        class StateMoving : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("移動");

                owner.actionCnt = 1;
                owner.rb.velocity = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);
            }

            public override void OnFixedUpdate(Enemy owner)
            {
                // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;

                // いずれかの方向に移動している場合
                if (owner.rb.velocity.magnitude > 0)
                {
                    // 敵の角度の更新
                    // Slerp:現在の向き、向きたい方向、向かせるスピード
                    // LookRotation(向きたい方向):
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.enemyStatus.applySpeed);

                    owner.rb.velocity = owner.rb.velocity;
                }

                //移動制限
                Vector3 moveDistance = owner.transform.position - owner.firstPos;
                if (owner.enemyStatus.moveRange < moveDistance.magnitude)
                {
                    //owner.ChangeState(stateReturn);
                    //return;
                }

                //次の遷移用の処理
                owner.actionCnt-=Time.deltaTime;
                if (owner.actionCnt <= 0)
                {
                    owner.ChangeState(stateWaiting);
                }
            }

            public override void OnExit(Enemy owner, EnemyStateBase nextState)
            {
                owner.rb.velocity = Vector3.zero;
                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);
            }
        }
    }
}
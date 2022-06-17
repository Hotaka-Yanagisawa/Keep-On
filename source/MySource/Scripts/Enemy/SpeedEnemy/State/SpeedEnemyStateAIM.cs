//////////////////////////////
// SpeedEnemy.cs
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
        private class StateAIM : EnemyStateBase
        {
            float time = 1.208f * (24f / 60f);

            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                time = 1.5f;
                owner.rb.velocity = Vector3.zero;

                owner.animator.SetTrigger("Found");

            }

            public override void OnUpdate(Enemy owner)
            {
                float Angle = Mathf.Atan2(player.transform.position.z - owner.transform.position.z,
                 player.transform.position.x - owner.transform.position.x);

                var ToPlayer = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));

                // 敵の角度の更新
                // Slerp:現在の向き、向きたい方向、向かせるスピード
                // LookRotation(向きたい方向):
                owner.transform.rotation =
                    Quaternion.Slerp(owner.transform.rotation,
                    Quaternion.LookRotation(ToPlayer),
                    owner.enemyStatus.applySpeed);

                owner.rb.velocity = Vector3.zero;

                // ここに照準をプレイヤーに合わせ続ける処理を記述
                time -= Time.deltaTime;
                if(time < 0f)
                {
                    // ステートを変えるよ
                    owner.ChangeState(stateAttacking);
                }

            }

            public override void OnExit(Enemy owner, EnemyStateBase nextState)
            {
                owner.animator.SetBool("isPunch", true);
            }

        }
    }
}

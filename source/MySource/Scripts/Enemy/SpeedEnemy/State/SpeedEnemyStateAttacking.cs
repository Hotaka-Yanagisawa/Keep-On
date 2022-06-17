//////////////////////////////
// SpeedEnemyStateAttacking.cs
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
        class StateAttacking : EnemyStateBase
        {
            private const int ATTACK_TIME_CNT = 120;

            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:攻撃");

                owner.actionCnt = ATTACK_TIME_CNT;
                // プレイヤーの位置にvelocityを向ける
                float Angle = Mathf.Atan2(SpeedEnemy.player.transform.position.z - owner.transform.position.z,
                 SpeedEnemy.player.transform.position.x - owner.transform.position.x);

                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));
                // 攻撃時の機動ザコのスピード更新
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed * 2;


                SpeedEnemy speedEnemy = owner as SpeedEnemy;
                // Hit判定
                speedEnemy.colliders[(int)ColliderKind.AttackCollider].enabled = true;
                // Bodyの当たり判定
                Physics.IgnoreCollision(speedEnemy.colliders[(int)ColliderKind.BodyCollider],
                                        SpeedEnemy.playerCollider, true);

                Vector3 playerPos = player.transform.position;

                playerPos.y = owner.transform.position.y;

                // 敵の角度の更新
                // Slerp:現在の向き、向きたい方向、向かせるスピード
                // LookRotation(向きたい方向):
                owner.transform.rotation =
                    Quaternion.Slerp(owner.transform.rotation,
                    Quaternion.LookRotation(playerPos - owner.transform.position),
                    1);

            }

            public override void OnFixedUpdate(Enemy owner)
            {
                if (--owner.actionCnt <= 0)
                {
                    owner.animator.SetBool("isWait", true);
                    owner.ChangeState(stateWaiting);
                }

                owner.GetComponent<EffectOperate>().CreateEffect(5, owner.transform.position, 2f, true, owner.transform.rotation);
                owner.GetComponent<EffectOperate>().CreateEffect(6, owner.transform.position, 2f, true, owner.transform.rotation);
            }

            public override void OnExit(Enemy owner, EnemyStateBase nextState)
            {
                owner.rb.velocity = Vector3.zero;

                SpeedEnemy speedEnemy = owner as SpeedEnemy;
                // Hit判定
                speedEnemy.colliders[(int)ColliderKind.AttackCollider].enabled = false;
                // Bodyの当たり判定
                Physics.IgnoreCollision(speedEnemy.colliders[(int)ColliderKind.BodyCollider],
                                        SpeedEnemy.playerCollider, false);

                owner.animator.SetBool("isPunch", false);
                owner.animator.ResetTrigger("Found");

            }

        }
    }
}

//////////////////////////////
// SpeedEnemyBehaviour.cs
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
        public class SpeedEnemyBehavior
        {
            #region PublicVariable
            public float actionCnt;
            #endregion

            #region PrivateVariable
            private SpeedEnemy getEnemy;
            private BehaviorTreeController _behaviorTreeController; // コントローラー
            private float attacksStamina = 0;                       // 攻撃行動のスタミナ
            #endregion

            #region 起動型の挙動
            public SpeedEnemyBehavior(SpeedEnemy external)
            {
                getEnemy = external;
            }
            #endregion

            public void OnEnter()
            {
                actionCnt -= 1;
            }

            public void OnStart()
            {
                //アクションを初期化
                _behaviorTreeController = new BehaviorTreeController();

                

                #region CreateNode
                // rootノード⇒攻撃行動用のスタミナがあるかの確認
                SelectorNode rootNode = new SelectorNode();
                rootNode.name = "rootノード";

                // 攻撃行動用のスタミナがあるかの確認⇒各種攻撃の子ノードを持つノード
                DecoratorNode confirmStamina = new DecoratorNode();
                confirmStamina.name = "Speed:スタミナがあるか確認するDecoratorNode";
                confirmStamina.SetConditionFunc(() => { return ConfirmStamina(); });

                //通常攻撃を行う
                ActionNode atk = new ActionNode();
                atk.name = "Speed:通常攻撃を行うActionNode";
                atk.SetRunningFunc(() => { return AtkAction(); });

                //プレイヤーと言って距離を取る
                ActionNode move = new ActionNode();
                move.name = "Speed:プレイヤーと言って距離を取るActionNode";
                move.SetRunningFunc(() => { return MoveAction(); });

                #endregion

                #region AddChild
                // ノードの子供登録
                rootNode.AddChild(confirmStamina);
                rootNode.AddChild(move);
                //             ↓
                confirmStamina.AddChild(atk);

                #endregion

                // ツリー実行
                _behaviorTreeController.Initialize(rootNode);
                _behaviorTreeController.OnStart();
            }

            #region 更新処理
            public void OnUpdate()
            {
                _behaviorTreeController.OnRunning();
            }
            #endregion

            #region NodeFunc 
            /// <summary>
            /// 攻撃行動用のスタミナの更新と確認
            /// </summary>
            /// <returns>値次第</returns>
            NodeStatus ConfirmStamina()
            {
                //攻撃行動用のスタミナと行動のスタミナを回復する
                if (attacksStamina < 100) attacksStamina += 30.0f / 60.0f;

                //攻撃行動用のスタミナが一定以上なら成功
                return attacksStamina > 99 ? NodeStatus.SUCCESS : NodeStatus.WAITING;
            }

            /// <summary>
            /// 通常攻撃時の処理
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus AtkAction()
            {

                Vector3 moveDistance = getEnemy.transform.position - SpeedEnemy.player.transform.position;
                //actionCntが-1であった場合
                //ここに最初のフレームの処理を行う
                if (actionCnt < 0)
                {
                    getEnemy.animator.SetBool("AttackEnd", false);
                    actionCnt = 0;
                }
                //プレイヤーと一定距離離れてたら近づく
                if (1.0f < moveDistance.magnitude)
                {
                    MoveForPlayer(true);

                }

                getEnemy.animator.SetBool("Attack", true);


                actionCnt++;
                if (actionCnt % 180 == 0)
                {
                    actionCnt = -1;
                    attacksStamina = 0;
                    getEnemy.animator.SetBool("Attack", false);
                    getEnemy.animator.SetBool("AttackEnd", true);
                }

                return NodeStatus.SUCCESS;
            }

            /// <summary>
            /// プレイヤーと言って距離を取る処理
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus MoveAction()
            {
                Vector3 moveDistance = getEnemy.transform.position - SpeedEnemy.player.transform.position;
                //プレイヤーと一定距離近づいてたら距離を取る
                if (5 > moveDistance.magnitude)
                {
                    MoveForPlayer(false, 0.5f);
                }
                //プレイヤーと一定距離離れてたら近づく
                else if (5.1f < moveDistance.magnitude)
                {
                    MoveForPlayer(true);
                }
                //境目は止まる
                else
                {
                    getEnemy.rb.velocity = Vector3.zero;
                    circleMove();
                }
                return NodeStatus.SUCCESS;
            }
            #endregion

            /// <summary>
            /// プレイヤーを基準としたエネミーの移動
            /// </summary>
            /// <param name="approach">近づくか一定距離を保つか</param>
            /// <param name="moveSpeedControl">移動スピードの調整</param>
            void MoveForPlayer(bool approach, float moveSpeedControl = 1)
            {
                float Angle = Mathf.Atan2(SpeedEnemy.player.transform.position.z - getEnemy.transform.position.z,
                      SpeedEnemy.player.transform.position.x - getEnemy.transform.position.x);

                getEnemy.rb.velocity = new Vector3(Mathf.Cos(Angle), getEnemy.rb.velocity.y, Mathf.Sin(Angle));
                // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
                getEnemy.rb.velocity = getEnemy.rb.velocity.normalized * getEnemy.enemyStatus.moveSpeed * moveSpeedControl;

                getEnemy.animator.SetFloat("Speed", getEnemy.rb.velocity.magnitude);

                // いずれかの方向に移動している場合
                if (getEnemy.rb.velocity.magnitude > 0)
                {
                    if (approach)
                    {
                        getEnemy.rb.velocity = getEnemy.rb.velocity;
                    }
                    else
                    {
                        getEnemy.rb.velocity = -getEnemy.rb.velocity;
                    }
                }
            }

            /// <summary>
            /// プレイヤーを中心に円を描いて移動
            /// </summary>
            void circleMove()
            {
              
            }

        }
    }

}
/////////////////////////////////////////////////////////////////////////
// 作成日 2021/05/14
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/05/14 作成開始
//             パワーボスビヘイビアツリーを作成
// 
//
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;
namespace Homare
{
    public partial class Boss
    {
        //攻撃⇒風圧⇒突進
        public class PowerBehavior
        {
            public enum Action
            {
                Non,
                Bombing,
                Wind,
                Lunge,
            }

            #region PublicVariable
            public Action action;
            public float actionCnt = -1;
            #endregion

            #region PrivateVariable
            private Boss getBoss;
            private BehaviorTreeController _behaviorTreeController; // コントローラー
            private float attacksStamina = 100;                     // 攻撃行動のスタミナ
            private uint bombingStamina = 100;                      // 爆撃攻撃のスタミナ
            private uint windStamina = 100;                         // 風圧のスタミナ
            private uint lungeStamina = 100;                        // 突進のスタミナ
            #endregion

            public PowerBehavior(Boss external)
            {
                getBoss = external;
            }

            public void OnEnter()
            {
                actionCnt = -1;
                action = Action.Non;
            }

            public void OnStart()
            {
                //アクションを初期化
                action = Action.Non;
                _behaviorTreeController = new BehaviorTreeController();

                #region CreateNode
                // rootノード⇒攻撃行動用のスタミナがあるかの確認
                SelectorNode rootNode = new SelectorNode();
                rootNode.name = "rootノード";

                // 攻撃行動用のスタミナがあるかの確認⇒各種攻撃の子ノードを持つノード
                DecoratorNode confirmStamina = new DecoratorNode();
                confirmStamina.name = "スタミナがあるか確認するDecoratorNode";
                confirmStamina.SetConditionFunc(() => { return ConfirmStamina(); });

                //各種攻撃の子ノードを持つノード⇒各種攻撃のスタミナを確認
                SelectorNode atkAction = new SelectorNode();
                atkAction.name = "攻撃アクションを行うノード";

                // 爆撃攻撃スタミナの確認⇒通常攻撃を行う
                DecoratorNode confirmBombingStamina = new DecoratorNode();
                confirmBombingStamina.name = "爆撃攻撃スタミナがあるか確認するDecoratorNode";
                confirmBombingStamina.SetConditionFunc(() => { return ConfirmBombingStamina(); });

                // 風圧スタミナの確認⇒風圧を行う
                DecoratorNode confirmWindStamina = new DecoratorNode();
                confirmWindStamina.name = "風圧スタミナがあるか確認するDecoratorNode";
                confirmWindStamina.SetConditionFunc(() => { return ConfirmWindStamina(); });

                // 突進スタミナの確認⇒突進を行う
                DecoratorNode confirmLungeStamina = new DecoratorNode();
                confirmLungeStamina.name = "突進スタミナがあるか確認するDecoratorNode";
                confirmLungeStamina.SetConditionFunc(() => { return ConfirmLungeStamina(); });

                //通常攻撃を行う
                ActionNode atk = new ActionNode();
                atk.name = "通常攻撃を行うActionNode";
                atk.SetRunningFunc(() => { return BombingAction(); });

                //風圧を行う
                ActionNode wind = new ActionNode();
                wind.name = "風圧を行うActionNode";
                wind.SetRunningFunc(() => { return WindAction(); });

                //突進を行う
                ActionNode lunge = new ActionNode();
                lunge.name = "突進を行うActionNode";
                lunge.SetRunningFunc(() => { return LungeAction(); });

                #endregion

                #region AddChild
                // ノードの子供登録
                rootNode.AddChild(confirmStamina);
                //             ↓
                confirmStamina.AddChild(atkAction);
                //             ↓
                atkAction.AddChild(confirmBombingStamina);
                atkAction.AddChild(confirmWindStamina);
                atkAction.AddChild(confirmLungeStamina);
                //             ↓
                confirmBombingStamina.AddChild(atk);
                confirmWindStamina.AddChild(wind);
                confirmLungeStamina.AddChild(lunge);
                #endregion

                // ツリー実行
                _behaviorTreeController.Initialize(rootNode);
                _behaviorTreeController.OnStart();
            }

            /// <summary>
            /// 更新処理
            /// </summary>
            public void OnUpdate()
            {
                _behaviorTreeController.OnRunning();
            }


            #region NodeFunc

            /// <summary>
            /// 攻撃行動用のスタミナの更新と確認
            /// </summary>
            /// <returns>status</returns>
            NodeStatus ConfirmStamina()
            {
                //攻撃行動用のスタミナと行動のスタミナを回復する
                if (attacksStamina < 100) attacksStamina += 1;
                if (bombingStamina < 100) bombingStamina++;
                if (windStamina < 100) windStamina++;
                if (lungeStamina < 100) lungeStamina++;

                //攻撃行動用のスタミナが一定以上なら成功
                NodeStatus status = attacksStamina > 99 ? NodeStatus.SUCCESS : NodeStatus.WAITING;

                if (status == NodeStatus.SUCCESS)
                {
                    Debug.Log("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
                }
                else
                {
                    //Debug.Log("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
                }
                return status;
            }

            /// <summary>
            /// 爆撃攻撃のスタミナがあるかの確認
            /// </summary>
            /// <returns></returns>
            NodeStatus ConfirmBombingStamina()
            {
                NodeStatus status;

                if (bombingStamina > 99)
                {
                    status = NodeStatus.SUCCESS;
                    if (action == Action.Non) action = Action.Bombing;
                    if (action != Action.Bombing) return NodeStatus.RUNNING;
                }
                else
                {
                    status = NodeStatus.RUNNING;
                }
                return status;
            }

            /// <summary>
            /// 風圧のスタミナががあるかの確認
            /// </summary>
            /// <returns>status</returns>
            NodeStatus ConfirmWindStamina()
            {
                NodeStatus status;
                if (windStamina > 99)
                {
                    status = NodeStatus.SUCCESS;
                    if (action == Action.Non) action = Action.Wind;
                    if (action != Action.Wind) return NodeStatus.RUNNING;
                }
                else
                {
                    status = NodeStatus.RUNNING;
                }

                return status;
            }

            /// <summary>
            /// 突進のスタミナの確認
            /// </summary>
            /// <returns>status</returns>
            NodeStatus ConfirmLungeStamina()
            {
                NodeStatus status;
                if (lungeStamina > 99)
                {
                    status = NodeStatus.SUCCESS;
                    if (action == Action.Non) action = Action.Lunge;
                    if (action != Action.Lunge) return NodeStatus.RUNNING;
                }
                else
                {
                    status = NodeStatus.RUNNING;
                }
                return status;
            }

            /// <summary>
            /// 爆撃攻撃時の処理
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus BombingAction()
            {
                if (actionCnt < 0)
                {
                    actionCnt = 0;
                    getBoss.searchArea.enabled = true;
                }
                actionCnt += Time.deltaTime * 1;

                if (getBoss.isAtk)
                {

                }

                if (actionCnt > 15)
                {
                    getBoss.isAtk = false;
                    actionCnt = -1;
                    attacksStamina -= 50;
                    bombingStamina = 0;
                    action = Action.Non;
                }

                return NodeStatus.SUCCESS;
            }

            /// <summary>
            /// 風圧の処理を行う
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus WindAction()
            {
                if (actionCnt < 0)
                {
                    Debug.Log("風圧");
                    actionCnt = 0;
                }
                actionCnt++;
                if (actionCnt == 30)
                {
                    actionCnt = -1;
                    windStamina = 0;
                    attacksStamina -= 10;
                    action = Action.Non;
                }

                return NodeStatus.SUCCESS;
            }

            /// <summary>
            /// 突進の処理を行う
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus LungeAction()
            {
                //ここ真似して書く
                lungeStamina = 0;
                attacksStamina -= 100;
                action = Action.Non;
                Debug.Log("突進");
                return NodeStatus.SUCCESS;
            }

            #endregion
            void SetArc(bool atkKind)
            {
                if (atkKind)
                {
                    getBoss.arc.enabled = true;
                    getBoss.arc.endPos = getBoss.player.transform.position;
                }
                else
                {
                    getBoss.arc.enabled = false;
                }
            }

            /// <summary>
            /// プレイヤーを基準としたエネミーの移動
            /// </summary>
            /// <param name="approach">近づくか一定距離を保つか</param>
            /// <param name="moveSpeedControl">移動スピードの調整</param>
            void MoveForPlayer(bool approach, float moveSpeedControl = 1)
            {
                float Angle = Mathf.Atan2(getBoss.player.transform.position.z - getBoss.transform.position.z,
                      getBoss.player.transform.position.x - getBoss.transform.position.x);

                getBoss.rb.velocity = new Vector3(Mathf.Cos(Angle), getBoss.rb.velocity.y, Mathf.Sin(Angle));
                //getBoss.velocity.x = Mathf.Cos(Angle);
                //getBoss.velocity.z = Mathf.Sin(Angle);
                // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
                getBoss.rb.velocity = getBoss.rb.velocity.normalized * getBoss.moveSpeed * moveSpeedControl;

                getBoss.animator.SetFloat("Speed", getBoss.rb.velocity.magnitude);

                // いずれかの方向に移動している場合
                if (getBoss.rb.velocity.magnitude > 0)
                {
                    if (approach)
                    {
                        getBoss.rb.velocity = getBoss.rb.velocity;
                    }
                    else
                    {
                        getBoss.rb.velocity = -getBoss.rb.velocity;
                    }
                }
            }
        }
    }
}
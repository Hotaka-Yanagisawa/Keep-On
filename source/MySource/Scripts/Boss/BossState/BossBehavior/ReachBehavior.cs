/////////////////////////////////////////////////////////////////////////
// 作成日 2021/05/10
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/05/10 作成開始
//             Reachボスビヘイビアツリーを作成
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
        public class ReachBehavior
        {
            public enum Action
            {
                Non,
                //CMove,
                Bombing,
                Wind,
                Laser,
            }

            #region PublicVariable
            public Action action;
            public float actionCnt = -1;
            #endregion

            #region PrivateVariable
            private Boss getBoss;
            private BehaviorTreeController _behaviorTreeController; // コントローラー
            private float attacksStamina = 100;                     // 攻撃行動のスタミナ
            //private uint cMoveStamina = 100;                      // 中央移動のスタミナ
            private uint bombingStamina = 100;                      // 爆撃攻撃のスタミナ
            private uint windStamina = 100;                         // 風圧のスタミナ
            private uint laserStamina = 100;                        // 突進のスタミナ
            //private int AtkCnt;                                     // 攻撃何回目か
            //private int AtkEndNum;                                  // 終了回数(攻撃)
            //int atkType;
            #endregion

            public ReachBehavior(Boss external)
            {
                getBoss = external;
                getBoss.atkCnt = 1;
                //atkType = 0;
            }

            public void OnEnter()
            {
                actionCnt = -1;
                action = Action.Non;
                //atkType = 0;

            }

            public void OnStart()
            {
                getBoss.isAtk = false;

                //atkType = 0;

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
                
                // 中央移動スタミナの確認⇒通常攻撃を行う
                //DecoratorNode confirmCMoveStamina = new DecoratorNode();
                //confirmCMoveStamina.name = "爆撃攻撃スタミナがあるか確認するDecoratorNode";
                //confirmCMoveStamina.SetConditionFunc(() => { return ConfirmCMoveStamina(); });

                // 爆撃攻撃スタミナの確認⇒通常攻撃を行う
                DecoratorNode confirmBombingStamina = new DecoratorNode();
                confirmBombingStamina.name = "爆撃攻撃スタミナがあるか確認するDecoratorNode";
                confirmBombingStamina.SetConditionFunc(() => { return ConfirmBombingStamina(); });

                // 風圧スタミナの確認⇒風圧を行う
                DecoratorNode confirmWindStamina = new DecoratorNode();
                confirmWindStamina.name = "風圧スタミナがあるか確認するDecoratorNode";
                confirmWindStamina.SetConditionFunc(() => { return ConfirmWindStamina(); });

                // 突進スタミナの確認⇒突進を行う
                DecoratorNode confirmLaserStamina = new DecoratorNode();
                confirmLaserStamina.name = "突進スタミナがあるか確認するDecoratorNode";
                confirmLaserStamina.SetConditionFunc(() => { return ConfirmLaserStamina(); });

                //爆撃攻撃を行う
                ActionNode atk = new ActionNode();
                atk.name = "通常攻撃を行うActionNode";
                atk.SetRunningFunc(() => { return BombingAction(); });

                //風圧を行う
                ActionNode wind = new ActionNode();
                wind.name = "風圧を行うActionNode";
                wind.SetRunningFunc(() => { return WindAction(); });

                //突進を行う
                ActionNode laser = new ActionNode();
                laser.name = "突進を行うActionNode";
                laser.SetRunningFunc(() => { return LaserAction(); });

                #endregion

                #region AddChild
                // ノードの子供登録
                rootNode.AddChild(confirmStamina);
                //             ↓
                confirmStamina.AddChild(atkAction);
                //             ↓
                atkAction.AddChild(confirmBombingStamina);
                atkAction.AddChild(confirmWindStamina);
                atkAction.AddChild(confirmLaserStamina);
                //             ↓
                confirmBombingStamina.AddChild(atk);
                confirmWindStamina.AddChild(wind);
                confirmLaserStamina.AddChild(laser);
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
                if (laserStamina < 100) laserStamina++;

                //攻撃行動用のスタミナが一定以上なら成功
                NodeStatus status = attacksStamina > 99 ? NodeStatus.SUCCESS : NodeStatus.WAITING;

                //if (status == NodeStatus.SUCCESS)
                //{
                //    Debug.Log("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
                //}
                //else
                //{
                //    //Debug.Log("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
                //}
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
            NodeStatus ConfirmLaserStamina()
            {
                NodeStatus status;
                if (laserStamina > 99)
                {
                    status = NodeStatus.SUCCESS;
                    if (action == Action.Non) action = Action.Laser;
                    if (action != Action.Laser) return NodeStatus.RUNNING;
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
                //ノードに来て最初の処理
                if (actionCnt < 0)
                {
                    //カウントの初期化、チャージモーション、フラグの初期化
                    actionCnt = 0;
                    getBoss.animator.SetInteger("AtkType", 0);
                    getBoss.animator.SetBool("Attack", true);
                    getBoss.isAtk = false;
                }

                //関係あるモーションかどうか
                if (getBoss.animator.GetCurrentAnimatorStateInfo(0).IsTag("extinction"))
                {
                    //まだ攻撃フラグが立ってなく攻撃モーション中なら
                    if (getBoss.animator.GetInteger("AtkType") == 1 && !getBoss.isAtk)
                    {
                        //攻撃開始、攻撃フラグ立てる
                        SetWind(true, 40);
                        SetExtinction(true);
                        getBoss.isAtk = true;
                    }
                    //攻撃中ならカウントを進める
                    else if(getBoss.isAtk) actionCnt += Time.deltaTime;
                    //カウントが進んだらendモーションへ移行
                    if(actionCnt > 5) getBoss.animator.SetInteger("AtkType", 2);
                    //endモーションで攻撃フラグが立っていたら終了処理へ
                    if (getBoss.animator.GetInteger("AtkType") == 2 && getBoss.isAtk)
                    {
                        //モーションを変更
                        getBoss.animator.SetBool("Attack", false);
                        //攻撃終了
                        SetExtinction(false);
                        SetWind(false, 40);
                        //例外値に
                        actionCnt = -1;
                        action = Action.Non;
                        //スタミナ減らす
                        attacksStamina -= 50;
                        bombingStamina = 0;
                        //仮チェンジ下に別のあり
                        getBoss.ChangeStateMachine(BossType.Speed);
                    }
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
                    //ここでWindをONにする
                    SetWind(true, 300);
                }
                actionCnt++;
                if (actionCnt == 30)
                {
                    SetWind(false, 40);
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
            NodeStatus LaserAction()
            {
                if (actionCnt < 0)
                {
                    //Debug.Log("爆撃攻撃");
                    //SetWind(true, 40);
                    //SetExtinction(true);
                    actionCnt = 0;
                }
                actionCnt += Time.deltaTime * 1;

                if (actionCnt > 1)
                {
                    //SetExtinction(false);
                    //SetWind(false, 40);
                    //bombingStamina = 0;
                    attacksStamina -= 50;
                    actionCnt = -1;
                    action = Action.Non;
                    getBoss.atkCnt--;
                    if (getBoss.atkCnt <= 0) getBoss.ChangeStateMachine(BossType.Speed);
                }
                return NodeStatus.SUCCESS;
            }

            #endregion

            void SetWind(bool active, float Power)
            {
                getBoss.wind.SetActive(active);
                getBoss.wind.GetComponent<Wind>().windPower = Power;

                if (active)
                {
                    getBoss.vEffect.Play();
                }
                else
                {
                    getBoss.vEffect.Stop();
                }
            }

            void SetExtinction(bool exist)
            {
                //getBoss.extinction.enabled = exist;
                getBoss.divisionExtinction.enabled = exist;
            }
        }
    }
}
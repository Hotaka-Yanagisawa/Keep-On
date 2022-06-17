/////////////////////////////////////////////////////////////////////////
// 作成日 2021/05/14
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/05/14 作成開始
//             Speedボスビヘイビアツリーを作成
//              
//
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
namespace Homare
{
    public partial class Boss
    {
        //攻撃⇒直進攻撃⇒ジャンプ攻撃
        public class SpeedBehavior
        {
            public enum Action
            {
                Non,
                //Jump,               // ジャンプ
                //StraightAttack,     // 直進攻撃
                SwoopAttack,        // ジャンプ攻撃
            }

            #region PublicVariable
            public Action action;
            public float actionCnt = -1;
            #endregion

            #region PrivateVariable
            private Boss getBoss;
            private BehaviorTreeController _behaviorTreeController; // コントローラー
            private float attacksStamina = 100;                     // 攻撃行動のスタミナ
            private uint jumpAtkStamina = 100;                      // ジャンプ攻撃スタミナ
            //private uint jumpingStamina = 100;                      // ジャンプのスタミナ
            //private uint stAtkStamina = 100;                        // 直進攻撃のスタミナ
            //private int AtkCnt;                                     // 攻撃何回目か
            //private int AtkEndNum;                                  // 終了回数(攻撃)
            //bool isCharge;
            #endregion

            public SpeedBehavior(Boss external)
            {
                getBoss = external;
                getBoss.atkCnt = 1;
                jumpAtkStamina = 100;
                //jumpingStamina = 100;
                //stAtkStamina = 100;
                //isCharge = false;
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

                // ジャンプ攻撃スタミナの確認⇒ジャンプ攻撃を行う
                DecoratorNode confirmJumpAtkStamina = new DecoratorNode();
                confirmJumpAtkStamina.name = "ジャンプ攻撃スタミナがあるか確認するDecoratorNode";
                confirmJumpAtkStamina.SetConditionFunc(() => { return ConfirmJumpAtkStamina(); });
                
                // ジャンプスタミナの確認⇒通常攻撃を行う
                //DecoratorNode confirmJumpingStamina = new DecoratorNode();
                //confirmJumpingStamina.name = "ジャンプスタミナがあるか確認するDecoratorNode";
                //confirmJumpingStamina.SetConditionFunc(() => { return ConfirmJumpingStamina(); });

                //// 直進攻撃スタミナの確認⇒直進攻撃を行う
                //DecoratorNode confirmStAtkStamina = new DecoratorNode();
                //confirmStAtkStamina.name = "直進攻撃スタミナがあるか確認するDecoratorNode";
                //confirmStAtkStamina.SetConditionFunc(() => { return ConfirmStAtkStamina(); });


                //ジャンプ攻撃を行う
                ActionNode jumpAtk = new ActionNode();
                jumpAtk.name = "ジャンプ攻撃を行うActionNode";
                jumpAtk.SetRunningFunc(() => { return JumpAtkAction(); });

                ////ジャンプを行う
                //ActionNode jump = new ActionNode();
                //jump.name = "通常攻撃を行うActionNode";
                //jump.SetRunningFunc(() => { return JumpAction(); });

                ////直進攻撃を行う
                //ActionNode stAtk = new ActionNode();
                //stAtk.name = "直進攻撃を行うActionNode";
                //stAtk.SetRunningFunc(() => { return stAtkAction(); });


                #endregion

                #region AddChild
                // ノードの子供登録
                rootNode.AddChild(confirmStamina);
                //             ↓
                confirmStamina.AddChild(atkAction);
                //             ↓
                atkAction.AddChild(confirmJumpAtkStamina);
                //atkAction.AddChild(confirmJumpingStamina);
                //atkAction.AddChild(confirmStAtkStamina);
                //             ↓
                confirmJumpAtkStamina.AddChild(jumpAtk);
                //confirmJumpingStamina.AddChild(jump);
                //confirmStAtkStamina.AddChild(stAtk);
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
                //if (jumpAtkStamina < 100) jumpAtkStamina++;
                //if (jumpingStamina < 100) jumpingStamina++;
                //if (stAtkStamina < 100) stAtkStamina++;

                //攻撃行動用のスタミナが一定以上なら成功
                NodeStatus status = attacksStamina > 99 ? NodeStatus.SUCCESS : NodeStatus.WAITING;

                return status;
            }

            /// <summary>
            /// ジャンプのスタミナがあるかの確認
            /// </summary>
            /// <returns></returns>
            //NodeStatus ConfirmJumpingStamina()
            //{
            //    NodeStatus status;

            //    if (jumpingStamina > 99)
            //    {
            //        status = NodeStatus.SUCCESS;
            //        if (action == Action.Non) action = Action.Jump;
            //        if (action != Action.Jump) return NodeStatus.RUNNING;
            //    }
            //    else
            //    {
            //        status = NodeStatus.RUNNING;
            //    }
            //    return status;
            //}

            /// <summary>
            /// ジャンプ攻撃のスタミナの確認
            /// </summary>
            /// <returns>status</returns>
            NodeStatus ConfirmJumpAtkStamina()
            {
                NodeStatus status;
                if (jumpAtkStamina > 99)
                {
                    status = NodeStatus.SUCCESS;
                    if (action == Action.Non) action = Action.SwoopAttack;
                    if (action != Action.SwoopAttack) return NodeStatus.RUNNING;
                }
                else
                {
                    status = NodeStatus.RUNNING;
                }
                return status;
            }
            
            /// <summary>
            /// 直進攻撃のスタミナががあるかの確認
            /// </summary>
            /// <returns>status</returns>
            //NodeStatus ConfirmStAtkStamina()
            //{
            //    NodeStatus status;
            //    if (stAtkStamina > 99)
            //    {
            //        status = NodeStatus.SUCCESS;
            //        if (action == Action.Non) action = Action.StraightAttack;
            //        if (action != Action.StraightAttack) return NodeStatus.RUNNING;
            //    }
            //    else
            //    {
            //        status = NodeStatus.RUNNING;
            //    }

            //    return status;
            //}


            /// <summary>
            /// ジャンプ攻撃の処理を行う
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus JumpAtkAction()
            {
                if (actionCnt < 0)
                {
                    actionCnt = 0;
                    //SetArc(true);
                    getBoss.animator.SetInteger("AtkType", 0);
                    getBoss.animator.SetBool("Attack", true);
                }
                actionCnt += Time.deltaTime;

                if (actionCnt > 5)
                {
                    if (getBoss.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                    {
                        actionCnt = -1;
                        attacksStamina -= 50;
                        jumpAtkStamina = 0;
                        action = Action.Non;
                        getBoss.animator.SetBool("Attack", false);
                        getBoss.ChangeStateMachine(BossType.Power);
                    }
                }

                return NodeStatus.SUCCESS;
            }
            /// <summary>
            /// ジャンプ時の処理
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            //NodeStatus JumpAction()
            //{
            //    if (actionCnt < 0)
            //    {
            //        actionCnt = 0;
            //        getBoss.animator.SetInteger("AtkType", 1);
            //        getBoss.animator.SetBool("Attack", true);
            //    }
            //    actionCnt += Time.deltaTime;
            //    if (actionCnt > 5)
            //    {
            //        if (getBoss.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            //        {
            //            actionCnt = -1;
            //            jumpingStamina = 0;
            //            attacksStamina -= 10;
            //            action = Action.Non;
            //            getBoss.animator.SetBool("Attack", false);
            //        }
            //    }
            //    return NodeStatus.SUCCESS;
            //}

            /// <summary>
            /// 直進攻撃の処理を行う
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            //NodeStatus stAtkAction()
            //{
            //    if (actionCnt < 0 && !isCharge)
            //    {
            //        actionCnt = 0;
            //        //チャージモーション
            //        getBoss.animator.SetInteger("AtkType", 2);
            //        getBoss.animator.SetBool("Attack", true);
            //    }
            //    actionCnt += Time.deltaTime;

            //    if (actionCnt > 5 && !isCharge)
            //    {
            //        if (getBoss.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            //        {
            //            actionCnt = -10;                                                              
            //            isCharge = true;
            //        }
            //    }
            //    else if (isCharge)
            //    {
            //        if (actionCnt < 0)
            //        {
            //            actionCnt = 0;
            //            //パンチモーション
            //            getBoss.animator.SetInteger("AtkType", 3);                       
            //        }
            //        if (actionCnt > 5)
            //        {
            //            if (getBoss.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            //            {
            //                actionCnt = -1;
            //                stAtkStamina = 0;
            //                jumpAtkStamina = 100;
            //                jumpingStamina = 100;
            //                stAtkStamina = 100;
            //                attacksStamina -= 10;
            //                action = Action.Non;
            //                getBoss.animator.SetBool("Attack", false);
            //                isCharge = false;
            //                getBoss.ChangeStateMachine(Type.Reach);
            //            }
            //        }
            //    }
            //    return NodeStatus.SUCCESS;
            //}


            #endregion

            //void SetWind(bool active, float Power)
            //{
            //    getBoss.wind.SetActive(active);
            //    getBoss.wind.GetComponent<Wind>().windPower = Power;

            //    if (active)
            //    {
            //        getBoss.vEffect.Play();
            //    }
            //    else
            //    {
            //        getBoss.vEffect.Stop();
            //    }
            //}

            //void Atk()
            //{
            //    float Angle = Mathf.Atan2(getBoss.player.transform.position.z - getBoss.transform.position.z,
            //                     getBoss.player.transform.position.x - getBoss.transform.position.x);

            //    getBoss.rb.velocity = new Vector3(Mathf.Cos(Angle), getBoss.rb.velocity.y, Mathf.Sin(Angle));
            //    // 攻撃時の機動ザコのスピード更新
            //    getBoss.rb.velocity = getBoss.rb.velocity.normalized * getBoss.moveSpeed * 2;

            //    // Hit判定
            //    speedEnemy.colliders[(int)ColliderKind.AttackCollider].enabled = true;
            //    // Bodyの当たり判定
            //    Physics.IgnoreCollision(speedEnemy.colliders[(int)ColliderKind.BodyCollider],
            //                            SpeedEnemy.playerCollider, true);

            //    Vector3 playerPos = getBoss.transform.position;

            //    playerPos.y = getBoss.transform.position.y;

            //    // 敵の角度の更新
            //    // Slerp:現在の向き、向きたい方向、向かせるスピード
            //    // LookRotation(向きたい方向):
            //    getBoss.transform.rotation =
            //        Quaternion.Slerp(getBoss.transform.rotation,
            //        Quaternion.LookRotation(playerPos - getBoss.transform.position),
            //        1);
            //}

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
        }
        //void OnArc()
        //{
        //    arc.enabled = true;
        //    arc.endPos = player.transform.position;
        //    owner.bossAction = BossAction.
        //}
        
        //void OnFootCol()
        //{
        //    fallDownAtk.SetActive(true);
        //    fallDownAtk.GetComponent<FallDownAtk>().enabled = true;
        //}
    }
}
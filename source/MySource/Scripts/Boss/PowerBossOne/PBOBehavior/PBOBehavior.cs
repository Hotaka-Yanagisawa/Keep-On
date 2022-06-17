using UnityEngine;

public partial class PBO
{
    //攻撃⇒風圧⇒突進
    public class PBOBehavior
    {
        public enum Action
        {
            Non,
            Atk,
            Wind,
            Lunge,
        }

        #region PublicVariable
        public Action action;
        public float actionCnt;
        #endregion

        #region PrivateVariable
        private PBO getPBO;
        private BehaviorTreeController _behaviorTreeController; // コントローラー
        private float attacksStamina = 100;                     // 攻撃行動のスタミナ
        private uint atkStamina = 100;                          // 通常攻撃のスタミナ
        private uint windStamina = 100;                         // 風圧のスタミナ
        private uint lungeStamina = 100;                        // 突進のスタミナ
        #endregion

        public PBOBehavior(PBO external)
        {
            getPBO = external;
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
            confirmStamina.SetConditionFunc(() =>{ return ConfirmStamina();});

            //各種攻撃の子ノードを持つノード⇒各種攻撃のスタミナを確認
            SelectorNode atkAction = new SelectorNode();
            atkAction.name = "攻撃アクションを行うノード";

            // 通常攻撃スタミナの確認⇒通常攻撃を行う
            DecoratorNode confirmAtkStamina = new DecoratorNode();
            confirmAtkStamina.name = "通常攻撃スタミナがあるか確認するDecoratorNode";
            confirmAtkStamina.SetConditionFunc(() =>{ return ConfirmAtkStamina();});

            // 風圧スタミナの確認⇒風圧を行う
            DecoratorNode confirmWindStamina = new DecoratorNode();
            confirmWindStamina.name = "風圧スタミナがあるか確認するDecoratorNode";
            confirmWindStamina.SetConditionFunc(() =>{ return ConfirmWindStamina();});

            // 突進スタミナの確認⇒突進を行う
            DecoratorNode confirmLungeStamina = new DecoratorNode();
            confirmLungeStamina.name = "突進スタミナがあるか確認するDecoratorNode";
            confirmLungeStamina.SetConditionFunc(() =>{ return ConfirmLungeStamina();});

            //通常攻撃を行う
            ActionNode atk = new ActionNode();
            atk.name = "通常攻撃を行うActionNode";
            atk.SetRunningFunc(() =>{ return AtkAction();});

            //風圧を行う
            ActionNode wind = new ActionNode();
            wind.name = "風圧を行うActionNode";
            wind.SetRunningFunc(() =>{ return WindAction();});

            //突進を行う
            ActionNode lunge = new ActionNode();
            lunge.name = "突進を行うActionNode";
            lunge.SetRunningFunc(() =>{ return LungeAction();});

            #endregion

            #region AddChild
            // ノードの子供登録
            rootNode.AddChild(confirmStamina);
            //             ↓
            confirmStamina.AddChild(atkAction);
            //             ↓
            atkAction.AddChild(confirmAtkStamina);
            atkAction.AddChild(confirmWindStamina);
            atkAction.AddChild(confirmLungeStamina);
            //             ↓
            confirmAtkStamina.AddChild(atk);
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
            if (atkStamina < 100) atkStamina++;
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
        /// 通常攻撃のスタミナがあるかの確認
        /// </summary>
        /// <returns></returns>
        NodeStatus ConfirmAtkStamina()
        {
            NodeStatus status;
            
            if (atkStamina > 99)
            {
                status = NodeStatus.SUCCESS;
                if (action == Action.Non) action = Action.Atk;
                if (action != Action.Atk) return NodeStatus.RUNNING;
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
        /// 通常攻撃時の処理
        /// </summary>
        /// <returns>NodeStatus.SUCCESS</returns>
        NodeStatus AtkAction()
        {
            Debug.LogError("通常攻撃");
            if (actionCnt < 0) actionCnt = 0;
            actionCnt++;
            if (actionCnt % 60 == 0)
            {
                attacksStamina -= 20;
                atkStamina = 0;
                action = Action.Non;
                Debug.LogError("通常攻撃スタミナ：" + atkStamina);
            }

            return NodeStatus.SUCCESS;
        }

        /// <summary>
        /// 風圧の処理を行う
        /// </summary>
        /// <returns>NodeStatus.SUCCESS</returns>
        NodeStatus WindAction()
        {
            if (actionCnt < 0) actionCnt = 0;
            actionCnt++;
            if (actionCnt % 60 == 0)
            {
                windStamina = 20;
                attacksStamina -= 10;
                action = Action.Non;
                Debug.LogError("風スタミナ：" + windStamina);
            }

            Debug.LogError("風圧");
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
            Debug.LogError("突進");
            return NodeStatus.SUCCESS;
        }

        #endregion

    }
}
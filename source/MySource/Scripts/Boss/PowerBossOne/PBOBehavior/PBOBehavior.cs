using UnityEngine;

public partial class PBO
{
    //�U���˕����˓ːi
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
        private BehaviorTreeController _behaviorTreeController; // �R���g���[���[
        private float attacksStamina = 100;                     // �U���s���̃X�^�~�i
        private uint atkStamina = 100;                          // �ʏ�U���̃X�^�~�i
        private uint windStamina = 100;                         // �����̃X�^�~�i
        private uint lungeStamina = 100;                        // �ːi�̃X�^�~�i
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
            //�A�N�V������������
            action = Action.Non;
            _behaviorTreeController = new BehaviorTreeController();

            #region CreateNode
            // root�m�[�h�ˍU���s���p�̃X�^�~�i�����邩�̊m�F
            SelectorNode rootNode = new SelectorNode();
            rootNode.name = "root�m�[�h";

            // �U���s���p�̃X�^�~�i�����邩�̊m�F�ˊe��U���̎q�m�[�h�����m�[�h
            DecoratorNode confirmStamina = new DecoratorNode();
            confirmStamina.name = "�X�^�~�i�����邩�m�F����DecoratorNode";
            confirmStamina.SetConditionFunc(() =>{ return ConfirmStamina();});

            //�e��U���̎q�m�[�h�����m�[�h�ˊe��U���̃X�^�~�i���m�F
            SelectorNode atkAction = new SelectorNode();
            atkAction.name = "�U���A�N�V�������s���m�[�h";

            // �ʏ�U���X�^�~�i�̊m�F�˒ʏ�U�����s��
            DecoratorNode confirmAtkStamina = new DecoratorNode();
            confirmAtkStamina.name = "�ʏ�U���X�^�~�i�����邩�m�F����DecoratorNode";
            confirmAtkStamina.SetConditionFunc(() =>{ return ConfirmAtkStamina();});

            // �����X�^�~�i�̊m�F�˕������s��
            DecoratorNode confirmWindStamina = new DecoratorNode();
            confirmWindStamina.name = "�����X�^�~�i�����邩�m�F����DecoratorNode";
            confirmWindStamina.SetConditionFunc(() =>{ return ConfirmWindStamina();});

            // �ːi�X�^�~�i�̊m�F�˓ːi���s��
            DecoratorNode confirmLungeStamina = new DecoratorNode();
            confirmLungeStamina.name = "�ːi�X�^�~�i�����邩�m�F����DecoratorNode";
            confirmLungeStamina.SetConditionFunc(() =>{ return ConfirmLungeStamina();});

            //�ʏ�U�����s��
            ActionNode atk = new ActionNode();
            atk.name = "�ʏ�U�����s��ActionNode";
            atk.SetRunningFunc(() =>{ return AtkAction();});

            //�������s��
            ActionNode wind = new ActionNode();
            wind.name = "�������s��ActionNode";
            wind.SetRunningFunc(() =>{ return WindAction();});

            //�ːi���s��
            ActionNode lunge = new ActionNode();
            lunge.name = "�ːi���s��ActionNode";
            lunge.SetRunningFunc(() =>{ return LungeAction();});

            #endregion

            #region AddChild
            // �m�[�h�̎q���o�^
            rootNode.AddChild(confirmStamina);
            //             ��
            confirmStamina.AddChild(atkAction);
            //             ��
            atkAction.AddChild(confirmAtkStamina);
            atkAction.AddChild(confirmWindStamina);
            atkAction.AddChild(confirmLungeStamina);
            //             ��
            confirmAtkStamina.AddChild(atk);
            confirmWindStamina.AddChild(wind);
            confirmLungeStamina.AddChild(lunge);
            #endregion

            // �c���[���s
            _behaviorTreeController.Initialize(rootNode);
            _behaviorTreeController.OnStart();
        }

        /// <summary>
        /// �X�V����
        /// </summary>
        public void OnUpdate()
        {
            _behaviorTreeController.OnRunning();
        }


        #region NodeFunc

        /// <summary>
        /// �U���s���p�̃X�^�~�i�̍X�V�Ɗm�F
        /// </summary>
        /// <returns>status</returns>
        NodeStatus ConfirmStamina()
        {
            //�U���s���p�̃X�^�~�i�ƍs���̃X�^�~�i���񕜂���
            if (attacksStamina < 100) attacksStamina += 1;
            if (atkStamina < 100) atkStamina++;
            if (windStamina < 100) windStamina++;
            if (lungeStamina < 100) lungeStamina++;

            //�U���s���p�̃X�^�~�i�����ȏ�Ȃ琬��
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
        /// �ʏ�U���̃X�^�~�i�����邩�̊m�F
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
        /// �����̃X�^�~�i�������邩�̊m�F
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
        /// �ːi�̃X�^�~�i�̊m�F
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
        /// �ʏ�U�����̏���
        /// </summary>
        /// <returns>NodeStatus.SUCCESS</returns>
        NodeStatus AtkAction()
        {
            Debug.LogError("�ʏ�U��");
            if (actionCnt < 0) actionCnt = 0;
            actionCnt++;
            if (actionCnt % 60 == 0)
            {
                attacksStamina -= 20;
                atkStamina = 0;
                action = Action.Non;
                Debug.LogError("�ʏ�U���X�^�~�i�F" + atkStamina);
            }

            return NodeStatus.SUCCESS;
        }

        /// <summary>
        /// �����̏������s��
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
                Debug.LogError("���X�^�~�i�F" + windStamina);
            }

            Debug.LogError("����");
            return NodeStatus.SUCCESS;
        }

        /// <summary>
        /// �ːi�̏������s��
        /// </summary>
        /// <returns>NodeStatus.SUCCESS</returns>
        NodeStatus LungeAction()
        {
            //�����^�����ď���
            lungeStamina = 0;
            attacksStamina -= 100;
            action = Action.Non;
            Debug.LogError("�ːi");
            return NodeStatus.SUCCESS;
        }

        #endregion

    }
}
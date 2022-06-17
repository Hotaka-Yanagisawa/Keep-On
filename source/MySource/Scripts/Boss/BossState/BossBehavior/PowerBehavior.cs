/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/05/14
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/05/14 �쐬�J�n
//             �p���[�{�X�r�w�C�r�A�c���[���쐬
// 
//
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;
namespace Homare
{
    public partial class Boss
    {
        //�U���˕����˓ːi
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
            private BehaviorTreeController _behaviorTreeController; // �R���g���[���[
            private float attacksStamina = 100;                     // �U���s���̃X�^�~�i
            private uint bombingStamina = 100;                      // �����U���̃X�^�~�i
            private uint windStamina = 100;                         // �����̃X�^�~�i
            private uint lungeStamina = 100;                        // �ːi�̃X�^�~�i
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
                confirmStamina.SetConditionFunc(() => { return ConfirmStamina(); });

                //�e��U���̎q�m�[�h�����m�[�h�ˊe��U���̃X�^�~�i���m�F
                SelectorNode atkAction = new SelectorNode();
                atkAction.name = "�U���A�N�V�������s���m�[�h";

                // �����U���X�^�~�i�̊m�F�˒ʏ�U�����s��
                DecoratorNode confirmBombingStamina = new DecoratorNode();
                confirmBombingStamina.name = "�����U���X�^�~�i�����邩�m�F����DecoratorNode";
                confirmBombingStamina.SetConditionFunc(() => { return ConfirmBombingStamina(); });

                // �����X�^�~�i�̊m�F�˕������s��
                DecoratorNode confirmWindStamina = new DecoratorNode();
                confirmWindStamina.name = "�����X�^�~�i�����邩�m�F����DecoratorNode";
                confirmWindStamina.SetConditionFunc(() => { return ConfirmWindStamina(); });

                // �ːi�X�^�~�i�̊m�F�˓ːi���s��
                DecoratorNode confirmLungeStamina = new DecoratorNode();
                confirmLungeStamina.name = "�ːi�X�^�~�i�����邩�m�F����DecoratorNode";
                confirmLungeStamina.SetConditionFunc(() => { return ConfirmLungeStamina(); });

                //�ʏ�U�����s��
                ActionNode atk = new ActionNode();
                atk.name = "�ʏ�U�����s��ActionNode";
                atk.SetRunningFunc(() => { return BombingAction(); });

                //�������s��
                ActionNode wind = new ActionNode();
                wind.name = "�������s��ActionNode";
                wind.SetRunningFunc(() => { return WindAction(); });

                //�ːi���s��
                ActionNode lunge = new ActionNode();
                lunge.name = "�ːi���s��ActionNode";
                lunge.SetRunningFunc(() => { return LungeAction(); });

                #endregion

                #region AddChild
                // �m�[�h�̎q���o�^
                rootNode.AddChild(confirmStamina);
                //             ��
                confirmStamina.AddChild(atkAction);
                //             ��
                atkAction.AddChild(confirmBombingStamina);
                atkAction.AddChild(confirmWindStamina);
                atkAction.AddChild(confirmLungeStamina);
                //             ��
                confirmBombingStamina.AddChild(atk);
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
                if (bombingStamina < 100) bombingStamina++;
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
            /// �����U���̃X�^�~�i�����邩�̊m�F
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
            /// �����U�����̏���
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
            /// �����̏������s��
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus WindAction()
            {
                if (actionCnt < 0)
                {
                    Debug.Log("����");
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
            /// �ːi�̏������s��
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus LungeAction()
            {
                //�����^�����ď���
                lungeStamina = 0;
                attacksStamina -= 100;
                action = Action.Non;
                Debug.Log("�ːi");
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
            /// �v���C���[����Ƃ����G�l�~�[�̈ړ�
            /// </summary>
            /// <param name="approach">�߂Â�����苗����ۂ�</param>
            /// <param name="moveSpeedControl">�ړ��X�s�[�h�̒���</param>
            void MoveForPlayer(bool approach, float moveSpeedControl = 1)
            {
                float Angle = Mathf.Atan2(getBoss.player.transform.position.z - getBoss.transform.position.z,
                      getBoss.player.transform.position.x - getBoss.transform.position.x);

                getBoss.rb.velocity = new Vector3(Mathf.Cos(Angle), getBoss.rb.velocity.y, Mathf.Sin(Angle));
                //getBoss.velocity.x = Mathf.Cos(Angle);
                //getBoss.velocity.z = Mathf.Sin(Angle);
                // ���x�x�N�g���̒�����1�b��moveSpeed�����i�ނ悤�ɒ������܂�
                getBoss.rb.velocity = getBoss.rb.velocity.normalized * getBoss.moveSpeed * moveSpeedControl;

                getBoss.animator.SetFloat("Speed", getBoss.rb.velocity.magnitude);

                // �����ꂩ�̕����Ɉړ����Ă���ꍇ
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
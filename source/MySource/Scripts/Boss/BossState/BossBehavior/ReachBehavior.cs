/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/05/10
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/05/10 �쐬�J�n
//             Reach�{�X�r�w�C�r�A�c���[���쐬
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
            private BehaviorTreeController _behaviorTreeController; // �R���g���[���[
            private float attacksStamina = 100;                     // �U���s���̃X�^�~�i
            //private uint cMoveStamina = 100;                      // �����ړ��̃X�^�~�i
            private uint bombingStamina = 100;                      // �����U���̃X�^�~�i
            private uint windStamina = 100;                         // �����̃X�^�~�i
            private uint laserStamina = 100;                        // �ːi�̃X�^�~�i
            //private int AtkCnt;                                     // �U������ڂ�
            //private int AtkEndNum;                                  // �I����(�U��)
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
                
                // �����ړ��X�^�~�i�̊m�F�˒ʏ�U�����s��
                //DecoratorNode confirmCMoveStamina = new DecoratorNode();
                //confirmCMoveStamina.name = "�����U���X�^�~�i�����邩�m�F����DecoratorNode";
                //confirmCMoveStamina.SetConditionFunc(() => { return ConfirmCMoveStamina(); });

                // �����U���X�^�~�i�̊m�F�˒ʏ�U�����s��
                DecoratorNode confirmBombingStamina = new DecoratorNode();
                confirmBombingStamina.name = "�����U���X�^�~�i�����邩�m�F����DecoratorNode";
                confirmBombingStamina.SetConditionFunc(() => { return ConfirmBombingStamina(); });

                // �����X�^�~�i�̊m�F�˕������s��
                DecoratorNode confirmWindStamina = new DecoratorNode();
                confirmWindStamina.name = "�����X�^�~�i�����邩�m�F����DecoratorNode";
                confirmWindStamina.SetConditionFunc(() => { return ConfirmWindStamina(); });

                // �ːi�X�^�~�i�̊m�F�˓ːi���s��
                DecoratorNode confirmLaserStamina = new DecoratorNode();
                confirmLaserStamina.name = "�ːi�X�^�~�i�����邩�m�F����DecoratorNode";
                confirmLaserStamina.SetConditionFunc(() => { return ConfirmLaserStamina(); });

                //�����U�����s��
                ActionNode atk = new ActionNode();
                atk.name = "�ʏ�U�����s��ActionNode";
                atk.SetRunningFunc(() => { return BombingAction(); });

                //�������s��
                ActionNode wind = new ActionNode();
                wind.name = "�������s��ActionNode";
                wind.SetRunningFunc(() => { return WindAction(); });

                //�ːi���s��
                ActionNode laser = new ActionNode();
                laser.name = "�ːi���s��ActionNode";
                laser.SetRunningFunc(() => { return LaserAction(); });

                #endregion

                #region AddChild
                // �m�[�h�̎q���o�^
                rootNode.AddChild(confirmStamina);
                //             ��
                confirmStamina.AddChild(atkAction);
                //             ��
                atkAction.AddChild(confirmBombingStamina);
                atkAction.AddChild(confirmWindStamina);
                atkAction.AddChild(confirmLaserStamina);
                //             ��
                confirmBombingStamina.AddChild(atk);
                confirmWindStamina.AddChild(wind);
                confirmLaserStamina.AddChild(laser);
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
                if (laserStamina < 100) laserStamina++;

                //�U���s���p�̃X�^�~�i�����ȏ�Ȃ琬��
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
            /// �����U�����̏���
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus BombingAction()
            {
                //�m�[�h�ɗ��čŏ��̏���
                if (actionCnt < 0)
                {
                    //�J�E���g�̏������A�`���[�W���[�V�����A�t���O�̏�����
                    actionCnt = 0;
                    getBoss.animator.SetInteger("AtkType", 0);
                    getBoss.animator.SetBool("Attack", true);
                    getBoss.isAtk = false;
                }

                //�֌W���郂�[�V�������ǂ���
                if (getBoss.animator.GetCurrentAnimatorStateInfo(0).IsTag("extinction"))
                {
                    //�܂��U���t���O�������ĂȂ��U�����[�V�������Ȃ�
                    if (getBoss.animator.GetInteger("AtkType") == 1 && !getBoss.isAtk)
                    {
                        //�U���J�n�A�U���t���O���Ă�
                        SetWind(true, 40);
                        SetExtinction(true);
                        getBoss.isAtk = true;
                    }
                    //�U�����Ȃ�J�E���g��i�߂�
                    else if(getBoss.isAtk) actionCnt += Time.deltaTime;
                    //�J�E���g���i�񂾂�end���[�V�����ֈڍs
                    if(actionCnt > 5) getBoss.animator.SetInteger("AtkType", 2);
                    //end���[�V�����ōU���t���O�������Ă�����I��������
                    if (getBoss.animator.GetInteger("AtkType") == 2 && getBoss.isAtk)
                    {
                        //���[�V������ύX
                        getBoss.animator.SetBool("Attack", false);
                        //�U���I��
                        SetExtinction(false);
                        SetWind(false, 40);
                        //��O�l��
                        actionCnt = -1;
                        action = Action.Non;
                        //�X�^�~�i���炷
                        attacksStamina -= 50;
                        bombingStamina = 0;
                        //���`�F���W���ɕʂ̂���
                        getBoss.ChangeStateMachine(BossType.Speed);
                    }
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
                    //������Wind��ON�ɂ���
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
            /// �ːi�̏������s��
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus LaserAction()
            {
                if (actionCnt < 0)
                {
                    //Debug.Log("�����U��");
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
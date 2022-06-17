/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/05/14
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/05/14 �쐬�J�n
//             Speed�{�X�r�w�C�r�A�c���[���쐬
//              
//
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
namespace Homare
{
    public partial class Boss
    {
        //�U���˒��i�U���˃W�����v�U��
        public class SpeedBehavior
        {
            public enum Action
            {
                Non,
                //Jump,               // �W�����v
                //StraightAttack,     // ���i�U��
                SwoopAttack,        // �W�����v�U��
            }

            #region PublicVariable
            public Action action;
            public float actionCnt = -1;
            #endregion

            #region PrivateVariable
            private Boss getBoss;
            private BehaviorTreeController _behaviorTreeController; // �R���g���[���[
            private float attacksStamina = 100;                     // �U���s���̃X�^�~�i
            private uint jumpAtkStamina = 100;                      // �W�����v�U���X�^�~�i
            //private uint jumpingStamina = 100;                      // �W�����v�̃X�^�~�i
            //private uint stAtkStamina = 100;                        // ���i�U���̃X�^�~�i
            //private int AtkCnt;                                     // �U������ڂ�
            //private int AtkEndNum;                                  // �I����(�U��)
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

                // �W�����v�U���X�^�~�i�̊m�F�˃W�����v�U�����s��
                DecoratorNode confirmJumpAtkStamina = new DecoratorNode();
                confirmJumpAtkStamina.name = "�W�����v�U���X�^�~�i�����邩�m�F����DecoratorNode";
                confirmJumpAtkStamina.SetConditionFunc(() => { return ConfirmJumpAtkStamina(); });
                
                // �W�����v�X�^�~�i�̊m�F�˒ʏ�U�����s��
                //DecoratorNode confirmJumpingStamina = new DecoratorNode();
                //confirmJumpingStamina.name = "�W�����v�X�^�~�i�����邩�m�F����DecoratorNode";
                //confirmJumpingStamina.SetConditionFunc(() => { return ConfirmJumpingStamina(); });

                //// ���i�U���X�^�~�i�̊m�F�˒��i�U�����s��
                //DecoratorNode confirmStAtkStamina = new DecoratorNode();
                //confirmStAtkStamina.name = "���i�U���X�^�~�i�����邩�m�F����DecoratorNode";
                //confirmStAtkStamina.SetConditionFunc(() => { return ConfirmStAtkStamina(); });


                //�W�����v�U�����s��
                ActionNode jumpAtk = new ActionNode();
                jumpAtk.name = "�W�����v�U�����s��ActionNode";
                jumpAtk.SetRunningFunc(() => { return JumpAtkAction(); });

                ////�W�����v���s��
                //ActionNode jump = new ActionNode();
                //jump.name = "�ʏ�U�����s��ActionNode";
                //jump.SetRunningFunc(() => { return JumpAction(); });

                ////���i�U�����s��
                //ActionNode stAtk = new ActionNode();
                //stAtk.name = "���i�U�����s��ActionNode";
                //stAtk.SetRunningFunc(() => { return stAtkAction(); });


                #endregion

                #region AddChild
                // �m�[�h�̎q���o�^
                rootNode.AddChild(confirmStamina);
                //             ��
                confirmStamina.AddChild(atkAction);
                //             ��
                atkAction.AddChild(confirmJumpAtkStamina);
                //atkAction.AddChild(confirmJumpingStamina);
                //atkAction.AddChild(confirmStAtkStamina);
                //             ��
                confirmJumpAtkStamina.AddChild(jumpAtk);
                //confirmJumpingStamina.AddChild(jump);
                //confirmStAtkStamina.AddChild(stAtk);
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
                //if (jumpAtkStamina < 100) jumpAtkStamina++;
                //if (jumpingStamina < 100) jumpingStamina++;
                //if (stAtkStamina < 100) stAtkStamina++;

                //�U���s���p�̃X�^�~�i�����ȏ�Ȃ琬��
                NodeStatus status = attacksStamina > 99 ? NodeStatus.SUCCESS : NodeStatus.WAITING;

                return status;
            }

            /// <summary>
            /// �W�����v�̃X�^�~�i�����邩�̊m�F
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
            /// �W�����v�U���̃X�^�~�i�̊m�F
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
            /// ���i�U���̃X�^�~�i�������邩�̊m�F
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
            /// �W�����v�U���̏������s��
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
            /// �W�����v���̏���
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
            /// ���i�U���̏������s��
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            //NodeStatus stAtkAction()
            //{
            //    if (actionCnt < 0 && !isCharge)
            //    {
            //        actionCnt = 0;
            //        //�`���[�W���[�V����
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
            //            //�p���`���[�V����
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
            //    // �U�����̋@���U�R�̃X�s�[�h�X�V
            //    getBoss.rb.velocity = getBoss.rb.velocity.normalized * getBoss.moveSpeed * 2;

            //    // Hit����
            //    speedEnemy.colliders[(int)ColliderKind.AttackCollider].enabled = true;
            //    // Body�̓����蔻��
            //    Physics.IgnoreCollision(speedEnemy.colliders[(int)ColliderKind.BodyCollider],
            //                            SpeedEnemy.playerCollider, true);

            //    Vector3 playerPos = getBoss.transform.position;

            //    playerPos.y = getBoss.transform.position.y;

            //    // �G�̊p�x�̍X�V
            //    // Slerp:���݂̌����A�������������A��������X�s�[�h
            //    // LookRotation(������������):
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
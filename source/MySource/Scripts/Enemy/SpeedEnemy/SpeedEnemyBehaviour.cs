//////////////////////////////
// SpeedEnemyBehaviour.cs
//----------------------------
// �쐬��:2021/4/25 
// �쐬��:�v�c���M
//----------------------------
// �X�V�����E���e
//  �E�X�N���v�g�쐬
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
            private BehaviorTreeController _behaviorTreeController; // �R���g���[���[
            private float attacksStamina = 0;                       // �U���s���̃X�^�~�i
            #endregion

            #region �N���^�̋���
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
                //�A�N�V������������
                _behaviorTreeController = new BehaviorTreeController();

                

                #region CreateNode
                // root�m�[�h�ˍU���s���p�̃X�^�~�i�����邩�̊m�F
                SelectorNode rootNode = new SelectorNode();
                rootNode.name = "root�m�[�h";

                // �U���s���p�̃X�^�~�i�����邩�̊m�F�ˊe��U���̎q�m�[�h�����m�[�h
                DecoratorNode confirmStamina = new DecoratorNode();
                confirmStamina.name = "Speed:�X�^�~�i�����邩�m�F����DecoratorNode";
                confirmStamina.SetConditionFunc(() => { return ConfirmStamina(); });

                //�ʏ�U�����s��
                ActionNode atk = new ActionNode();
                atk.name = "Speed:�ʏ�U�����s��ActionNode";
                atk.SetRunningFunc(() => { return AtkAction(); });

                //�v���C���[�ƌ����ċ��������
                ActionNode move = new ActionNode();
                move.name = "Speed:�v���C���[�ƌ����ċ��������ActionNode";
                move.SetRunningFunc(() => { return MoveAction(); });

                #endregion

                #region AddChild
                // �m�[�h�̎q���o�^
                rootNode.AddChild(confirmStamina);
                rootNode.AddChild(move);
                //             ��
                confirmStamina.AddChild(atk);

                #endregion

                // �c���[���s
                _behaviorTreeController.Initialize(rootNode);
                _behaviorTreeController.OnStart();
            }

            #region �X�V����
            public void OnUpdate()
            {
                _behaviorTreeController.OnRunning();
            }
            #endregion

            #region NodeFunc 
            /// <summary>
            /// �U���s���p�̃X�^�~�i�̍X�V�Ɗm�F
            /// </summary>
            /// <returns>�l����</returns>
            NodeStatus ConfirmStamina()
            {
                //�U���s���p�̃X�^�~�i�ƍs���̃X�^�~�i���񕜂���
                if (attacksStamina < 100) attacksStamina += 30.0f / 60.0f;

                //�U���s���p�̃X�^�~�i�����ȏ�Ȃ琬��
                return attacksStamina > 99 ? NodeStatus.SUCCESS : NodeStatus.WAITING;
            }

            /// <summary>
            /// �ʏ�U�����̏���
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus AtkAction()
            {

                Vector3 moveDistance = getEnemy.transform.position - SpeedEnemy.player.transform.position;
                //actionCnt��-1�ł������ꍇ
                //�����ɍŏ��̃t���[���̏������s��
                if (actionCnt < 0)
                {
                    getEnemy.animator.SetBool("AttackEnd", false);
                    actionCnt = 0;
                }
                //�v���C���[�ƈ�苗������Ă���߂Â�
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
            /// �v���C���[�ƌ����ċ�������鏈��
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus MoveAction()
            {
                Vector3 moveDistance = getEnemy.transform.position - SpeedEnemy.player.transform.position;
                //�v���C���[�ƈ�苗���߂Â��Ă��狗�������
                if (5 > moveDistance.magnitude)
                {
                    MoveForPlayer(false, 0.5f);
                }
                //�v���C���[�ƈ�苗������Ă���߂Â�
                else if (5.1f < moveDistance.magnitude)
                {
                    MoveForPlayer(true);
                }
                //���ڂ͎~�܂�
                else
                {
                    getEnemy.rb.velocity = Vector3.zero;
                    circleMove();
                }
                return NodeStatus.SUCCESS;
            }
            #endregion

            /// <summary>
            /// �v���C���[����Ƃ����G�l�~�[�̈ړ�
            /// </summary>
            /// <param name="approach">�߂Â�����苗����ۂ�</param>
            /// <param name="moveSpeedControl">�ړ��X�s�[�h�̒���</param>
            void MoveForPlayer(bool approach, float moveSpeedControl = 1)
            {
                float Angle = Mathf.Atan2(SpeedEnemy.player.transform.position.z - getEnemy.transform.position.z,
                      SpeedEnemy.player.transform.position.x - getEnemy.transform.position.x);

                getEnemy.rb.velocity = new Vector3(Mathf.Cos(Angle), getEnemy.rb.velocity.y, Mathf.Sin(Angle));
                // ���x�x�N�g���̒�����1�b��moveSpeed�����i�ނ悤�ɒ������܂�
                getEnemy.rb.velocity = getEnemy.rb.velocity.normalized * getEnemy.enemyStatus.moveSpeed * moveSpeedControl;

                getEnemy.animator.SetFloat("Speed", getEnemy.rb.velocity.magnitude);

                // �����ꂩ�̕����Ɉړ����Ă���ꍇ
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
            /// �v���C���[�𒆐S�ɉ~��`���Ĉړ�
            /// </summary>
            void circleMove()
            {
              
            }

        }
    }

}
// �p���[�^�G���G�̍U������p�̃r�w�C�r�A�H


using UnityEngine;


namespace Homare
{
    public partial class PowerEnemy
    {
        //�U���˕����˓ːi
        public class PowerEnemyBehavior
        {

            #region PublicVariable
            //public Action action;
            public float actionCnt;
            #endregion

            #region PrivateVariable
            private PowerEnemy getEnemy;
            private BehaviorTreeController _behaviorTreeController; // �R���g���[���[
            private float attacksStamina = 0;                       // �U���s���̃X�^�~�i
            #endregion

            public PowerEnemyBehavior(PowerEnemy external)
            {
                getEnemy = external;
            }

            public void OnEnter()
            {
                actionCnt -= 1;
                //action = Action.Non;
            }

            public void OnStart()
            {
                //�A�N�V������������
                //action = Action.Non;
                _behaviorTreeController = new BehaviorTreeController();

                #region CreateNode
                // root�m�[�h�ˍU���s���p�̃X�^�~�i�����邩�̊m�F
                SelectorNode rootNode = new SelectorNode();
                rootNode.name = "root�m�[�h";

                // �U���s���p�̃X�^�~�i�����邩�̊m�F�ˊe��U���̎q�m�[�h�����m�[�h
                DecoratorNode confirmStamina = new DecoratorNode();
                confirmStamina.name = "�X�^�~�i�����邩�m�F����DecoratorNode";
                confirmStamina.SetConditionFunc(() => { return ConfirmStamina(); });

                //�ʏ�U�����s��
                ActionNode atk = new ActionNode();
                atk.name = "�ʏ�U�����s��ActionNode";
                atk.SetRunningFunc(() => { return AtkAction(); });

                //�v���C���[�ƌ����ċ��������
                ActionNode move = new ActionNode();
                move.name = "�v���C���[�ƌ����ċ��������ActionNode";
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
                Vector3 moveDistance = getEnemy.transform.position - PowerEnemy.player.transform.position;
                //actionCnt��-1�ł������ꍇ
                //�����ɍŏ��̃t���[���̏������s��
                if (actionCnt < 0)
                {
                    //getEnemy.animator.SetBool("AttackEnd", false);
                    actionCnt = 0;
                }
                //�v���C���[�ƈ�苗������Ă���߂Â�
                if (1.5f < moveDistance.magnitude)
                {
                    MoveForPlayer(true);

                }

                getEnemy.animator.SetBool("Attack", true);


                actionCnt+=  Time.deltaTime;
                if (actionCnt > 2)
                {
                    actionCnt = -1;
                    attacksStamina = 0;
                    getEnemy.animator.SetBool("Attack", false);
                    //getEnemy.animator.SetBool("AttackEnd", true);
                    getEnemy.weaponDamage.Collider.enabled = false;
                }

                return NodeStatus.SUCCESS;
            }

            /// <summary>
            /// �v���C���[�ƌ����ċ�������鏈��
            /// </summary>
            /// <returns>NodeStatus.SUCCESS</returns>
            NodeStatus MoveAction()
            {
                Vector3 moveDistance = getEnemy.transform.position - PowerEnemy.player.transform.position;
                //�v���C���[�ƈ�苗���߂Â��Ă��狗�������
                if (3.5f > moveDistance.magnitude)
                {
                    MoveForPlayer(false, 0.5f);
                }
                //�v���C���[�ƈ�苗������Ă���߂Â�
                else if (3.6f < moveDistance.magnitude)
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
                float Angle = Mathf.Atan2(PowerEnemy.player.transform.position.z - getEnemy.transform.position.z,
                      PowerEnemy.player.transform.position.x - getEnemy.transform.position.x);

                getEnemy.rb.velocity = new Vector3(Mathf.Cos(Angle),getEnemy.rb.velocity.y, Mathf.Sin(Angle));
                //getEnemy.velocity.x = Mathf.Cos(Angle);
                //getEnemy.velocity.z = Mathf.Sin(Angle);
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
            //static int angle = 0;
            /// <summary>
            /// �v���C���[�𒆐S�ɉ~��`���Ĉړ�
            /// </summary>
            void circleMove()
            {
                //angle++;
                //var angleAxis = Quaternion.AngleAxis(angle, Vector3.down);

                //getEnemy.transform.position = angleAxis * getEnemy.transform.position;
            }

        }
    }
}